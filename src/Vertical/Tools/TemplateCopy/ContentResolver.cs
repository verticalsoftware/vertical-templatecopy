// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.ObjectPool;
using Serilog;
using Serilog.Events;

namespace Vertical.Tools.TemplateCopy
{
    /// <summary>
    /// Represents an object that transforms content with symbol replacement.
    /// </summary>
    public class ContentResolver : IContentResolver
    {
        /// <summary>
        /// Defines the default symbol matching pattern.
        /// </summary>
        public const string DefaultSymbolPattern = @"\$\{(?<symbol>[a-zA-Z0-9_]+)\}";
        
        private readonly ObjectPool<StringBuilder> _stringBuilderPool = 
            new DefaultObjectPool<StringBuilder>(new PoolPolicy(), 5);
        private readonly ILogger _logger;
        private readonly IOptionsProvider _options;
        private readonly ISymbolStore[] _symbolStores;

        private class PoolPolicy : IPooledObjectPolicy<StringBuilder>
        {
            /// <inheritdoc />
            public StringBuilder Create() => new StringBuilder(4096);

            /// <inheritdoc />
            public bool Return(StringBuilder obj)
            {
                obj.Clear();
                return true;
            }
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="symbolStores">Collection of symbol stores</param>
        /// <param name="options">Options</param>
        public ContentResolver(ILogger logger
            , IEnumerable<ISymbolStore> symbolStores
            , IOptionsProvider options)
        {
            _logger = logger;
            _options = options;
            _symbolStores = symbolStores.ToArray();
        }

        /// <inheritdoc />
        public void LoadSymbols()
        {
            foreach (var symbolStore in _symbolStores)
            {
                symbolStore.Build();
            }
        }

        /// <inheritdoc />
        public string ReplaceSymbols(string content)
        {
            var builder = _stringBuilderPool.Get();

            try
            {
                return ReplaceSymbols(content, builder);
            }
            finally
            {
                _stringBuilderPool.Return(builder);   
            }
        }

        private string ReplaceSymbols(string content, StringBuilder builder)
        {
            var match = Regex.Match(content, _options.SymbolPattern);
            var position = 0;
            var span = content.AsSpan();
            
            for (; match.Success; match = match.NextMatch())
            {
                builder.Append(span.Slice(position, match.Index - position));
                position += match.Index - position;

                var symbol = match.Groups["symbol"].Value;
                var value = ResolveSymbol(symbol) ?? match.Value;

                builder.Append(value);
                position += match.Length;
            }

            builder.Append(span.Slice(position));

            return builder.ToString();
        }

        private string ResolveSymbol(string symbol)
        {
            var match = _symbolStores
                .Select(store => new {store, function = store.GetValueFunction(symbol)})
                .LastOrDefault(item => item.function != null);

            var value = match?.function?.Invoke();
            
            if (match != null)
                _logger.Debug("Resolved symbol ${{{symbol}}} from {store}={value}", symbol, match.store.GetType().Name, value);
            else
            {
                var eventLevel = _options.WarnSymbolsMissing ? LogEventLevel.Warning : LogEventLevel.Debug;
                _logger.Write(eventLevel, "Symbol ${{{symbol}}} not found", symbol);
            }

            return value;
        }
    }
}