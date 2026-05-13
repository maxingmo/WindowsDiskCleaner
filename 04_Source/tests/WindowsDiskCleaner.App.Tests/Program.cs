using System;
using System.Collections.Generic;
using WindowsDiskCleaner.App;
using WindowsDiskCleaner.Core;

namespace WindowsDiskCleaner.App.Tests
{
    internal static class Program
    {
        private static int Main()
        {
            var tests = new Action[]
            {
                FolderDisplayTextDoesNotUseTechnicalPrefix,
                FileDisplayTextDoesNotUseTechnicalPrefix
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

            Console.WriteLine("All app tests passed.");
            return 0;
        }

        private static void FolderDisplayTextDoesNotUseTechnicalPrefix()
        {
            var file = new FileEntry("report.txt", @"D:\Root\report.txt", 1024, DateTime.Now, DateTime.Now, ".txt");
            var node = new FolderTreeBuilder().Build(new[] { file }).SingleForTest();
            var viewModel = FolderTreeNodeViewModel.Create(node, new Dictionary<string, FileEntryViewModel>());

            AssertTrue(!viewModel.DisplayText.StartsWith("[DIR]", StringComparison.Ordinal), "folder prefix should be hidden");
            AssertTrue(viewModel.DisplayText.StartsWith("Root", StringComparison.Ordinal), "folder name should lead display text");
        }

        private static void FileDisplayTextDoesNotUseTechnicalPrefix()
        {
            var file = new FileEntry("report.txt", @"D:\Root\report.txt", 1024, DateTime.Now, DateTime.Now, ".txt");
            var node = new FolderTreeBuilder().Build(new[] { file }).SingleForTest().Children[0];
            var fileViewModel = new FileEntryViewModel(file);
            var map = new Dictionary<string, FileEntryViewModel>(StringComparer.OrdinalIgnoreCase)
            {
                { file.FullPath, fileViewModel }
            };
            var viewModel = FolderTreeNodeViewModel.Create(node, map);

            AssertTrue(!viewModel.DisplayText.StartsWith("[FILE]", StringComparison.Ordinal), "file prefix should be hidden");
            AssertTrue(viewModel.DisplayText.StartsWith("report.txt", StringComparison.Ordinal), "file name should lead display text");
        }

        private static T SingleForTest<T>(this IEnumerable<T> values)
        {
            using (var enumerator = values.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    throw new InvalidOperationException("Expected one item but got none.");
                }

                var item = enumerator.Current;
                if (enumerator.MoveNext())
                {
                    throw new InvalidOperationException("Expected one item but got more.");
                }

                return item;
            }
        }

        private static void AssertTrue(bool condition, string message)
        {
            if (!condition)
            {
                throw new InvalidOperationException(message);
            }
        }
    }
}
