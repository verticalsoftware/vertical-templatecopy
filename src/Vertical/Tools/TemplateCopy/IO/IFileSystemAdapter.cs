// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.Collections.Generic;

namespace Vertical.Tools.TemplateCopy.IO
{
    /// <summary>
    /// Abstracts the file system interface.
    /// </summary>
    public interface IFileSystemAdapter
    {
        /// <summary>
        /// Gets the current directory.
        /// </summary>
        string CurrentDirectory { get; }
        
        /// <summary>
        /// Resolves the path, inferring the working directory if needed.
        /// </summary>
        /// <param name="path">Path to resolve.</param>
        /// <returns>Path</returns>
        string ResolvePath(string path);

        /// <summary>
        /// Gets a directory name.
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>Path</returns>
        string GetDirectoryName(string path);
        
        /// <summary>
        /// Gets the file name from the path.
        /// </summary>
        string GetFileName(string path);

        /// <summary>
        /// Gets the extension from the path.
        /// </summary>
        string GetFileExtension(string path);

        /// <summary>
        /// Combines two paths.
        /// </summary>
        string CombinePaths(string path1, string path2);

        /// <summary>
        /// Creates a directory.
        /// </summary>
        void CreateDirectory(string path);

        /// <summary>
        /// Gets the file in a directory.
        /// </summary>
        IEnumerable<string> GetFiles(string path);

        /// <summary>
        /// Gets the directories in a directory.
        /// </summary>
        IEnumerable<string> GetDirectories(string path);

        /// <summary>
        /// Reads the contents of a file.
        /// </summary>
        string ReadFile(string path);

        /// <summary>
        /// Reads the file as a sequence of bytes.
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>File bytes</returns>
        byte[] ReadFileBytes(string path);
        
        /// <summary>
        /// Writes the contents of a file.
        /// </summary>
        void WriteFile(string path, string content);

        /// <summary>
        /// Copies the contents of a file.
        /// </summary>
        void CopyFile(string sourcePath, string targetPath);

        /// <summary>
        /// Validates a file or directory.
        /// </summary>
        /// <param name="path">Path to validate</param>
        /// <param name="throwException">Whether to throw an exception if the object is not valid.</param>
        bool Validate(string path, bool throwException);
    }
}