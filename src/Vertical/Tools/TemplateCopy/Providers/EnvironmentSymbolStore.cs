// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using Serilog.Events;
using Vertical.Tools.TemplateCopy.Core;

namespace Vertical.Tools.TemplateCopy.Providers
{
    /// <summary>
    /// Represents an environment variable store.
    /// </summary>
    public class EnvironmentSymbolStore : ISymbolStore
    {
        private readonly ILogger _logger;
        private readonly IDictionary<string, string> _variableDictionary =
            new Dictionary<string, string>(64);

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        public EnvironmentSymbolStore(ILogger logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public void Build()
        {
            _logger.Debug("Loading symbols from environment variables");

            using var _ = _logger.Indent();

            var variables = Environment
                .GetEnvironmentVariables()
                .Cast<DictionaryEntry>()
                .Select(entry => ((string) entry.Key, (string) entry.Value))
                .ToArray();

            _logger.LogProperties(LogEventLevel.Debug, variables);
            
            foreach(var (key, value) in variables) { _variableDictionary.Add(key, value); }
        }
        
        /// <inheritdoc />
        public Func<string> GetValueFunction(string key)
        {
            return _variableDictionary.TryGetValue(key, out var value)
                ? () => value
                : default(Func<string>);
        }
    }
}