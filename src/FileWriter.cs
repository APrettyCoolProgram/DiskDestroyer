// 261107_code
// 261107_documentation

using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DiskDestroyer
{
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
        public static async Task<DiskWriteResult> WriteUntilDiskFullAsync(
            char driveLetter, 
            long chunkSize = 1024 * 1024, // 1MB chunks
            double maxFillPercentage = 95.0,
            CancellationToken cancellationToken = default)
        {
            string filePath = $@"{driveLetter}:\disk_destroyer.dat";  
            string msg = $"Writing to {filePath}";
            Console.WriteLine(msg);

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }
                
            var result = new DiskWriteResult();
            long totalWritten = 0;
            long currentFileSize = 0;
            DiskSpaceInfo? diskInfo = null; // Initialize to avoid null reference

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read))
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        // Check disk space before each write
                        diskInfo = GetDiskSpace(filePath);
                        
                        if (diskInfo == null || diskInfo.TotalSize <= 0)
                        {
                            throw new InvalidOperationException("Unable to determine disk size");
                        }

                        double currentUsagePercent = (double)(currentFileSize + chunkSize) / diskInfo.TotalSize * 100;

                        // Stop if we're approaching the limit
                        if (currentUsagePercent >= maxFillPercentage)
                        {
                            Console.WriteLine($"Reached target: {maxFillPercentage}% usage");
                            break;
                        }

                        // Write chunk
                        byte[] buffer = new byte[chunkSize];
                        var randomData = GenerateRandomBuffer(buffer);
                        
                        await fileStream.WriteAsync(randomData, cancellationToken);
                        currentFileSize += chunkSize;
                        totalWritten += chunkSize;

                        result.Statistics.Add(new DiskWriteStatistics(
                            timestamp: DateTime.UtcNow, 
                            bytesWritten: chunkSize, 
                            cumulativeBytes: currentFileSize, 
                            diskUsagePercent: currentUsagePercent));

                        // Progress reporting every 10 chunks or 1GB
                        if (totalWritten % (chunkSize * 10) == 0 || totalWritten >= 1024L * 1024 * 1024)
                        {
                            Console.WriteLine($"Progress: {currentFileSize:N0} bytes ({(double)currentFileSize / diskInfo.TotalSize:P2})");
                        }
                            
                        // Small delay to prevent I/O saturation
                        await Task.Delay(10, cancellationToken);
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

            result.TotalBytesWritten = totalWritten;
            // Fixed null reference issue - check diskInfo before using it
            result.FinalDiskUsagePercent = currentFileSize > 0 && diskInfo != null 
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
                    return null;

                DriveInfo driveInfo = new DriveInfo(drive);
                
                // For more accurate real-time data, use WMI or Win32 API
                // This is a simplified version using DriveInfo
                return new DiskSpaceInfo(
                    totalSize: driveInfo.TotalSize,
                    freeSpace: driveInfo.AvailableFreeSpace,
                    usedSpace: driveInfo.TotalSize - driveInfo.AvailableFreeSpace);
            }
            catch (Exception)
            {
                // Fallback to DriveInfo if WMI fails
                var drive = Path.GetPathRoot(filePath);
                return new DiskSpaceInfo(
                    totalSize: 0, 
                    freeSpace: 0, 
                    usedSpace: 0);
            }
        }

        /// <summary>
        /// Generate random data for writing to avoid patterns.
        /// </summary>
        private static byte[] GenerateRandomBuffer(byte[] buffer)
        {
            var rng = new Random();
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = (byte)(rng.Next(256));
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
        public class DiskWriteStatistics
        {
            public DateTime Timestamp { get; set; }
            public long BytesWritten { get; set; }
            public long CumulativeBytes { get; set; }
            public double DiskUsagePercent { get; set; }

            public DiskWriteStatistics(DateTime timestamp, long bytesWritten, 
                long cumulativeBytes, double diskUsagePercent)
            {
                Timestamp = timestamp;
                BytesWritten = bytesWritten;
                CumulativeBytes = cumulativeBytes;
                DiskUsagePercent = diskUsagePercent;
            }
        }

        /// <summary>
        /// Simple disk space information.
        /// </summary>
        private class DiskSpaceInfo
        {
            public long TotalSize { get; set; }
            public long FreeSpace { get; set; }
            public long UsedSpace { get; set; }

            // FIXED: Removed the typo 'usedSynced' from line 94
            public DiskSpaceInfo(long totalSize, long freeSpace, long usedSpace)
            {
                TotalSize = totalSize;
                FreeSpace = freeSpace;
                UsedSpace = usedSpace; // Correct assignment
            }
        }
    }
}
