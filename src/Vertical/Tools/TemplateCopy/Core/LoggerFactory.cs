// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Serilog;
using Serilog.Events;

namespace Vertical.Tools.TemplateCopy.Core
{
    /// <summary>
    /// Configures logging.
    /// </summary>
    public static class LoggerFactory
    {
        /// <summary>
        /// Creates the logger.
        /// </summary>
        /// <param name="options">Options</param>
        /// <returns><see cref="ILogger"/></returns>
        public static ILogger CreateLogger(Options options)
        {
            var outputTemplate = options.Verbosity >= LogEventLevel.Information
                ? "{Message:lj}{NewLine}{Exception}"
                : "[{Level:u3}] {Indent}{Message:lj}{NewLine}{Exception}";
            
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(options.Verbosity, outputTemplate)
                .Enrich.FromLogContext()
                .CreateLogger();
        }
    }
}