// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Diagnostics.CodeAnalysis;
using Serilog;
using Serilog.Core;
using Vertical.TemplateCopy.Configuration;
using MSLogger = Microsoft.Extensions.Logging.ILogger;
using MSLoggerEx = Microsoft.Extensions.Logging.LoggerExtensions;

namespace Vertical.TemplateCopy.Utilities
{
    /// <summary>
    /// Manages creation of the logger.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class LoggingBootstrapper
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
                .WriteTo.Console(options.LoggerLevel, outputTemplate: LoggingConstants.OutputTemplate)
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
