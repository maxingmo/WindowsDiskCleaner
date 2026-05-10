using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WindowsDiskCleaner.Core
{
    public sealed class FolderTreeBuilder
    {
        public IReadOnlyList<FolderTreeNode> Build(IEnumerable<FileEntry> files)
        {
            if (files == null)
            {
                throw new ArgumentNullException("files");
            }

            var roots = new List<FolderTreeNode>();
            var foldersByPath = new Dictionary<string, FolderTreeNode>(StringComparer.OrdinalIgnoreCase);

            foreach (var file in files)
            {
                if (file == null || string.IsNullOrWhiteSpace(file.FullPath))
                {
                    continue;
                }

                var directory = Path.GetDirectoryName(file.FullPath);
                if (string.IsNullOrWhiteSpace(directory))
                {
                    directory = "(No folder)";
                }

                var folder = EnsureFolder(directory, roots, foldersByPath);
                var fileNode = new FolderTreeNode(file.Name, file.FullPath, FolderTreeNodeType.File, file);
                fileNode.SetFileTotals();
                folder.AddChild(fileNode);

                AddTotalsToAncestors(folder.FullPath, file.SizeBytes, foldersByPath);
            }

            SortRecursively(roots);
            return roots;
        }

        private static FolderTreeNode EnsureFolder(
            string directory,
            List<FolderTreeNode> roots,
            Dictionary<string, FolderTreeNode> foldersByPath)
        {
            FolderTreeNode existing;
            if (foldersByPath.TryGetValue(directory, out existing))
            {
                return existing;
            }

            var parentPath = Path.GetDirectoryName(directory);
            var name = Path.GetFileName(directory);
            if (string.IsNullOrWhiteSpace(name))
            {
                name = directory;
            }

            var node = new FolderTreeNode(name, directory, FolderTreeNodeType.Folder, null);
            foldersByPath[directory] = node;

            if (string.IsNullOrWhiteSpace(parentPath) || IsPathRoot(parentPath))
            {
                roots.Add(node);
            }
            else
            {
                var parent = EnsureFolder(parentPath, roots, foldersByPath);
                parent.AddChild(node);
            }

            return node;
        }

        private static bool IsPathRoot(string path)
        {
            var root = Path.GetPathRoot(path);
            return !string.IsNullOrWhiteSpace(root)
                && string.Equals(
                    root.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
                    path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
                    StringComparison.OrdinalIgnoreCase);
        }

        private static void AddTotalsToAncestors(
            string folderPath,
            long sizeBytes,
            Dictionary<string, FolderTreeNode> foldersByPath)
        {
            var current = folderPath;
            while (!string.IsNullOrWhiteSpace(current))
            {
                FolderTreeNode node;
                if (foldersByPath.TryGetValue(current, out node))
                {
                    node.AddToTotals(sizeBytes, 1);
                }

                var parent = Path.GetDirectoryName(current);
                if (string.Equals(parent, current, StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                current = parent;
            }
        }

        private static void SortRecursively(List<FolderTreeNode> nodes)
        {
            nodes.Sort(CompareNodes);

            foreach (var node in nodes)
            {
                SortRecursively(node.Children);
            }
        }

        private static int CompareNodes(FolderTreeNode left, FolderTreeNode right)
        {
            if (left.NodeType != right.NodeType)
            {
                return left.NodeType == FolderTreeNodeType.Folder ? -1 : 1;
            }

            return string.Compare(left.Name, right.Name, StringComparison.OrdinalIgnoreCase);
        }
    }
}
