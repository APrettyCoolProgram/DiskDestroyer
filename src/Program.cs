// 261107_code
// 261107_documentation

using System;
using System.IO;
using System.Threading.Tasks;

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

        await ConfirmAndProceedAsync(driveLetter);

        try
        {
            var result = await FileWriter.WriteUntilDiskFullAsync(
                driveLetter: driveLetter,
                chunkSize: 1024 * 1024,      // 1MB chunks
                maxFillPercentage: 95.0,     // Stop at 95% usage (safety limit)
                cancellationToken: new CancellationTokenSource(TimeSpan.FromMinutes(30)).Token);

            Console.WriteLine($"\n=== Write Complete ===");
            Console.WriteLine($"Total bytes written: {result.TotalBytesWritten:N0} ({(double)result.TotalBytesWritten / (1024 * 1024 * 1024):F2} GB)");
            Console.WriteLine($"Final disk usage: {result.FinalDiskUsagePercent:F2}%");
            Console.WriteLine($"Statistics recorded: {result.Statistics.Count} entries");
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"\nOperation cancelled: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"\nI/O Error: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\nDisk Info Error: {ex.Message}");
        }
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
