// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.Collections.Generic;
using Vertical.Tools.TemplateCopy.Providers;

namespace Vertical.Tools.TemplateCopy.Tasks
{
    /// <summary>
    /// Symbols loader.
    /// </summary>
    public class LoadSymbolsTask : ISequenceTask
    {
        private readonly IEnumerable<ISymbolStore> _symbolStores;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="symbolStores">Symbol stores.</param>
        public LoadSymbolsTask(IEnumerable<ISymbolStore> symbolStores)
        {
            _symbolStores = symbolStores;
        }
        
        /// <inheritdoc />
        public void Execute()
        {
            foreach (var store in _symbolStores)
            {
                store.Build();
            }
        }
    }
}