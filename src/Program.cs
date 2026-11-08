// 261107_code
// 261107_documentation

namespace DiskDestroyer;

class Program
{
    static async Task Main(string[] args)
    {
        string? currentDrive = Path.GetPathRoot(Environment.CurrentDirectory);

        if (currentDrive is null)
        {
            Console.WriteLine("Error: Could not determine current drive.");
            Environment.Exit(1);
        }

        char driveLetter = currentDrive[0];

        if (!TryParseChunkSizeArgument(args, out long chunkSizeInBytes, out string? errorMessage))
        {
            Console.WriteLine(errorMessage);
            Environment.Exit(1);
        }

        await ConfirmAndProceedAsync(driveLetter);

        try
        {
            var result = await FileWriter.WriteUntilDiskFullAsync(
                driveLetter: driveLetter,
                chunkSize: chunkSizeInBytes,
                maxFillPercentage: 95.0);

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

    /// <summary>
    /// Parses an optional chunk size argument where the value is expressed in MB.
    /// Example: DiskDestroyer -8 => 8 MB chunks.
    /// </summary>
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

    /// <summary>
    /// Confirms with user before proceeding with destructive operation.
    /// </summary>
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
}