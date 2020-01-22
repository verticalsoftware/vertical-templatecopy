// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Serilog;
using Serilog.Context;
using Serilog.Events;

namespace Vertical.Tools.TemplateCopy
{
    /// <summary>
    /// Represents extensions to logging.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class LoggerExtensions
    {
        private static int _indent = 0;

        // Manage indentation
        private sealed class IndentContext : IDisposable
        {
            private readonly IDisposable _context;
            public IndentContext()
            {
                _indent++;
                _context = LogContext.PushProperty("Indent", $"{new string('-', _indent)}> ");
            }
            
            void IDisposable.Dispose()
            {
                _indent--;
                _context.Dispose();
            }
        }
        
        /// <summary>
        /// Begins an indented section.
        /// </summary>
        public static IDisposable Indent(this ILogger _) => new IndentContext();

        /// <summary>
        /// Begins an indented section.
        /// </summary>
        public static IDisposable Indent(this ILogger logger
            , LogEventLevel logEventLevel
            , string messageTemplate
            , params object[] propertyValue)
        {
            logger.Write(logEventLevel, messageTemplate, propertyValue);
            return logger.Indent();
        }

        /// <summary>
        /// Logs properties.
        /// </summary>
        public static void LogProperties(this ILogger logger
            , LogEventLevel logEventLevel
            , IEnumerable<(string key, string value)> properties)
        {
            var propertyArray = properties.ToArray();
            var maxKeyLength = propertyArray.Max(prop => prop.key.Length);

            foreach (var (k, v) in propertyArray.OrderBy(p => p.key))
            {
                var key = string.Format($"{{0,-{maxKeyLength}}}", k);
                logger.Write(logEventLevel, default(Exception), "{key} : {value}", key, v);
            }
        }

        /// <summary>
        /// Logs options
        /// </summary>
        public static void LogOptions(this ILogger logger
            , Options options)
        {
            logger.Debug("Command line options:");

            using (logger.Indent())
            {
                var properties = OptionsConfiguration.ExplainOptions(options);
                logger.LogProperties(LogEventLevel.Debug, properties);
            }
        }
    }
}