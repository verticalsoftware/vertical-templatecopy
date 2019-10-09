using Serilog.Events;
using System;
using System.Collections.Generic;

namespace Vertical.TemplateCopy.Configuration
{
    /// <summary>
    /// Represents program options.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Gets/sets logger event level
        /// </summary>
        public LogEventLevel LoggerLevel { get; set; } = LogEventLevel.Verbose;

        /// <summary>
        /// Gets/sets the output path
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// Gets/sets the template path
        /// </summary>
        public string TemplatePath { get; set; }

        /// <summary>
        /// Gets/sets clean & overwrite flag
        /// </summary>
        public bool CleanOverwrite { get; set; }

        /// <summary>
        /// Gets/sets the macro expansion pattern.
        /// </summary>
        public string MacroMatchPattern { get; set; } = @"\$\(@(?<fn>[\w]+)(\((?<arg>[^\)]+)\))?\)";

        /// <summary>
        /// Gets/sets the user variable pattern.
        /// </summary>
        public string ArgumentVariableMatchPattern { get; set; } = @"\$\((?<key>[^@)]+)\)";

        /// <summary>
        /// Gets/sets the environment variable match pattern.
        /// </summary>
        public string EnvironmentVariableMatchPattern { get; set; } = @"\$\(env:(?<key>[^@)]+)\)";

        /// <summary>
        /// Gets/sets the command line variables.
        /// </summary>
        public IDictionary<string, string> Variables { get; } = new Dictionary<string, string>();
    }
}
