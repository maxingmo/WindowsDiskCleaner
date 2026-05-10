using System.Collections.Generic;

namespace WindowsDiskCleaner.Core
{
    public sealed class FolderTreeNode
    {
        private readonly List<FolderTreeNode> _children;

        public FolderTreeNode(string name, string fullPath, FolderTreeNodeType nodeType, FileEntry file)
        {
            Name = name;
            FullPath = fullPath;
            NodeType = nodeType;
            File = file;
            _children = new List<FolderTreeNode>();
        }

        public string Name { get; private set; }

        public string FullPath { get; private set; }

        public FolderTreeNodeType NodeType { get; private set; }

        public long SizeBytes { get; private set; }

        public int FileCount { get; private set; }

        public FileEntry File { get; private set; }

        public List<FolderTreeNode> Children
        {
            get { return _children; }
        }

        public void AddChild(FolderTreeNode child)
        {
            _children.Add(child);
        }

        public void AddToTotals(long sizeBytes, int fileCount)
        {
            SizeBytes += sizeBytes;
            FileCount += fileCount;
        }

        public void SetFileTotals()
        {
            if (File != null)
            {
                SizeBytes = File.SizeBytes;
                FileCount = 1;
            }
        }
    }
}

