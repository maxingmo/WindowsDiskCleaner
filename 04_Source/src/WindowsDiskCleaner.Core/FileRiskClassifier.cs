using System;
using System.Collections.Generic;

namespace WindowsDiskCleaner.Core
{
    public sealed class FileRiskClassifier
    {
        private static readonly HashSet<string> CacheTemporaryExtensions = CreateExtensions(".tmp", ".temp", ".log", ".bak", ".old", ".dmp");

        public FileRiskAssessment Classify(FileEntry file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            var path = NormalizePath(file.FullPath);
            var extension = NormalizeExtension(file.Extension);

            if (IsSystemPath(path))
            {
                return new FileRiskAssessment(
                    FileRiskLevel.System,
                    "\u7cfb\u7edf\u7ea7",
                    "\u4f4d\u4e8e Windows \u7cfb\u7edf\u76ee\u5f55\uff0c\u5220\u9664\u98ce\u9669\u5f88\u9ad8\u3002");
            }

            if (IsProgramInstallPath(path))
            {
                return new FileRiskAssessment(
                    FileRiskLevel.ProgramInstall,
                    "\u7a0b\u5e8f\u5b89\u88c5\u7ea7",
                    "\u4f4d\u4e8e\u7a0b\u5e8f\u5b89\u88c5\u76ee\u5f55\uff0c\u5220\u9664\u53ef\u80fd\u5bfc\u81f4\u8f6f\u4ef6\u5f02\u5e38\u3002");
            }

            if (IsCacheTemporaryPath(path) || CacheTemporaryExtensions.Contains(extension))
            {
                return new FileRiskAssessment(
                    FileRiskLevel.CacheTemporary,
                    "\u7f13\u5b58/\u4e34\u65f6\u7ea7",
                    "\u770b\u8d77\u6765\u50cf\u7f13\u5b58\u3001\u65e5\u5fd7\u6216\u4e34\u65f6\u6587\u4ef6\uff0c\u4ecd\u9700\u786e\u8ba4\u6765\u6e90\u3002");
            }

            if (IsUserDataPath(path))
            {
                return new FileRiskAssessment(
                    FileRiskLevel.UserData,
                    "\u7528\u6237\u6570\u636e\u7ea7",
                    "\u4f4d\u4e8e\u7528\u6237\u5e38\u7528\u8d44\u6599\u76ee\u5f55\uff0c\u5220\u9664\u524d\u5e94\u786e\u8ba4\u662f\u5426\u4ecd\u9700\u8981\u3002");
            }

            return new FileRiskAssessment(
                FileRiskLevel.UnknownCaution,
                "\u672a\u77e5/\u8c28\u614e\u7ea7",
                "\u672a\u5339\u914d\u5230\u660e\u786e\u7c7b\u522b\uff0c\u5efa\u8bae\u8c28\u614e\u5904\u7406\u3002");
        }

        private static bool IsSystemPath(string path)
        {
            return StartsWithPath(path, @"c:\windows\")
                || StartsWithPath(path, @"c:\programdata\microsoft\windows\")
                || StartsWithPath(path, @"c:\system volume information\")
                || StartsWithPath(path, @"c:\$recycle.bin\");
        }

        private static bool IsProgramInstallPath(string path)
        {
            return StartsWithPath(path, @"c:\program files\")
                || StartsWithPath(path, @"c:\program files (x86)\");
        }

        private static bool IsCacheTemporaryPath(string path)
        {
            return ContainsPathPart(path, @"\temp\")
                || ContainsPathPart(path, @"\tmp\")
                || ContainsPathPart(path, @"\cache\")
                || ContainsPathPart(path, @"\logs\")
                || ContainsPathPart(path, @"\log\");
        }

        private static bool IsUserDataPath(string path)
        {
            return ContainsPathPart(path, @"\users\")
                && (ContainsPathPart(path, @"\desktop\")
                    || ContainsPathPart(path, @"\documents\")
                    || ContainsPathPart(path, @"\downloads\")
                    || ContainsPathPart(path, @"\pictures\")
                    || ContainsPathPart(path, @"\videos\")
                    || ContainsPathPart(path, @"\music\"));
        }

        private static bool StartsWithPath(string path, string prefix)
        {
            return path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
        }

        private static bool ContainsPathPart(string path, string part)
        {
            return path.IndexOf(part, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string NormalizePath(string path)
        {
            return (path ?? string.Empty).Replace('/', '\\').Trim().ToLowerInvariant();
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
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var extension in extensions)
            {
                result.Add(NormalizeExtension(extension));
            }

            return result;
        }
    }
}
