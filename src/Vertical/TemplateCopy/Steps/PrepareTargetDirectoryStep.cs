using Microsoft.Extensions.Logging;
using System.IO;
using Vertical.TemplateCopy.Configuration;
using Vertical.TemplateCopy.Utilities;

namespace Vertical.TemplateCopy.Steps
{
    public class PrepareTargetDirectoryStep : RunStep
    {
        private readonly Options options;
        private readonly FileSystem fileSystem;
        private readonly AddOnStepCollection addOnSteps;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="options">Options</param>
        /// <param name="fileSystem">File System</param>
        /// <param name="addOnSteps">Add on steps</param>
        public PrepareTargetDirectoryStep(ILogger<PrepareTargetDirectoryStep> logger
            , Options options
            , FileSystem fileSystem
            , AddOnStepCollection addOnSteps)
            : base(logger, "PrepareTarget")
        {
            this.options = options;
            this.fileSystem = fileSystem;
            this.addOnSteps = addOnSteps;
        }

        /// <summary>
        /// Executes the step
        /// </summary>
        public override void Run()
        {
            var path = options.OutputPath;
            var exists = Directory.Exists(path);

            Logger.LogInformation("Prepare output directory {path}", path);

            if (options.CleanOverwrite && exists)
            {
                fileSystem.CleanDirectory(path);
            }

            if (!exists)
            {
                fileSystem.CreateDirectory(path);
                addOnSteps.RollbackSteps.Push(CreateRollback(path));               
            }
        }

        private IRunStep CreateRollback(string path)
        {
            return new AddOnStep<(ILogger log, FileSystem fs, string path)>("PrepareTarget:Rollback"
                , (Logger, fileSystem, path)
                , state =>
                {
                    state.log.LogInformation("Remove output directory {path}", state.path);
                    state.fs.DeleteDirectoryFast(state.path, true);
                });
        }
    }
}
