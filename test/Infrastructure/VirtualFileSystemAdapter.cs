using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Vertical.Tools.TemplateCopy;

namespace Infrastructure
{
    public class VirtualFileSystemAdapter : IFileSystemAdapter
    {
        private readonly IFileSystem _fileSystem = new MockFileSystem();

        /// <inheritdoc />
        public string ResolvePath(string path) => path;

        /// <inheritdoc />
        public string GetFileName(string path) => _fileSystem.Path.GetFileName(path);

        /// <inheritdoc />
        public string GetFileExtension(string path) => _fileSystem.Path.GetExtension(path);

        /// <inheritdoc />
        public string CombinePaths(string path1, string path2) => _fileSystem.Path.Combine(path1, path2);

        /// <inheritdoc />
        public void CreateDirectory(string path) => _fileSystem.Directory.CreateDirectory(path);

        /// <inheritdoc />
        public IEnumerable<string> GetFiles(string path) => _fileSystem.Directory.GetFiles(path);

        /// <inheritdoc />
        public IEnumerable<string> GetDirectories(string path) => _fileSystem.Directory.GetDirectories(path);

        /// <inheritdoc />
        public string ReadFile(string path) => _fileSystem.File.ReadAllText(path);

        /// <inheritdoc />
        public void WriteFile(string path, string content) => _fileSystem.File.WriteAllText(path, content);

        /// <inheritdoc />
        public void CopyFile(string sourcePath, string targetPath) => _fileSystem.File.Copy(sourcePath, targetPath);

        /// <inheritdoc />
        public void Validate(string path)
        {
            if (_fileSystem.File.Exists(path) || _fileSystem.Directory.Exists(path))
                return;
            
            throw new FileNotFoundException(path);
        }
    }
}