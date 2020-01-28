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

namespace Vertical.Tools.TemplateCopy.Core
{
    /// <summary>
    /// Represents extensions to logging.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class LoggerExtensions
    {
        private static int _indent = 0;

        private sealed class NoContext : IDisposable
        {
            public static IDisposable Instance => new NoContext();
            void IDisposable.Dispose() {}
        }

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
            return logger.IsEnabled(logEventLevel)
                ? logger.Indent()
                : NoContext.Instance;
        }

        /// <summary>
        /// Logs properties.
        /// </summary>
        public static void LogProperties(this ILogger logger
            , LogEventLevel logEventLevel
            , IEnumerable<(string key, string value)> properties)
        {
            var propertyArray = properties.ToArray();

            if (propertyArray.Length == 0) { return; }
            
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
                var properties = options.Explain();
                logger.LogProperties(LogEventLevel.Debug, properties);
            }
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public static void Log(this ILogger logger, int level, string message, object[] values)
        {
            var logEventLevel = (LogEventLevel) level;

            // So we get structured output... ugly
            switch (values?.Length ?? 0)
            {
                case 0:
                    logger.Write(logEventLevel, message);
                    break;
                
                case 1:
                    logger.Write(logEventLevel, message, values[0]);
                    break;
                
                case 2:
                    logger.Write(logEventLevel, message, values[0], values[1]);
                    break;
                
                case 3:
                    logger.Write(logEventLevel, message, values[0], values[1], values[2]);
                    break;
                
                case 4:
                    logger.Write(logEventLevel, message, values[0], values[1], values[2], values[3]);
                    break;
                
                case 5:
                    logger.Write(logEventLevel, message, values[0], values[1], values[2], values[3], values[4]);
                    break;
                
                case 6:
                        logger.Write(logEventLevel, message, values[0], values[1], values[2], values[3], values[4], values[5]);
                        break;
                
                case 7:
                    logger.Write(logEventLevel, message, values[0], values[1], values[2], values[3], values[4], values[5], values[6]);
                    break;
                
                case 8:
                    logger.Write(logEventLevel, message, values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7]);
                    break;
                
                default:
                    logger.Write(logEventLevel, message, values);
                    break;
            }
        }
    }
}