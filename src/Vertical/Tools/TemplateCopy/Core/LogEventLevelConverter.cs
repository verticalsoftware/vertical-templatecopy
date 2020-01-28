using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Serilog.Events;
using Vertical.CommandLine;

namespace Vertical.Tools.TemplateCopy.Core
{
    /// <summary>
    /// Defines log levels.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class LogEventLevelConverter : Dictionary<string, LogEventLevel>
    {
        public static readonly LogEventLevelConverter Default = new LogEventLevelConverter();
        
        private LogEventLevelConverter() : base(StringComparer.OrdinalIgnoreCase)
        {
            Add("v", LogEventLevel.Verbose);
            Add("verbose", LogEventLevel.Verbose);
            Add("d", LogEventLevel.Debug);
            Add("debug", LogEventLevel.Debug);
            Add("i", LogEventLevel.Information);
            Add("info", LogEventLevel.Information);
            Add("information", LogEventLevel.Information);
            Add("w", LogEventLevel.Warning);
            Add("warn", LogEventLevel.Warning);
            Add("warning", LogEventLevel.Warning);
            Add("e", LogEventLevel.Error);
            Add("error", LogEventLevel.Error);
            Add("f", LogEventLevel.Fatal);
            Add("fatal", LogEventLevel.Fatal);
        }

        /// <summary>
        /// Gets the level.
        /// </summary>
        public LogEventLevel GetLevel(string key)
        {
            return TryGetValue(key, out var level)
                ? level
                : throw new UsageException($"Invalid verbosity level '{key}'");
        }
    }
}