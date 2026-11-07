// 261107_code
// 261107_documentation

namespace DiskDestroyer;

public static class FileWriter
{
    /// <summary>
    /// Writes data to a file until the disk is nearly full.
    /// Includes safety limits to prevent system crashes.
    /// </summary>
    /// <param name="driveLetter">Drive letter (e.g., 'C')</param>
    /// <param name="chunkSize">Size of each write chunk in bytes (default: 1MB)</param>
    /// <param name="maxFillPercentage">Maximum disk usage percentage before stopping (default: 95%)</param>
    /// <param name="cancellationToken">Cancellation token for async operations</param>
    /// <returns>Total bytes written and final disk usage statistics</returns>
    public static async Task<DiskWriteResult> WriteUntilDiskFullAsync(char driveLetter, long chunkSize = 1024 * 1024, double maxFillPercentage = 95.0, CancellationToken cancellationToken = default)
    {
        string filePath = $@"{driveLetter}:\disk_destroyer.dat";
        string msg      = $"Writing to {filePath}";

        Console.WriteLine(msg);

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Dispose();
        }

        var result              = new DiskWriteResult();
        long totalWritten       = 0;
        long currentFileSize    = 0;
        DiskSpaceInfo? diskInfo = null;

        try
        {
            using var fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);

            while (!cancellationToken.IsCancellationRequested)
            {
                diskInfo = GetDiskSpace(filePath);

                if (diskInfo == null || diskInfo.TotalSize <= 0)
                {
                    throw new InvalidOperationException("Unable to determine disk size");
                }

                double currentUsagePercent = (double)(currentFileSize + chunkSize) / diskInfo.TotalSize * 100;

                if (currentUsagePercent >= maxFillPercentage)
                {
                    Console.WriteLine($"Reached target: {maxFillPercentage}% usage");
                    break;
                }

                byte[] buffer  = new byte[chunkSize];
                var randomData = GenerateRandomBuffer(buffer);

                await fileStream.WriteAsync(randomData, cancellationToken);

                currentFileSize += chunkSize;
                totalWritten    += chunkSize;

                result.Statistics.Add(new DiskWriteStatistics(timestamp: DateTime.UtcNow, bytesWritten: chunkSize, cumulativeBytes: currentFileSize, diskUsagePercent: currentUsagePercent));

                if (totalWritten % (chunkSize * 10) == 0 || totalWritten >= 1024L * 1024 * 1024)
                {
                    Console.WriteLine($"Progress: {currentFileSize:N0} bytes ({(double)currentFileSize / diskInfo.TotalSize:P2})");
                }

                await Task.Delay(10, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Write operation cancelled by user.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"I/O error: {ex.Message}");

            throw;
        }

        result.TotalBytesWritten = totalWritten;

        result.FinalDiskUsagePercent = (currentFileSize > 0 && diskInfo != null)
            ? (double)currentFileSize / diskInfo.TotalSize * 100
            : 0;

        return result;
    }

    /// <summary>
    /// Get available and total disk space for the drive containing the file.
    /// </summary>
    private static DiskSpaceInfo? GetDiskSpace(string filePath)
    {
        try
        {
            var drive = Path.GetPathRoot(filePath);

            if (string.IsNullOrEmpty(drive))
            {
                return null;
            }

            DriveInfo driveInfo = new(drive);

            return new DiskSpaceInfo(totalSize: driveInfo.TotalSize, freeSpace: driveInfo.AvailableFreeSpace, usedSpace: driveInfo.TotalSize - driveInfo.AvailableFreeSpace);
        }
        catch (Exception)
        {
            var drive = Path.GetPathRoot(filePath);

            return new DiskSpaceInfo(totalSize: 0, freeSpace: 0, usedSpace: 0);
        }
    }

    /// <summary>
    /// Generate random data for writing to avoid patterns.
    /// </summary>
    private static byte[] GenerateRandomBuffer(byte[] buffer)
    {
        var rng = new Random();

        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = (byte)(rng.Next(256));
        }

        return buffer;
    }

    /// <summary>
    /// Result containing write statistics.
    /// </summary>
    public class DiskWriteResult
    {
        public long TotalBytesWritten { get; set; }
        public double FinalDiskUsagePercent { get; set; }
        public List<DiskWriteStatistics> Statistics { get; set; } = new();
        public void AddStatistics(DiskWriteStatistics stat) => Statistics.Add(stat);
    }

    /// <summary>
    /// Individual write statistics entry.
    /// </summary>
    public class DiskWriteStatistics(DateTime timestamp, long bytesWritten, long cumulativeBytes, double diskUsagePercent)
    {
        public DateTime Timestamp { get; set; } = timestamp;
        public long BytesWritten { get; set; } = bytesWritten;
        public long CumulativeBytes { get; set; } = cumulativeBytes;
        public double DiskUsagePercent { get; set; } = diskUsagePercent;
    }

    /// <summary>
    /// Simple disk space information.
    /// </summary>
    private class DiskSpaceInfo(long totalSize, long freeSpace, long usedSpace)
    {
        public long TotalSize { get; set; } = totalSize;
        public long FreeSpace { get; set; } = freeSpace;
        public long UsedSpace { get; set; } = usedSpace; // Correct assignment
    }
}
