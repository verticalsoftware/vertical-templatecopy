using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using Vertical.TemplateCopy.Configuration;
using Vertical.TemplateCopy.Utilities;

namespace Vertical.TemplateCopy.Steps
{
    /// <summary>
    /// Archive output directory step.
    /// </summary>
    public class ArchiveOutputDirectoryStep : RunStep
    {
        private readonly Options options;
        private readonly FileSystem fileSystem;
        private readonly AddOnStepCollection addOnSteps;

        public ArchiveOutputDirectoryStep(ILogger<ArchiveOutputDirectoryStep> logger
            , Options options
            , FileSystem fileSystem
            , AddOnStepCollection addonSteps)
            : base(logger, "ArchiveOutputDirectory")
        {
            this.options = options;
            this.fileSystem = fileSystem;
            this.addOnSteps = addonSteps;
        }

        /// <summary>
        /// Executes the step.
        /// </summary>
        public override void Run()
        {
            if (Skip) { return; }

            var archivePath = fileSystem.GetTemporaryDirectoryName();

            CreateArchive(archivePath);

            addOnSteps.RollbackSteps.Push(CreateRollback(archivePath));
            addOnSteps.FinalizeSteps.Enqueue(CreateFinalize(archivePath));
        }

        private IRunStep CreateRollback(string archivePath)
        {
            return new AddOnStep<(ILogger logger, FileSystem fs, string arch)>("ArchiveOutputDirectory:Rollback"
                , (Logger, fileSystem, archivePath)
                , state =>
                {
                    state.logger.LogInformation("Restore output directory {output}", options.OutputPath);
                    state.fs.CleanDirectory(options.OutputPath);
                    state.fs.CopyDirectory(state.arch, options.OutputPath, true, FileSystem.ReplaceFileOperation);
                });                
        }

        private IRunStep CreateFinalize(string archivePath)
        {
            return new AddOnStep<(ILogger logger, FileSystem fs, string arch)>("ArchiveOutputDirectory:Finalize"
                , (Logger, fileSystem, archivePath)
                , state =>
                {
                    state.logger.LogInformation("Remove output directory archive {path}", state.arch);
                    state.fs.DeleteDirectoryFast(archivePath, true);
                });
        }

        private void CreateArchive(string archivePath)
        {
            Logger.LogInformation("Temporarily archive output path to {temp}", archivePath);

            fileSystem.CreateDirectory(archivePath);
            fileSystem.CopyDirectory(options.OutputPath, archivePath, true, FileSystem.ReplaceFileOperation);
        }

        private bool Skip
        {
            get
            {
                if (!options.CleanOverwrite)
                {
                    Logger.LogDebug("Skip step (no --clean-and-overwrite)");
                    return true;
                }

                if (!Directory.Exists(options.OutputPath))
                {
                    Logger.LogDebug("Skip step, {path} does not exist", options.OutputPath);
                    return true;
                }

                if (!Directory.GetFileSystemEntries(options.OutputPath).Any())
                {
                    Logger.LogDebug("Skip step, {path} is empty", options.OutputPath);
                    return true;
                }

                return false;
            }
        }
    }
}
