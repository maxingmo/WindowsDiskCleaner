using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace WindowsDiskCleaner.Core
{
    public sealed class FileScanner
    {
        public ScanResult Scan(ScanOptions options, CancellationToken cancellationToken, Action<ScanProgress> progress)
        {
            var files = new List<FileEntry>();
            var errors = new List<ScanError>();

            if (!Directory.Exists(options.RootPath))
            {
                errors.Add(ScanError.FromException(
                    options.RootPath,
                    new DirectoryNotFoundException("Scan root path does not exist.")));

                return new ScanResult(files, errors, false);
            }

            WalkDirectory(options.RootPath, files, errors, cancellationToken, progress);

            return new ScanResult(files, errors, cancellationToken.IsCancellationRequested);
        }

        private static void WalkDirectory(
            string directory,
            List<FileEntry> files,
            List<ScanError> errors,
            CancellationToken cancellationToken,
            Action<ScanProgress> progress)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            string[] filePaths;
            if (!TryGetFiles(directory, errors, out filePaths))
            {
                Report(progress, directory, files.Count, errors.Count);
                return;
            }

            foreach (var filePath in filePaths)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                try
                {
                    var info = new FileInfo(filePath);
                    files.Add(new FileEntry(
                        info.Name,
                        info.FullName,
                        info.Length,
                        info.CreationTime,
                        info.LastWriteTime,
                        info.Extension));
                }
                catch (Exception exception)
                {
                    if (IsExpectedIoException(exception))
                    {
                        errors.Add(ScanError.FromException(filePath, exception));
                    }
                    else
                    {
                        throw;
                    }
                }

                Report(progress, filePath, files.Count, errors.Count);
            }

            string[] directories;
            if (!TryGetDirectories(directory, errors, out directories))
            {
                Report(progress, directory, files.Count, errors.Count);
                return;
            }

            foreach (var childDirectory in directories)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                WalkDirectory(childDirectory, files, errors, cancellationToken, progress);
            }
        }

        private static bool TryGetFiles(string directory, List<ScanError> errors, out string[] files)
        {
            try
            {
                files = Directory.GetFiles(directory);
                return true;
            }
            catch (Exception exception)
            {
                if (IsExpectedIoException(exception))
                {
                    files = new string[0];
                    errors.Add(ScanError.FromException(directory, exception));
                    return false;
                }

                throw;
            }
        }

        private static bool TryGetDirectories(string directory, List<ScanError> errors, out string[] directories)
        {
            try
            {
                directories = Directory.GetDirectories(directory);
                return true;
            }
            catch (Exception exception)
            {
                if (IsExpectedIoException(exception))
                {
                    directories = new string[0];
                    errors.Add(ScanError.FromException(directory, exception));
                    return false;
                }

                throw;
            }
        }

        private static void Report(Action<ScanProgress> progress, string currentPath, int fileCount, int skippedCount)
        {
            if (progress != null)
            {
                progress(new ScanProgress(currentPath, fileCount, skippedCount));
            }
        }

        private static bool IsExpectedIoException(Exception exception)
        {
            return exception is UnauthorizedAccessException
                || exception is IOException
                || exception is DirectoryNotFoundException
                || exception is FileNotFoundException
                || exception is PathTooLongException;
        }
    }
}

