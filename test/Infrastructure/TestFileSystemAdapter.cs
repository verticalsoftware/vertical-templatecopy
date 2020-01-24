using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using Vertical.Tools.TemplateCopy;

namespace Infrastructure
{
    public class TestFileSystemAdapter : IFileSystemAdapter
    {
        private readonly IFileSystem _fileSystem = new MockFileSystem();
        
        public TestFileSystemAdapter()
        {
            using var zipArchive = new ZipArchive(new FileStream("Resources/template.zip", FileMode.Open));

            foreach (var entry in zipArchive.Entries)
            {
                if (entry.Length > 0)
                {
                    // File
                    var directory = GetDirectoryName(entry.FullName);
                    CreateDirectory(directory);
                    using var reader = new StreamReader(entry.Open());
                    var content = reader.ReadToEnd();
                    WriteFile(entry.FullName, content);
                }
                else
                {
                    // Directory
                    CreateDirectory(entry.FullName);
                }
            }
        }

        public string CurrentDirectory => _fileSystem.Directory.GetCurrentDirectory();
        public string ResolvePath(string path) => path;
        public string GetDirectoryName(string path) => _fileSystem.Path.GetDirectoryName(path);
        public string GetFileName(string path) => _fileSystem.Path.GetFileName(path);
        public string GetFileExtension(string path) => _fileSystem.Path.GetExtension(path);
        public string CombinePaths(string path1, string path2) => _fileSystem.Path.Combine(path1, path2);
        public void CreateDirectory(string path) => _fileSystem.Directory.CreateDirectory(path);
        public IEnumerable<string> GetFiles(string path) => _fileSystem.Directory.GetFiles(path);
        public IEnumerable<string> GetDirectories(string path) => _fileSystem.Directory.GetDirectories(path);
        public string ReadFile(string path) => _fileSystem.File.ReadAllText(path);
        public void WriteFile(string path, string content) => _fileSystem.File.WriteAllText(path, content);
        public void CopyFile(string sourcePath, string targetPath) => _fileSystem.File.Copy(sourcePath, targetPath);
        public bool Validate(string path, bool @throw)
        {
            if (File.Exists(path) || Directory.Exists(path))
                return true;

            return @throw
                ? throw Exceptions.InvalidFileSystemObject(path)
                : false;
        }
    }
}