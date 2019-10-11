// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Microsoft.Extensions.Logging;
using System.IO;
using System.IO.Abstractions;
using Vertical.TemplateCopy.Abstractions;
using Vertical.TemplateCopy.Configuration;
using Vertical.TemplateCopy.Utilities;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Vertical.TemplateCopy.Steps
{
    /// <summary>
    /// Copies the template
    /// </summary>
    public class CopyTemplateStep : RunStep
    {
        private readonly Options options;
        private readonly IFileSystem fileSystem;
        private readonly ITextTransform textTransform;
        private readonly IPathContext pathContext;
        private readonly IAddOnSteps addOnSteps;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="options">Options</param>
        public CopyTemplateStep(ILogger<CopyTemplateStep> logger
            , Options options
            , IFileSystem fileSystem
            , ITextTransform textTransform
            , IPathContext pathContext
            , IAddOnSteps addOnSteps) 
            : base(logger, "CopyTemplate")
        {
            this.options = options;
            this.fileSystem = fileSystem;
            this.textTransform = textTransform;
            this.pathContext = pathContext;
            this.addOnSteps = addOnSteps;
        }

        /// <summary>
        /// Executes the step
        /// </summary>
        public override void Run()
        {
            CopyDirectoryRecursive(options.TemplatePath, options.OutputPath);
            Logger.LogInformation("Template assets created in {path}", options.OutputPath);
        }

        /// <summary>
        /// Recursively copies and transforms template files.
        /// </summary>
        private void CopyDirectoryRecursive(string source, string dest, int iteration = 0)
        {
            CreateTargetDirectory(dest, iteration);

            var relativeRoot = dest.Substring(options.OutputPath.Length);

            foreach(var file in fileSystem.Directory.EnumerateFiles(source))
            {
                var fileName = Path.GetFileName(file);
                var transformedFileName = textTransform.TransformContent(fileName
                    , file
                    , $"resolveFileName:{fileName}");

                var targetPath = Path.Combine(dest, transformedFileName);

                CheckDuplicateObject(targetPath);

                var content = fileSystem.File.ReadAllText(file);
                var transformedContent = textTransform.TransformContent(content, file, targetPath);

                fileSystem.File.WriteAllText(targetPath, transformedContent);

                AddRollbackStepForObject(targetPath, "File");
            }

            foreach(var directory in fileSystem.Directory.EnumerateDirectories(source))
            {
                var pathName = Path.GetFileName(directory);
                var transformedPathName = textTransform.TransformContent(pathName
                    , directory
                    , $"resolveDirectoryName:{pathName}");

                CopyDirectoryRecursive(directory, Path.Combine(dest, transformedPathName), iteration + 1);
            }
        }
        
        /// <summary>
        /// Creates a target directory.
        /// </summary>
        private void CreateTargetDirectory(string path, int iteration)
        {
            if (fileSystem.Directory.Exists(path)) return;

            fileSystem.Directory.CreateDirectory(path);
            AddRollbackStepForObject(path, "Directory");
        }

        /// <summary>
        /// Adds a rollback step to remove the new object.
        /// </summary>
        private void AddRollbackStepForObject(string path, string type)
        {
            addOnSteps.AddRollbackStep(new AddOnStep<(ILogger log, IFileSystem fs, string path)>($"Create{type}:Rollback"
                , (Logger, fileSystem, path)
                , state =>
                {
                    var fs = state.fs;
                    var exists = fs.Directory.Exists(state.path) || fs.File.Exists(state.path);
                    if (!exists) { return; }

                    state.log.LogInformation("Delete directory {path}", state.path);
                    state.fs.Directory.Delete(state.path, true);
                }));
        }

        /// <summary>
        /// Checks for a duplicate object.
        /// </summary>
        private void CheckDuplicateObject(string path)
        {
            if (!fileSystem.File.Exists(path))
                return;

            if (!options.CleanOverwrite)
            {
                Logger.LogErrorWithAbort("File already exists {path} and will not be overwritten (use --overwrite to get past this)."
                    , path);
            }

            Logger.LogWarning("File {path} was overwritten, which means the output tree is not completely fresh."
                + "{n}There may be artifacts in the output directory that were not generated by this utility."
                , path
                , LoggingConstants.NewLine);
        }
    }
}
