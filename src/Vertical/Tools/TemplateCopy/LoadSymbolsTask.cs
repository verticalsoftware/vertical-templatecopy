using System.Collections.Generic;

namespace Vertical.Tools.TemplateCopy
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