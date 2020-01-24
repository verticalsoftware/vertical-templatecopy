// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Serilog;

namespace Vertical.Tools.TemplateCopy
{
    /// <summary>
    /// Represents a symbol store for command line options.
    /// </summary>
    public class OptionsSymbolStore : ISymbolStore
    {
        private readonly ILogger _logger;
        private readonly IDictionary<string, string> _properties;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="options">Options</param>
        /// <param name="logger">Logger</param>
        public OptionsSymbolStore(IOptionsProvider options, ILogger logger)
        {
            _logger = logger;
            _properties = options.Properties;
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public void Build()
        {
            _logger.Debug("Loading symbols passed in options");

            using var _ = _logger.Indent();

            foreach (var (key, value) in _properties)
            {
                _logger.Verbose("{key}={value}", key, value);
            }
        }

        /// <inheritdoc />
        public Func<string> GetValueFunction(string key)
        {
            return _properties.TryGetValue(key, out var value)
                ? () => value
                : default(Func<string>);
        }
    }
}