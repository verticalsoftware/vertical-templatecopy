// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.Linq;
using Serilog;
using Serilog.Events;
using Vertical.Tools.TemplateCopy.Core;
using Vertical.Tools.TemplateCopy.IO;
using Vertical.Tools.TemplateCopy.Providers;

namespace Vertical.Tools.TemplateCopy.Tasks
{
    /// <summary>
    /// Represents a task that processes the templates.
    /// </summary>
    public class ProcessTemplatesTask : ISequenceTask
    {
        private readonly IFileSystemAdapter _fileSystemAdapter;
        private readonly IContentResolver _contentResolver;
        private readonly ILogger _logger;
        private readonly IOptionsProvider _options;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="fileSystemAdapter">File system</param>
        /// <param name="contentResolver">Content resolver</param>
        /// <param name="logger">Logger</param>
        /// <param name="options">Options</param>
        public ProcessTemplatesTask(IFileSystemAdapter fileSystemAdapter
            , IContentResolver contentResolver
            , ILogger logger
            , IOptionsProvider options)
        {
            _fileSystemAdapter = fileSystemAdapter;
            _contentResolver = contentResolver;
            _logger = logger;
            _options = options;
        }

        private bool MatchAnyExtension => !_options.ContentFileExtensions.Any();
        
        /// <inheritdoc />
        public void Execute()
        {
            _logger.Information("Generating template assets to {path}", _options.TargetPath);
            
            var sourcePaths = _options.SourcePaths.Select(_fileSystemAdapter.ResolvePath);
            var targetPath = _fileSystemAdapter.ResolvePath(_options.TargetPath);
            
            foreach (var sourcePath in sourcePaths)
            {
                ProcessTemplateDirectory(sourcePath, targetPath, true);
            }
        }

        /// <summary>
        /// Processes a single directory and its children
        /// </summary>
        private void ProcessTemplateDirectory(string sourcePath, string targetPath, bool isSourceRoot)
        {
            _logger.Debug("Processing template directory, {source} -> {target}"
                , sourcePath
                , targetPath);

            var finalTargetPath = isSourceRoot 
                ? targetPath 
                : TransformToFinalPath(sourcePath, targetPath);
            
            _fileSystemAdapter.CreateDirectory(finalTargetPath);
            
            ProcessTemplateFiles(sourcePath, finalTargetPath);
            ProcessTemplateDirectories(sourcePath, finalTargetPath);
        }

        /// <summary>
        /// Processes child directories
        /// </summary>
        private void ProcessTemplateDirectories(string sourcePath, string targetPath)
        {
            foreach (var directoryPath in _fileSystemAdapter.GetDirectories(sourcePath))
            {
                ProcessTemplateDirectory(directoryPath, targetPath, false);
            }
        }

        /// <summary>
        /// Processes child files
        /// </summary>
        private void ProcessTemplateFiles(string sourcePath, string targetPath)
        {
            foreach (var filePath in _fileSystemAdapter.GetFiles(sourcePath))
            {
                ProcessTemplateFile(filePath, targetPath);
            }
        }   

        /// <summary>
        /// Processes a single template file
        /// </summary>
        private void ProcessTemplateFile(string filePath, string targetPath)
        {
            var finalFilePath = TransformToFinalPath(filePath, targetPath);
            var extension = _fileSystemAdapter.GetFileExtension(finalFilePath);

            if (MatchAnyExtension || _options.ContentFileExtensions.Contains(extension))
            {
                TransformTemplateFile(filePath, finalFilePath);
            }
            else
            {
                _fileSystemAdapter.CopyFile(filePath, finalFilePath);
            }
        }

        /// <summary>
        /// Transforms a template file
        /// </summary>
        private void TransformTemplateFile(string sourcePath, string targetPath)
        {
            using var _ = _logger.Indent(LogEventLevel.Debug, "Transforming content file {source}", sourcePath);
            var content = _fileSystemAdapter.ReadFile(sourcePath);
            
            _fileSystemAdapter.WriteFile(targetPath, _contentResolver.ReplaceSymbols(content));
        }

        /// <summary>
        /// Forms a final target path with transformations
        /// </summary>
        private string TransformToFinalPath(string sourcePath, string targetPath)
        {
            var fileName = _fileSystemAdapter.GetFileName(sourcePath);
            var transformedFileName = _contentResolver.ReplaceSymbols(fileName);
            var combinedPath = _fileSystemAdapter.CombinePaths(targetPath, transformedFileName);

            if (fileName != transformedFileName)
            {
                _logger.Verbose("Transforming object name {source} -> {target}"
                    , sourcePath
                    , combinedPath);
            }
            
            return combinedPath;
        }
    }
}