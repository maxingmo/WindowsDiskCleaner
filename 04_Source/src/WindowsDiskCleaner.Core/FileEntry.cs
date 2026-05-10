using System;

namespace WindowsDiskCleaner.Core
{
    public sealed class FileEntry
    {
        public FileEntry(string name, string fullPath, long sizeBytes, DateTime createdAt, DateTime modifiedAt, string extension)
        {
            Name = name;
            FullPath = fullPath;
            SizeBytes = sizeBytes;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
            Extension = extension;
        }

        public string Name { get; private set; }

        public string FullPath { get; private set; }

        public long SizeBytes { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime ModifiedAt { get; private set; }

        public string Extension { get; private set; }
    }
}

