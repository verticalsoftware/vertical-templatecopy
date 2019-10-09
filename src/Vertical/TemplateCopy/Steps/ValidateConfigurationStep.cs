using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using Vertical.TemplateCopy.Configuration;

namespace Vertical.TemplateCopy.Steps
{
    /// <summary>
    /// Validates configuration
    /// </summary>
    public class ValidateConfigurationStep : RunStep
    {
        private readonly Options options;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="options">Options</param>
        public ValidateConfigurationStep(ILogger<ValidateConfigurationStep> logger,
            Options options) : base(logger, "ValidateCore")
        {
            this.options = options;
        }

        /// <summary>
        /// Executes the step
        /// </summary>
        public override void Run()
        {
            if (string.IsNullOrWhiteSpace(options.OutputPath))
            {
                throw Logger.LogErrorWithAbort("Missing required output path argument (-o | --output <PATH>, use --help to see options).");
            }
           
            if (string.IsNullOrWhiteSpace(options.TemplatePath))
            {
                throw Logger.LogErrorWithAbort("Missing required template path argument (-t | --template <PATH>, use --help to see options).");
            }

            if (!Directory.Exists(options.TemplatePath))
            {
                throw Logger.LogErrorWithAbort("Template directory {path} could not be found or is not accessible (-t | --template <PATH>)."
                    , options.TemplatePath);
            }

            Logger.LogInformation("Configuration validated");
        }
    }
}
