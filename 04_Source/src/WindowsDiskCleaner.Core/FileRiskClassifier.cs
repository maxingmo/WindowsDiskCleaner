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
                return new FileRiskAssessment(FileRiskLevel.System, "系统级", "位于 Windows 系统目录，删除风险很高。");
            }

            if (IsProgramInstallPath(path))
            {
                return new FileRiskAssessment(FileRiskLevel.ProgramInstall, "程序安装级", "位于程序安装目录，删除可能导致软件异常。");
            }

            if (IsCacheTemporaryPath(path) || CacheTemporaryExtensions.Contains(extension))
            {
                return new FileRiskAssessment(FileRiskLevel.CacheTemporary, "缓存/临时级", "看起来像缓存、日志或临时文件，仍需确认来源。");
            }

            if (IsUserDataPath(path))
            {
                return new FileRiskAssessment(FileRiskLevel.UserData, "用户数据级", "位于用户常用资料目录，删除前应确认是否仍需要。");
            }

            return new FileRiskAssessment(FileRiskLevel.UnknownCaution, "未知/谨慎级", "未匹配到明确类别，建议谨慎处理。");
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

