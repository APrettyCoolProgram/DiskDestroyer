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
        if (chunkSize <= 0 || chunkSize > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(chunkSize), "Chunk size must be between 1 byte and Int32.MaxValue.");
        }

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

        const int diskCheckIntervalChunks = 16;
        const int statsIntervalChunks     = 8;
        const int progressIntervalChunks  = 32;

        int chunkCount = 0;

        try
        {
            using var fileStream = new FileStream(
                filePath,
                FileMode.Append,
                FileAccess.Write,
                FileShare.Read,
                bufferSize: 1024 * 1024,
                options: FileOptions.Asynchronous | FileOptions.SequentialScan);

            currentFileSize = fileStream.Length;

            var buffer = new byte[(int)chunkSize];

            while (!cancellationToken.IsCancellationRequested)
            {
                if (chunkCount % diskCheckIntervalChunks == 0)
                {
                    diskInfo = GetDiskSpace(filePath);

                    if (diskInfo == null || diskInfo.TotalSize <= 0)
                    {
                        throw new InvalidOperationException("Unable to determine disk size");
                    }
                }

                long projectedUsedSpace = diskInfo!.UsedSpace + chunkSize;
                double currentUsagePercent = (double)projectedUsedSpace / diskInfo.TotalSize * 100;

                if (currentUsagePercent >= maxFillPercentage)
                {
                    Console.WriteLine($"Reached target: {maxFillPercentage}% usage");
                    break;
                }

                Random.Shared.NextBytes(buffer);
                await fileStream.WriteAsync(buffer, cancellationToken);

                currentFileSize += chunkSize;
                totalWritten    += chunkSize;
                chunkCount++;

                if (chunkCount % statsIntervalChunks == 0)
                {
                    result.Statistics.Add(new DiskWriteStatistics(
                        timestamp: DateTime.UtcNow,
                        bytesWritten: chunkSize,
                        cumulativeBytes: currentFileSize,
                        diskUsagePercent: currentUsagePercent));
                }

                if (chunkCount % progressIntervalChunks == 0)
                {
                    Console.WriteLine(
        $"Progress: file={currentFileSize:N0} bytes, disk={(double)diskInfo.UsedSpace / diskInfo.TotalSize:P2}");
                }
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

        // after the try/catch, refresh disk info before final statistics
        diskInfo ??= GetDiskSpace(filePath);
        if (diskInfo != null && diskInfo.TotalSize > 0)
        {
            diskInfo = GetDiskSpace(filePath) ?? diskInfo;
        }

        if (diskInfo != null && diskInfo.TotalSize > 0 &&
            (result.Statistics.Count == 0 || result.Statistics[^1].CumulativeBytes != currentFileSize))
        {
            result.Statistics.Add(new DiskWriteStatistics(
                timestamp: DateTime.UtcNow,
                bytesWritten: 0,
                cumulativeBytes: currentFileSize,
                diskUsagePercent: (double)diskInfo.UsedSpace / diskInfo.TotalSize * 100));
        }

        result.TotalBytesWritten = totalWritten;

        result.FinalDiskUsagePercent = (diskInfo != null && diskInfo.TotalSize > 0)
            ? (double)diskInfo.UsedSpace / diskInfo.TotalSize * 100
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

            return new DiskSpaceInfo(
                totalSize: driveInfo.TotalSize,
                freeSpace: driveInfo.AvailableFreeSpace,
                usedSpace: driveInfo.TotalSize - driveInfo.AvailableFreeSpace);
        }
        catch (Exception)
        {
            return new DiskSpaceInfo(totalSize: 0, freeSpace: 0, usedSpace: 0);
        }
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
        public long UsedSpace { get; set; } = usedSpace;
    }
}
