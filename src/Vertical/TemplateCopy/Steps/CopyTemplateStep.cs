using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using Vertical.TemplateCopy.Configuration;
using Vertical.TemplateCopy.Text;
using Vertical.TemplateCopy.Utilities;

namespace Vertical.TemplateCopy.Steps
{
    /// <summary>
    /// Copies the template
    /// </summary>
    public class CopyTemplateStep : RunStep
    {
        private readonly Options options;
        private readonly FileSystem fileSystem;
        private readonly ITextTransform textTransform;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="options">Options</param>
        public CopyTemplateStep(ILogger<CopyTemplateStep> logger
            , Options options
            , FileSystem fileSystem
            , ITextTransform textTransform) 
            : base(logger, "CopyTemplate")
        {
            this.options = options;
            this.fileSystem = fileSystem;
            this.textTransform = textTransform;
        }

        /// <summary>
        /// Executes the step
        /// </summary>
        public override void Run()
        {
            CopyDirectoryRecursive(options.TemplatePath, options.OutputPath);
        }

        private void CopyTemplateDirectory(string source, string dest)
        {
            var directoryName = Path.GetFileName(source);
            var targetRoot = Path.Combine(dest, textTransform.TransformContent(directoryName));

            ValidateBaseOutputPath(targetRoot);
            CopyDirectoryRecursive(source, targetRoot);
        }

        private void CopyDirectoryRecursive(string source, string dest)
        {
            fileSystem.CreateDirectory(dest);

            foreach(var file in Directory.EnumerateFiles(source))
            {
                var fileName = Path.GetFileName(file);
                var transformedFileName = textTransform.TransformContent(fileName);
                fileSystem.CopyFile(file, Path.Combine(dest, transformedFileName), FileSystem.ReplaceFileOperation, textTransform.TransformContent);
            }

            foreach(var directory in Directory.EnumerateDirectories(source))
            {
                var pathName = Path.GetFileName(directory);
                var transformedPathName = textTransform.TransformContent(pathName);
                CopyDirectoryRecursive(directory, Path.Combine(dest, transformedPathName));
            }
        }

        private void ValidateBaseOutputPath(string path)
        {
            if (!Directory.Exists(path)) { return; }

            if (!Directory.EnumerateFileSystemEntries(path).Any()) { return; }

            if (!options.CleanOverwrite)
            {
                throw Logger.LogErrorWithAbort("Target output root directory {path} not empty (--clean-and-overwrite not set)", path);
            }

            fileSystem.CleanDirectory(path);
        }
    }
}
