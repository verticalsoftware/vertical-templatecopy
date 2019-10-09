using Serilog;
using Serilog.Core;
using System;
using Vertical.TemplateCopy.Configuration;
using MSLogger = Microsoft.Extensions.Logging.ILogger;
using MSLoggerEx = Microsoft.Extensions.Logging.LoggerExtensions;

namespace Vertical.TemplateCopy
{
    /// <summary>
    /// Manages creation of the logger.
    /// </summary>
    public static class Logging
    {
        /// <summary>
        /// Creates the root logger.
        /// </summary>
        /// <param name="options">Options</param>
        /// <returns>Logger</returns>
        public static Logger CreateLogger(Options options)
        {
            var configuration = new LoggerConfiguration()
                .MinimumLevel.Is(options.LoggerLevel)
                .WriteTo.Console(options.LoggerLevel, outputTemplate: "[{Level:u3}] {step}{substep}{Message:lj}{NewLine}{Exception}")
                .Enrich.FromLogContext();

            return configuration.CreateLogger();
        }

        /// <summary>
        /// Logs an error and returns an abort exception.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="message">Message to log</param>
        /// <param name="args">Logger arguments</param>
        /// <returns><see cref="Exception"/></returns>
        public static Exception LogErrorWithAbort(this MSLogger logger, string message, params object[] args)
        {
            MSLoggerEx.LogError(logger, message, args);
            throw new AbortException();
        }
    }
}
