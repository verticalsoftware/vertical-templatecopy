using System.Collections.Generic;

namespace Vertical.Tools.TemplateCopy
{
    /// <summary>
    /// Represents an object that manages user options.
    /// </summary>
    public interface IOptionsProvider
    {
        /// <summary>
        /// Gets the source paths.
        /// </summary>
        public IEnumerable<string> SourcePaths { get; }
        
        /// <summary>
        /// Gets a collection of assembly reference locations.
        /// </summary>
        public IEnumerable<string> AssemblyReferences { get; }
        
        /// <summary>
        /// Gets the target path.
        /// </summary>
        public string TargetPath { get; }
        
        /// <summary>
        /// Gets a set of file extensions that should be transformed for content.
        /// </summary>
        public IEnumerable<string> ContentFileExtensions { get; }
        
        /// <summary>
        /// Gets/sets whether to display the plan and not make permanent changes.
        /// </summary>
        public bool PlanOnly { get; }
        
        /// <summary>
        /// Gets/sets whether to warn the user if symbols are found in content files
        /// that are not resolved.
        /// </summary>
        public bool WarnSymbolsMissing { get; }
        
        /// <summary>
        /// Gets whether to overwrite files if they exist.
        /// </summary>
        public bool OverwriteFiles { get; }
        
        /// <summary>
        /// Gets a collection of extension sources.
        /// </summary>
        public IEnumerable<string> ExtensionScriptPaths { get; }
        
        /// <summary>
        /// Gets the property dictionary.
        /// </summary>
        public IDictionary<string, string> Properties { get; }
        
        /// <summary>
        /// Gets the underlying options.
        /// </summary>
        public Options Values { get; }
    }
}