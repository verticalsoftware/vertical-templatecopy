using System.Collections.Generic;
using Vertical.Tools.TemplateCopy.Core;

namespace Vertical.Tools.TemplateCopy.Providers
{
    /// <summary>
    /// Represents an object that manages user options.
    /// </summary>
    public interface IOptionsProvider
    {
        /// <summary>
        /// Gets the source paths.
        /// </summary>
        IEnumerable<string> SourcePaths { get; }
        
        /// <summary>
        /// Gets a collection of assembly reference locations.
        /// </summary>
        IEnumerable<string> AssemblyReferences { get; }
        
        /// <summary>
        /// Gets the target path.
        /// </summary>
        string TargetPath { get; }
        
        /// <summary>
        /// Gets a set of file extensions that should be transformed for content.
        /// </summary>
        IEnumerable<string> ContentFileExtensions { get; }
        
        /// <summary>
        /// Gets/sets whether to display the plan and not make permanent changes.
        /// </summary>
        bool PlanOnly { get; }
        
        /// <summary>
        /// Gets/sets whether to warn the user if symbols are found in content files
        /// that are not resolved.
        /// </summary>
        bool WarnSymbolsMissing { get; }
        
        /// <summary>
        /// Gets whether to overwrite files if they exist.
        /// </summary>
        bool OverwriteFiles { get; }
        
        /// <summary>
        /// Gets a collection of extension sources.
        /// </summary>
        IEnumerable<string> ExtensionScriptPaths { get; }
        
        /// <summary>
        /// Gets the property dictionary.
        /// </summary>
        IDictionary<string, string> Properties { get; }
        
        /// <summary>
        /// Gets the underlying options.
        /// </summary>
        Options Values { get; }
        
        /// <summary>
        /// Gets the symbol pattern.
        /// </summary>
        string SymbolPattern { get; }
    }
}