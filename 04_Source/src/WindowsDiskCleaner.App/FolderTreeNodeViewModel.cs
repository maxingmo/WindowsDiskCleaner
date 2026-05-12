using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WindowsDiskCleaner.Core;

namespace WindowsDiskCleaner.App
{
    public sealed class FolderTreeNodeViewModel
    {
        private readonly FolderTreeNode _node;
        private readonly FileEntryViewModel _fileEntry;

        private FolderTreeNodeViewModel(
            FolderTreeNode node,
            FileEntryViewModel fileEntry,
            IEnumerable<FolderTreeNodeViewModel> children)
        {
            _node = node;
            _fileEntry = fileEntry;
            Children = new ObservableCollection<FolderTreeNodeViewModel>(children);
        }

        public ObservableCollection<FolderTreeNodeViewModel> Children { get; private set; }

        public FileEntryViewModel FileEntry
        {
            get { return _fileEntry; }
        }

        public bool IsFile
        {
            get { return _node.NodeType == FolderTreeNodeType.File; }
        }

        public string FullPath
        {
            get { return _node.FullPath; }
        }

        public string DisplayText
        {
            get
            {
                if (IsFile)
                {
                    return "[FILE] " + _node.Name
                        + " | " + FormatSize(_node.SizeBytes)
                        + " | " + RiskDisplayText
                        + " | " + (_node.File == null ? string.Empty : _node.File.ModifiedAt.ToString("yyyy-MM-dd HH:mm"));
                }

                return "[DIR] " + _node.Name
                    + " | " + _node.FullPath
                    + " | " + _node.FileCount + " \u4e2a\u6587\u4ef6"
                    + " | " + FormatSize(_node.SizeBytes);
            }
        }

        public string RiskDisplayText
        {
            get
            {
                return _fileEntry == null
                    ? string.Empty
                    : _fileEntry.RiskLevelDisplay + " / " + _fileEntry.RiskBadgeText;
            }
        }

        public string RiskForegroundHex
        {
            get { return _fileEntry == null ? "#374151" : _fileEntry.RiskForegroundHex; }
        }

        public string RiskBackgroundHex
        {
            get { return _fileEntry == null ? "#ffffff" : _fileEntry.RiskBackgroundHex; }
        }

        public static FolderTreeNodeViewModel Create(
            FolderTreeNode node,
            IDictionary<string, FileEntryViewModel> fileEntriesByPath)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            FileEntryViewModel fileEntry = null;
            if (node.NodeType == FolderTreeNodeType.File && node.File != null)
            {
                fileEntriesByPath.TryGetValue(node.File.FullPath, out fileEntry);
            }

            var children = new List<FolderTreeNodeViewModel>();
            foreach (var child in node.Children)
            {
                children.Add(Create(child, fileEntriesByPath));
            }

            return new FolderTreeNodeViewModel(node, fileEntry, children);
        }

        private static string FormatSize(long bytes)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB" };
            var size = (double)bytes;
            var unit = 0;

            while (size >= 1024 && unit < units.Length - 1)
            {
                size = size / 1024;
                unit++;
            }

            return unit == 0
                ? bytes + " " + units[unit]
                : size.ToString("0.##") + " " + units[unit];
        }
    }
}
