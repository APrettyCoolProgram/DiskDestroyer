// 261109_code
// 261109_documentation

namespace DiskDestroyer;

/// <summary>Entry class for DiskDestroyer.</summary>
/// <remarks>
/// Performs a destructive disk-fill operation and includes safety prompts, input validation, and basic
/// exception handling for cancellation, I/O failures, and drive access errors.
/// </remarks>
static class Program
{
    /// <summary>Entry method for DiskDestroyer.</summary>
    /// <remarks>
    /// This does the heavy lifting.
    /// </remarks>
    /// <param name="args">Command-line arguments.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal static async Task Main(string[] args)
    {
        string? currentDrive = Path.GetPathRoot(Environment.CurrentDirectory);

        VerifyDriveExists(currentDrive);

        char driveLetter = currentDrive[0];

        if (!TryParseChunkSizeArgument(args, out long chunkSizeInBytes, out string? errorMessage))
        {
            Console.WriteLine(errorMessage);

            Environment.Exit(1);
        }

        await ConfirmAndProceedAsync(driveLetter);

        try
        {
            var result = await FileWriter.WriteUntilDiskFullAsync(driveLetter: driveLetter, chunkSize: chunkSizeInBytes, maxFillPercentage: 95.0);

            Console.WriteLine($"{Environment.NewLine}=== Write Complete ==={Environment.NewLine}");
            Console.WriteLine($"Total bytes written: {result.TotalBytesWritten:N0} ({(double)result.TotalBytesWritten / (1024 * 1024 * 1024):F2} GB)");
            Console.WriteLine($"Final disk usage: {result.FinalDiskUsagePercent:F2}%");
            Console.WriteLine($"Statistics recorded: {result.Statistics.Count} entries");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"{Environment.NewLine}Operation cancelled: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"{Environment.NewLine}I/O Error: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"{Environment.NewLine}Drive Info Error: {ex.Message}");
        }
    }

    /// <summary>Parses an optional chunk size argument in megabytes and converts it to bytes.</summary>
    /// <remarks>
    /// A leading '-' on the first argument is ignored before parsing.
    /// </remarks>
    /// <param name="args">Command-line arguments where the first value may specify the chunk size in MB.</param>
    /// <param name="chunkSizeInBytes">When parsing succeeds, contains the chunk size in bytes; defaults to 1 MB when no argument is provided.</param>
    /// <param name="errorMessage">When parsing fails, contains a validation error message; otherwise <see langword="null"/>.</param>
    /// <returns><see langword="true"/> when the argument is missing or contains a valid positive integer chunk size; otherwise,
    /// <see langword="false"/>.</returns>
    internal static bool TryParseChunkSizeArgument(string[] args, out long chunkSizeInBytes, out string? errorMessage)
    {
        const long oneMegabyte = 1024 * 1024;

        chunkSizeInBytes = oneMegabyte;
        errorMessage     = null;

        if (args.Length == 0)
        {
            return true;
        }

        string rawArgument = args[0].Trim();

        if (rawArgument.StartsWith("-"))
        {
            rawArgument = rawArgument[1..];
        }

        if (!int.TryParse(rawArgument, out int chunkSizeInMb) || chunkSizeInMb <= 0)
        {
            errorMessage = $"Invalid chunk size argument '{args[0]}'. Provide a positive number of MB, e.g. DiskDestroyer -8.";

            return false;
        }

        chunkSizeInBytes = chunkSizeInMb * oneMegabyte;

        Console.WriteLine($"Using chunk size: {chunkSizeInMb} MB ({chunkSizeInBytes:N0} bytes)");

        return true;
    }

    /// <summary>Confirms with user before proceeding with destructive operation.</summary>
    internal static async Task ConfirmAndProceedAsync(char driveLetter)
    {
        var msg = $"{Environment.NewLine}" +
                  $"WARNING: This will destroy data on drive {driveLetter}!{Environment.NewLine}" +
                  $"{Environment.NewLine}" +
                  $"This action is IRREVERSIBLE and may cause system instability.{Environment.NewLine}" +
                  $"{Environment.NewLine}" +
                  $"Are you absolutely sure you want to continue? (y/n){Environment.NewLine}";

        Console.WriteLine(msg);
        Console.WriteLine("Enter your choice: ");

        var choice = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(choice))
        {
            Console.WriteLine($"{Environment.NewLine}No input provided. Operation cancelled.{Environment.NewLine}");
            Environment.Exit(0);
        }

        if (choice?.ToLower() != "y")
        {
            Console.WriteLine($"{Environment.NewLine}Operation cancelled by user.{Environment.NewLine}");
            Environment.Exit(0);
        }

        var driveInfo = new DriveInfo(driveLetter.ToString());

        if (!driveInfo.IsReady)
        {
            throw new InvalidOperationException($"Drive {driveLetter} is not accessible.");
        }

        Console.WriteLine($"\n✓ Proceeding with write operation on {driveInfo.Name}...");
    }

    /// <summary>Validates that the current drive value is available before continuing execution.</summary>
    /// <remarks>
    /// Writes an error message to standard output and terminates the process with exit code 1 when
    /// <paramref name="currentDrive"/> is <see langword="null"/>.
    /// </remarks>
    /// <param name="currentDrive">The drive value to validate.</param>
    private static void VerifyDriveExists(string currentDrive)
    {
        if (currentDrive is null)
        {
            Console.WriteLine($"[ERROR] Drive {currentDrive} does not exist.");

            Environment.Exit(1);
        }
    }
}