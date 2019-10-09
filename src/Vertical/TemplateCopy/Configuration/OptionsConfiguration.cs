using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using Vertical.CommandLine;
using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Help;

namespace Vertical.TemplateCopy.Configuration
{
    /// <summary>
    /// Represents the options configuration.
    /// </summary>
    public class OptionsConfiguration : ApplicationConfiguration<Options>
    {
        private static readonly IDictionary<string, LogEventLevel> LogEventLevelMap =
            new Dictionary<string, LogEventLevel>(StringComparer.OrdinalIgnoreCase)
            {
                ["v"] = LogEventLevel.Verbose,
                ["verbose"] = LogEventLevel.Verbose,
                ["d"] = LogEventLevel.Debug,
                ["debug"] = LogEventLevel.Debug,
                ["i"] = LogEventLevel.Information,
                ["info"] = LogEventLevel.Information,
                ["information"] = LogEventLevel.Information,
                ["q"] = LogEventLevel.Error,
                ["quiet"] = LogEventLevel.Error
            };

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="handler">Handler</param>
        public OptionsConfiguration(Action<Options> handler)
        {
            Switch("--clean-and-overwrite", arg => arg.Map.ToProperty(opt => opt.CleanOverwrite));
            
            Option("-l|--logger", arg => arg.Map.Using(MapLogEventLevel));
            Option("-o|--output", arg => arg.Map.Using((opt, value) => opt.OutputPath = MapFullPath(value)));
            Option("-t|--template", arg => arg.Map.Using((opt, value) => opt.TemplatePath = MapFullPath(value)));
            Option("-v|--var", arg => arg.MapMany.Using(MapVariable));
            Option("-a|--arg-pattern", arg => arg.Map.ToProperty(opt => opt.ArgumentVariableMatchPattern));
            Option("-e|--env-pattern", arg => arg.Map.ToProperty(opt => opt.EnvironmentVariableMatchPattern));
            Option("-m|--macro-pattern", arg => arg.Map.ToProperty(opt => opt.MacroMatchPattern));

            Help.UseFile(Path.Combine(Path.GetDirectoryName(typeof(OptionsConfiguration).Assembly.Location)
                , "Resources"
                , "help.txt"));

            HelpOption("-h|--help", InteractiveConsoleHelpWriter.Default);

            OnExecute(handler);
        }

        private string MapFullPath(string path)
        {
            try
            {
                return Path.GetFullPath(path);
            }
            catch (Exception ex)
            {
                throw new UsageException($"Could not expand given path '{path}' - {ex.Message}");
            }
        }

        private void MapLogEventLevel(Options opt, string value)
        {
            try { opt.LoggerLevel = LogEventLevelMap[value]; }
            catch (KeyNotFoundException)
            {
                throw new UsageException($"Invalid logging verbosity level '{value}'");
            }
        }

        private void MapVariable(Options opt, string value)
        {
            var split = value.Split('=');
            if (split.Length != 2)
            {
                throw new UsageException("Invalid value for -v|--var option, use key=value format.\n"
                    + "\tExample: -v count=2\n"
                    + "If you need spaces in your value, enclose quote the entire option value.\n"
                    + "\tExample: -v \"name=Dwight Schrute\"");
            }
            opt.Variables[split[0].Trim()] = split[1].Trim();
        }
    }
}
