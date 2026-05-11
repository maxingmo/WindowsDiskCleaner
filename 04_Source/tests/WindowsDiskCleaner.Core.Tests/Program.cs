using System;
using System.IO;
using System.Linq;
using System.Threading;
using WindowsDiskCleaner.Core;

namespace WindowsDiskCleaner.Core.Tests
{
    internal static class Program
    {
        private static int Main()
        {
            var tests = new Action[]
            {
                ScanIncludesRootAndNestedFiles,
                ScanMapsFileMetadata,
                MissingRootIsReportedAsSkippedError,
                CancellationStopsScan,
                FilterMatchesKeywordInNameOrPath,
                FilterNormalizesExtension,
                FilterAppliesMinimumSize,
                FilterAppliesModifiedBefore,
                FilterAppliesQuickTypeAndCombinedRules,
                DebouncedActionDoesNotRunBeforeTimerElapsed,
                DebouncedActionRunNowStopsTimerAndRunsImmediately,
                RiskClassifierMarksWindowsPathAsSystem,
                RiskClassifierMarksProgramFilesAsProgramInstall,
                RiskClassifierMarksUserKnownFoldersAsUserData,
                RiskClassifierMarksCacheAndLogsAsTemporary,
                RiskClassifierMarksUnknownPathsAsCaution,
                DeleteServiceRejectsUnconfirmedNormalFile,
                DeleteServiceDeletesConfirmedNormalFile,
                DeleteServiceRequiresHighRiskConfirmation,
                DeleteServiceDeletesHighRiskFileAfterSecondConfirmation,
                DeleteServiceReturnsFailureWhenAdapterThrows,
                BatchDeleteRejectsEmptySelection,
                BatchDeleteRejectsUnconfirmedSelection,
                BatchDeleteRequiresHighRiskConfirmationBeforeAnyDelete,
                BatchDeleteDeletesMixedFilesAfterHighRiskConfirmation,
                BatchDeleteReportsPartialFailures,
                FolderTreeBuilderGroupsFilesByFolder,
                FolderTreeBuilderRollsUpNestedFolderTotals,
                FolderTreeBuilderSortsFoldersBeforeFiles
            };

            var failed = 0;

            foreach (var test in tests)
            {
                try
                {
                    test();
                    Console.WriteLine("PASS " + test.Method.Name);
                }
                catch (Exception exception)
                {
                    failed++;
                    Console.WriteLine("FAIL " + test.Method.Name);
                    Console.WriteLine(exception);
                }
            }

            if (failed > 0)
            {
                Console.WriteLine(failed + " test(s) failed.");
                return 1;
            }

            Console.WriteLine("All tests passed.");
            return 0;
        }

        private static void ScanIncludesRootAndNestedFiles()
        {
            WithTempDirectory(root =>
            {
                File.WriteAllText(Path.Combine(root, "root.txt"), "root");
                var nested = Directory.CreateDirectory(Path.Combine(root, "nested")).FullName;
                File.WriteAllText(Path.Combine(nested, "child.log"), "child");

                var scanner = new FileScanner();
                var result = scanner.Scan(new ScanOptions(root), CancellationToken.None, null);

                AssertEqual(2, result.Files.Count, "file count");
                AssertTrue(result.Files.Any(file => file.Name == "root.txt"), "root file missing");
                AssertTrue(result.Files.Any(file => file.Name == "child.log"), "nested file missing");
                AssertEqual(0, result.SkippedCount, "skipped count");
            });
        }

        private static void ScanMapsFileMetadata()
        {
            WithTempDirectory(root =>
            {
                var path = Path.Combine(root, "sample.data");
                File.WriteAllText(path, "1234567890");

                var scanner = new FileScanner();
                var result = scanner.Scan(new ScanOptions(root), CancellationToken.None, null);
                var entry = result.Files.Single();

                AssertEqual("sample.data", entry.Name, "name");
                AssertEqual(path, entry.FullPath, "full path");
                AssertEqual(10L, entry.SizeBytes, "size");
                AssertEqual(".data", entry.Extension, "extension");
                AssertTrue(entry.CreatedAt > DateTime.MinValue, "created time missing");
                AssertTrue(entry.ModifiedAt > DateTime.MinValue, "modified time missing");
            });
        }

        private static void MissingRootIsReportedAsSkippedError()
        {
            var root = Path.Combine(Path.GetTempPath(), "wdc-missing-" + Guid.NewGuid().ToString("N"));
            var scanner = new FileScanner();

            var result = scanner.Scan(new ScanOptions(root), CancellationToken.None, null);

            AssertEqual(0, result.Files.Count, "file count");
            AssertEqual(1, result.SkippedCount, "skipped count");
            AssertTrue(result.Errors[0].ErrorType == "DirectoryNotFoundException", "wrong error type");
        }

        private static void CancellationStopsScan()
        {
            WithTempDirectory(root =>
            {
                File.WriteAllText(Path.Combine(root, "first.txt"), "first");
                File.WriteAllText(Path.Combine(root, "second.txt"), "second");

                var source = new CancellationTokenSource();
                var scanner = new FileScanner();

                var result = scanner.Scan(
                    new ScanOptions(root),
                    source.Token,
                    progress => source.Cancel());

                AssertTrue(result.WasCancelled, "scan was not marked cancelled");
            });
        }

        private static void FilterMatchesKeywordInNameOrPath()
        {
            var files = SampleFiles();
            var filter = new FileFilter();

            var result = filter.Apply(files, new FileFilterOptions { Keyword = "movie" }).ToList();

            AssertEqual(1, result.Count, "keyword count");
            AssertEqual("movie.mp4", result[0].Name, "keyword result");
        }

        private static void FilterNormalizesExtension()
        {
            var files = SampleFiles();
            var filter = new FileFilter();

            var result = filter.Apply(files, new FileFilterOptions { Extension = "zip" }).ToList();

            AssertEqual(1, result.Count, "extension count");
            AssertEqual("archive.zip", result[0].Name, "extension result");
        }

        private static void FilterAppliesMinimumSize()
        {
            var files = SampleFiles();
            var filter = new FileFilter();

            var result = filter.Apply(files, new FileFilterOptions { MinimumSizeBytes = 100L * 1024L * 1024L }).ToList();

            AssertEqual(2, result.Count, "minimum size count");
            AssertTrue(result.All(file => file.SizeBytes >= 100L * 1024L * 1024L), "small file included");
        }

        private static void FilterAppliesModifiedBefore()
        {
            var files = SampleFiles();
            var filter = new FileFilter();

            var result = filter.Apply(files, new FileFilterOptions { ModifiedBefore = new DateTime(2025, 1, 1) }).ToList();

            AssertEqual(2, result.Count, "modified-before count");
            AssertTrue(result.All(file => file.ModifiedAt < new DateTime(2025, 1, 1)), "new file included");
        }

        private static void FilterAppliesQuickTypeAndCombinedRules()
        {
            var files = SampleFiles();
            var filter = new FileFilter();

            var result = filter.Apply(files, new FileFilterOptions
            {
                Keyword = "setup",
                QuickFilter = QuickFileFilter.Installers
            }).ToList();

            AssertEqual(1, result.Count, "combined quick count");
            AssertEqual("setup.exe", result[0].Name, "combined quick result");
        }

        private static FileEntry[] SampleFiles()
        {
            return new[]
            {
                new FileEntry("movie.mp4", @"D:\Media\movie.mp4", 700L * 1024L * 1024L, new DateTime(2024, 1, 1), new DateTime(2024, 6, 1), ".mp4"),
                new FileEntry("archive.zip", @"D:\Downloads\archive.zip", 120L * 1024L * 1024L, new DateTime(2023, 1, 1), new DateTime(2023, 5, 1), ".zip"),
                new FileEntry("setup.exe", @"D:\Downloads\setup.exe", 80L * 1024L * 1024L, new DateTime(2026, 1, 1), new DateTime(2026, 2, 1), ".exe"),
                new FileEntry("trace.log", @"D:\Logs\trace.log", 2L * 1024L * 1024L, new DateTime(2026, 1, 1), new DateTime(2026, 2, 1), ".log"),
                new FileEntry("notes.txt", @"D:\Docs\notes.txt", 10L * 1024L, new DateTime(2026, 1, 1), new DateTime(2026, 2, 1), ".txt")
            };
        }

        private static void DebouncedActionDoesNotRunBeforeTimerElapsed()
        {
            var timer = new FakeDebounceTimer();
            var runCount = 0;
            var action = new DebouncedAction(timer, () => runCount++);

            action.Schedule();
            action.Schedule();

            AssertEqual(0, runCount, "run count before elapsed");
            AssertEqual(2, timer.RestartCount, "restart count");

            timer.Fire();

            AssertEqual(1, runCount, "run count after elapsed");
        }

        private static void DebouncedActionRunNowStopsTimerAndRunsImmediately()
        {
            var timer = new FakeDebounceTimer();
            var runCount = 0;
            var action = new DebouncedAction(timer, () => runCount++);

            action.Schedule();
            action.RunNow();
            timer.Fire();

            AssertEqual(1, runCount, "run count");
            AssertEqual(1, timer.StopCount, "stop count");
        }

        private static void RiskClassifierMarksWindowsPathAsSystem()
        {
            var classifier = new FileRiskClassifier();
            var entry = new FileEntry("kernel.dll", @"C:\Windows\System32\kernel.dll", 10, DateTime.Now, DateTime.Now, ".dll");

            var result = classifier.Classify(entry);

            AssertEqual(FileRiskLevel.System, result.Level, "risk level");
            AssertEqual("系统级", result.DisplayName, "display name");
        }

        private static void RiskClassifierMarksProgramFilesAsProgramInstall()
        {
            var classifier = new FileRiskClassifier();
            var entry = new FileEntry("app.dll", @"C:\Program Files\Vendor\App\app.dll", 10, DateTime.Now, DateTime.Now, ".dll");

            var result = classifier.Classify(entry);

            AssertEqual(FileRiskLevel.ProgramInstall, result.Level, "risk level");
        }

        private static void RiskClassifierMarksUserKnownFoldersAsUserData()
        {
            var classifier = new FileRiskClassifier();
            var entry = new FileEntry("photo.jpg", @"C:\Users\Alice\Pictures\photo.jpg", 10, DateTime.Now, DateTime.Now, ".jpg");

            var result = classifier.Classify(entry);

            AssertEqual(FileRiskLevel.UserData, result.Level, "risk level");
        }

        private static void RiskClassifierMarksCacheAndLogsAsTemporary()
        {
            var classifier = new FileRiskClassifier();
            var entry = new FileEntry("trace.log", @"C:\Users\Alice\AppData\Local\Vendor\Cache\trace.log", 10, DateTime.Now, DateTime.Now, ".log");

            var result = classifier.Classify(entry);

            AssertEqual(FileRiskLevel.CacheTemporary, result.Level, "risk level");
        }

        private static void RiskClassifierMarksUnknownPathsAsCaution()
        {
            var classifier = new FileRiskClassifier();
            var entry = new FileEntry("data.bin", @"D:\Unsorted\data.bin", 10, DateTime.Now, DateTime.Now, ".bin");

            var result = classifier.Classify(entry);

            AssertEqual(FileRiskLevel.UnknownCaution, result.Level, "risk level");
        }

        private static void DeleteServiceRejectsUnconfirmedNormalFile()
        {
            var adapter = new FakeDeleteAdapter();
            var service = new FileDeletionService(adapter);
            var entry = new FileEntry("photo.jpg", @"C:\Users\Alice\Pictures\photo.jpg", 10, DateTime.Now, DateTime.Now, ".jpg");

            var result = service.DeleteToRecycleBin(entry, FileDeleteConfirmation.NotConfirmed);

            AssertTrue(!result.Succeeded, "delete should fail");
            AssertEqual(0, adapter.CallCount, "adapter calls");
        }

        private static void DeleteServiceDeletesConfirmedNormalFile()
        {
            var adapter = new FakeDeleteAdapter();
            var service = new FileDeletionService(adapter);
            var entry = new FileEntry("photo.jpg", @"C:\Users\Alice\Pictures\photo.jpg", 10, DateTime.Now, DateTime.Now, ".jpg");

            var result = service.DeleteToRecycleBin(entry, FileDeleteConfirmation.Confirmed);

            AssertTrue(result.Succeeded, "delete should succeed");
            AssertEqual(1, adapter.CallCount, "adapter calls");
            AssertEqual(entry.FullPath, adapter.LastPath, "deleted path");
        }

        private static void DeleteServiceRequiresHighRiskConfirmation()
        {
            var adapter = new FakeDeleteAdapter();
            var service = new FileDeletionService(adapter);
            var entry = new FileEntry("kernel.dll", @"C:\Windows\System32\kernel.dll", 10, DateTime.Now, DateTime.Now, ".dll");

            var result = service.DeleteToRecycleBin(entry, FileDeleteConfirmation.Confirmed);

            AssertTrue(!result.Succeeded, "delete should fail");
            AssertEqual(0, adapter.CallCount, "adapter calls");
        }

        private static void DeleteServiceDeletesHighRiskFileAfterSecondConfirmation()
        {
            var adapter = new FakeDeleteAdapter();
            var service = new FileDeletionService(adapter);
            var entry = new FileEntry("kernel.dll", @"C:\Windows\System32\kernel.dll", 10, DateTime.Now, DateTime.Now, ".dll");

            var result = service.DeleteToRecycleBin(entry, FileDeleteConfirmation.HighRiskConfirmed);

            AssertTrue(result.Succeeded, "delete should succeed");
            AssertEqual(1, adapter.CallCount, "adapter calls");
        }

        private static void DeleteServiceReturnsFailureWhenAdapterThrows()
        {
            var adapter = new FakeDeleteAdapter { ThrowOnDelete = true };
            var service = new FileDeletionService(adapter);
            var entry = new FileEntry("photo.jpg", @"C:\Users\Alice\Pictures\photo.jpg", 10, DateTime.Now, DateTime.Now, ".jpg");

            var result = service.DeleteToRecycleBin(entry, FileDeleteConfirmation.Confirmed);

            AssertTrue(!result.Succeeded, "delete should fail");
            AssertTrue(result.Message.IndexOf("boom", StringComparison.OrdinalIgnoreCase) >= 0, "failure message missing adapter error");
        }

        private static void BatchDeleteRejectsEmptySelection()
        {
            var adapter = new FakeDeleteAdapter();
            var service = new FileBatchDeletionService(adapter);

            var result = service.DeleteToRecycleBin(new FileEntry[0], FileDeleteConfirmation.Confirmed);

            AssertTrue(!result.Succeeded, "batch delete should fail");
            AssertEqual(0, adapter.CallCount, "adapter calls");
            AssertEqual(0, result.DeletedCount, "deleted count");
        }

        private static void BatchDeleteRejectsUnconfirmedSelection()
        {
            var adapter = new FakeDeleteAdapter();
            var service = new FileBatchDeletionService(adapter);
            var files = new[]
            {
                new FileEntry("photo.jpg", @"C:\Users\Alice\Pictures\photo.jpg", 10, DateTime.Now, DateTime.Now, ".jpg"),
                new FileEntry("notes.txt", @"C:\Users\Alice\Documents\notes.txt", 10, DateTime.Now, DateTime.Now, ".txt")
            };

            var result = service.DeleteToRecycleBin(files, FileDeleteConfirmation.NotConfirmed);

            AssertTrue(!result.Succeeded, "batch delete should fail");
            AssertEqual(0, adapter.CallCount, "adapter calls");
            AssertEqual(0, result.DeletedCount, "deleted count");
        }

        private static void BatchDeleteRequiresHighRiskConfirmationBeforeAnyDelete()
        {
            var adapter = new FakeDeleteAdapter();
            var service = new FileBatchDeletionService(adapter);
            var files = new[]
            {
                new FileEntry("photo.jpg", @"C:\Users\Alice\Pictures\photo.jpg", 10, DateTime.Now, DateTime.Now, ".jpg"),
                new FileEntry("kernel.dll", @"C:\Windows\System32\kernel.dll", 10, DateTime.Now, DateTime.Now, ".dll")
            };

            var result = service.DeleteToRecycleBin(files, FileDeleteConfirmation.Confirmed);

            AssertTrue(!result.Succeeded, "batch delete should fail");
            AssertEqual(0, adapter.CallCount, "adapter calls");
            AssertEqual(0, result.DeletedCount, "deleted count");
        }

        private static void BatchDeleteDeletesMixedFilesAfterHighRiskConfirmation()
        {
            var adapter = new FakeDeleteAdapter();
            var service = new FileBatchDeletionService(adapter);
            var files = new[]
            {
                new FileEntry("photo.jpg", @"C:\Users\Alice\Pictures\photo.jpg", 10, DateTime.Now, DateTime.Now, ".jpg"),
                new FileEntry("kernel.dll", @"C:\Windows\System32\kernel.dll", 10, DateTime.Now, DateTime.Now, ".dll")
            };

            var result = service.DeleteToRecycleBin(files, FileDeleteConfirmation.HighRiskConfirmed);

            AssertTrue(result.Succeeded, "batch delete should succeed");
            AssertEqual(2, adapter.CallCount, "adapter calls");
            AssertEqual(2, result.DeletedCount, "deleted count");
            AssertEqual(0, result.FailedCount, "failed count");
        }

        private static void BatchDeleteReportsPartialFailures()
        {
            var adapter = new FakeDeleteAdapter { PathToFail = @"C:\Users\Alice\Documents\bad.txt" };
            var service = new FileBatchDeletionService(adapter);
            var files = new[]
            {
                new FileEntry("good.txt", @"C:\Users\Alice\Documents\good.txt", 10, DateTime.Now, DateTime.Now, ".txt"),
                new FileEntry("bad.txt", @"C:\Users\Alice\Documents\bad.txt", 10, DateTime.Now, DateTime.Now, ".txt")
            };

            var result = service.DeleteToRecycleBin(files, FileDeleteConfirmation.Confirmed);

            AssertTrue(!result.Succeeded, "batch delete should report partial failure");
            AssertEqual(2, adapter.CallCount, "adapter calls");
            AssertEqual(1, result.DeletedCount, "deleted count");
            AssertEqual(1, result.FailedCount, "failed count");
            AssertEqual(1, result.DeletedPaths.Count, "deleted path count");
            AssertEqual(@"C:\Users\Alice\Documents\good.txt", result.DeletedPaths[0], "deleted path");
        }

        private static void FolderTreeBuilderGroupsFilesByFolder()
        {
            var builder = new FolderTreeBuilder();
            var files = new[]
            {
                new FileEntry("a.txt", @"D:\Root\a.txt", 10, DateTime.Now, DateTime.Now, ".txt"),
                new FileEntry("b.log", @"D:\Root\b.log", 20, DateTime.Now, DateTime.Now, ".log")
            };

            var roots = builder.Build(files).ToList();

            AssertEqual(1, roots.Count, "root count");
            AssertEqual(FolderTreeNodeType.Folder, roots[0].NodeType, "root type");
            AssertEqual(@"D:\Root", roots[0].FullPath, "root path");
            AssertEqual(2, roots[0].FileCount, "root file count");
            AssertEqual(30L, roots[0].SizeBytes, "root size");
            AssertEqual(2, roots[0].Children.Count, "children count");
        }

        private static void FolderTreeBuilderRollsUpNestedFolderTotals()
        {
            var builder = new FolderTreeBuilder();
            var files = new[]
            {
                new FileEntry("a.txt", @"D:\Root\a.txt", 10, DateTime.Now, DateTime.Now, ".txt"),
                new FileEntry("c.mp4", @"D:\Root\Media\c.mp4", 100, DateTime.Now, DateTime.Now, ".mp4")
            };

            var root = builder.Build(files).Single();
            var media = root.Children.Single(child => child.NodeType == FolderTreeNodeType.Folder);

            AssertEqual(2, root.FileCount, "root file count");
            AssertEqual(110L, root.SizeBytes, "root size");
            AssertEqual(1, media.FileCount, "media file count");
            AssertEqual(100L, media.SizeBytes, "media size");
        }

        private static void FolderTreeBuilderSortsFoldersBeforeFiles()
        {
            var builder = new FolderTreeBuilder();
            var files = new[]
            {
                new FileEntry("z.txt", @"D:\Root\z.txt", 10, DateTime.Now, DateTime.Now, ".txt"),
                new FileEntry("a.txt", @"D:\Root\Folder\a.txt", 20, DateTime.Now, DateTime.Now, ".txt")
            };

            var root = builder.Build(files).Single();

            AssertEqual(FolderTreeNodeType.Folder, root.Children[0].NodeType, "first child type");
            AssertEqual("Folder", root.Children[0].Name, "first child name");
            AssertEqual(FolderTreeNodeType.File, root.Children[1].NodeType, "second child type");
        }

        private static void WithTempDirectory(Action<string> action)
        {
            var root = Path.Combine(Path.GetTempPath(), "wdc-tests-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(root);

            try
            {
                action(root);
            }
            finally
            {
                if (Directory.Exists(root))
                {
                    Directory.Delete(root, true);
                }
            }
        }

        private static void AssertEqual<T>(T expected, T actual, string label)
        {
            if (!object.Equals(expected, actual))
            {
                throw new InvalidOperationException(label + ": expected <" + expected + "> but got <" + actual + ">.");
            }
        }

        private static void AssertTrue(bool condition, string message)
        {
            if (!condition)
            {
                throw new InvalidOperationException(message);
            }
        }

        private sealed class FakeDebounceTimer : IDebounceTimer
        {
            private bool _isRunning;

            public event EventHandler Elapsed;

            public int RestartCount { get; private set; }

            public int StopCount { get; private set; }

            public void Restart()
            {
                RestartCount++;
                _isRunning = true;
            }

            public void Stop()
            {
                StopCount++;
                _isRunning = false;
            }

            public void Fire()
            {
                if (!_isRunning)
                {
                    return;
                }

                _isRunning = false;
                var handler = Elapsed;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }

        private sealed class FakeDeleteAdapter : IFileDeleteAdapter
        {
            public int CallCount { get; private set; }

            public string LastPath { get; private set; }

            public bool ThrowOnDelete { get; set; }

            public string PathToFail { get; set; }

            public void MoveToRecycleBin(string path)
            {
                CallCount++;
                LastPath = path;

                if (ThrowOnDelete || string.Equals(PathToFail, path, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("boom");
                }
            }
        }
    }
}
