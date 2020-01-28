using System.Collections.Generic;
using Vertical.Tools.TemplateCopy.Core;

namespace Vertical.Tools.TemplateCopy.Providers
{
    /// <summary>
    /// Represents an options provider.
    /// </summary>
    public class OptionsProvider : IOptionsProvider
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="values">Values</param>
        public OptionsProvider(Options values)
        {
            Values = values;
        }
        
        /// <inheritdoc />
        public Options Values { get; }

        /// <inheritdoc />
        public string SymbolPattern => Values.SymbolPattern;

        /// <inheritdoc />
        public IEnumerable<string> SourcePaths => Values.SourcePaths;

        /// <inheritdoc />
        public IEnumerable<string> AssemblyReferences => Values.AssemblyReferences;

        /// <inheritdoc />
        public string TargetPath => Values.TargetPath;

        /// <inheritdoc />
        public IEnumerable<string> ContentFileExtensions => Values.ContentFileExtensions;

        /// <inheritdoc />
        public bool PlanOnly => Values.PlanOnly;

        /// <inheritdoc />
        public bool WarnSymbolsMissing => Values.WarnSymbolsMissing;

        /// <inheritdoc />
        public bool OverwriteFiles => Values.OverwriteFiles;

        /// <inheritdoc />
        public IEnumerable<string> ExtensionScriptPaths => Values.ExtensionScriptPaths;

        /// <inheritdoc />
        public IDictionary<string, string> Properties => Values.Properties;
    }
}