using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsDiskCleaner.Core
{
    public sealed class FileFilter
    {
        private static readonly HashSet<string> VideoExtensions = CreateExtensions(".mp4", ".mkv", ".avi", ".mov", ".wmv", ".flv", ".webm");
        private static readonly HashSet<string> ArchiveExtensions = CreateExtensions(".zip", ".rar", ".7z", ".tar", ".gz", ".iso");
        private static readonly HashSet<string> InstallerExtensions = CreateExtensions(".exe", ".msi", ".msix", ".appx");
        private static readonly HashSet<string> LogAndTemporaryExtensions = CreateExtensions(".log", ".tmp", ".temp", ".bak", ".old");

        public IEnumerable<FileEntry> Apply(IEnumerable<FileEntry> files, FileFilterOptions options)
        {
            if (files == null)
            {
                throw new ArgumentNullException("files");
            }

            options = options ?? new FileFilterOptions();

            foreach (var file in files)
            {
                if (!MatchesKeyword(file, options.Keyword))
                {
                    continue;
                }

                if (!MatchesExtension(file, options.Extension))
                {
                    continue;
                }

                if (options.MinimumSizeBytes.HasValue && file.SizeBytes < options.MinimumSizeBytes.Value)
                {
                    continue;
                }

                if (options.ModifiedBefore.HasValue && file.ModifiedAt >= options.ModifiedBefore.Value)
                {
                    continue;
                }

                if (!MatchesQuickFilter(file, options.QuickFilter))
                {
                    continue;
                }

                yield return file;
            }
        }

        private static bool MatchesKeyword(FileEntry file, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return true;
            }

            return Contains(file.Name, keyword) || Contains(file.FullPath, keyword);
        }

        private static bool Contains(string value, string keyword)
        {
            return value != null && value.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool MatchesExtension(FileEntry file, string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                return true;
            }

            return NormalizeExtension(file.Extension) == NormalizeExtension(extension);
        }

        private static bool MatchesQuickFilter(FileEntry file, QuickFileFilter quickFilter)
        {
            switch (quickFilter)
            {
                case QuickFileFilter.All:
                    return true;
                case QuickFileFilter.LargeFiles:
                    return file.SizeBytes >= 100L * 1024L * 1024L;
                case QuickFileFilter.Videos:
                    return VideoExtensions.Contains(NormalizeExtension(file.Extension));
                case QuickFileFilter.Archives:
                    return ArchiveExtensions.Contains(NormalizeExtension(file.Extension));
                case QuickFileFilter.Installers:
                    return InstallerExtensions.Contains(NormalizeExtension(file.Extension));
                case QuickFileFilter.LogsAndTemporary:
                    return LogAndTemporaryExtensions.Contains(NormalizeExtension(file.Extension));
                default:
                    return true;
            }
        }

        private static string NormalizeExtension(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                return string.Empty;
            }

            var normalized = extension.Trim().ToLowerInvariant();
            return normalized.StartsWith(".") ? normalized : "." + normalized;
        }

        private static HashSet<string> CreateExtensions(params string[] extensions)
        {
            return new HashSet<string>(extensions.Select(NormalizeExtension), StringComparer.OrdinalIgnoreCase);
        }
    }
}

