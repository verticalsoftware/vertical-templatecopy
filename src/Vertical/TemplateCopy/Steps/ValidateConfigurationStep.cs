// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using Microsoft.Extensions.Logging;
using System.IO.Abstractions;
using System.Text.RegularExpressions;
using Vertical.TemplateCopy.Configuration;
using Vertical.TemplateCopy.Utilities;

namespace Vertical.TemplateCopy.Steps
{
    /// <summary>
    /// Validates configuration
    /// </summary>
    public class ValidateConfigurationStep : RunStep
    {
        private readonly IFileSystem fileSystem;
        private readonly Options options;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="options">Options</param>
        public ValidateConfigurationStep(ILogger<ValidateConfigurationStep> logger
            , IFileSystem fileSystem
            , Options options) 
            : base(logger, "ValidateCore")
        {
            this.fileSystem = fileSystem;
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

            if (!fileSystem.Directory.Exists(options.TemplatePath))
            {
                throw Logger.LogErrorWithAbort("Template directory {path} could not be found or is not accessible (-t | --template <PATH>)."
                    , options.TemplatePath);
            }

            if (!ValidatePattern(options.ArgumentVariableMatchPattern))
            {
                throw Logger.LogErrorWithAbort("Argument match pattern '{value}' is an invalid regular expression."
                    , options.ArgumentVariableMatchPattern);
            }

            if (!ValidatePattern(options.EnvironmentVariableMatchPattern))
            {
                throw Logger.LogErrorWithAbort("Environment variable match pattern '{value}' is an invalid regular expression."
                    , options.EnvironmentVariableMatchPattern);
            }

            if (!ValidatePattern(options.MacroMatchPattern))
            {
                throw Logger.LogErrorWithAbort("Macro matching pattern '{value}' is an invalid regular expression."
                    , options.MacroMatchPattern);
            }

            Logger.LogInformation("Configuration validated");
        }

        private static bool ValidatePattern(string pattern)
        {
            try
            {
                return Regex.Match("abcde", pattern) != null;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
