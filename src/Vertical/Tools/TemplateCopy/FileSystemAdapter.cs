// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Serilog;

namespace Vertical.Tools.TemplateCopy
{
    /// <summary>
    /// Base class for the file system.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FileSystemAdapter : IFileSystemAdapter
    {
        private readonly ILogger _logger;
        private readonly IOptionsProvider _options;

        /// <summary>
        /// Creates a new instance 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        public FileSystemAdapter(ILogger logger, IOptionsProvider options)
        {
            _logger = logger;
            _options = options;
        }

        /// <inheritdoc />
        public string CurrentDirectory => Directory.GetCurrentDirectory();

        /// <inheritdoc />
        public string ResolvePath(string path)
        {
            return Path.IsPathRooted(path)
                ? path
                : Path.GetFullPath(path);
        }

        /// <inheritdoc />
        public string GetDirectoryName(string path) => Path.GetDirectoryName(path);

        /// <inheritdoc />
        public string GetFileName(string path) => Path.GetFileName(path);

        /// <inheritdoc />
        public string GetFileExtension(string path) => Path.GetExtension(path);

        /// <inheritdoc />
        public string CombinePaths(string path1, string path2) => Path.Combine(path1, path2);

        /// <inheritdoc />
        public void CreateDirectory(string path)
        {
            _logger.Debug("Creating directory {path}", path);

            ExecuteOrPlan(() => Directory.CreateDirectory(path));
        }

        /// <inheritdoc />
        public IEnumerable<string> GetFiles(string path) => Directory.GetFiles(path);

        /// <inheritdoc />
        public IEnumerable<string> GetDirectories(string path) => Directory.GetDirectories(path);

        /// <inheritdoc />
        public string ReadFile(string path) => File.ReadAllText(path);

        /// <inheritdoc />
        public void WriteFile(string path, string content)
        {
            _logger.Debug("Writing file content to {path} (length={length})", path, content.Length);

            ExecuteOrPlan(() => File.WriteAllText(ValidateWritePath(path), content));
        }

        /// <inheritdoc />
        public void CopyFile(string sourcePath, string targetPath)
        {
            _logger.Debug("Copying file {source} -> {target}", sourcePath, targetPath);

            ExecuteOrPlan(() => File.Copy(sourcePath, ValidateWritePath(targetPath)));
        }

        /// <inheritdoc />
        public bool Validate(string path, bool throwException)
        {
            if (File.Exists(path) || Directory.Exists(path))
                return true;

            return throwException
                ? throw Exceptions.InvalidFileSystemObject(path)
                : false;
        }

        private string ValidateWritePath(string path)
        {
            return _options.OverwriteFiles || !File.Exists(path)
                ? path
                : throw Exceptions.FileExists(path);
        }

        private void ExecuteOrPlan(Action action)
        {
            if (_options.PlanOnly) return;
            action();
        }
    }
}