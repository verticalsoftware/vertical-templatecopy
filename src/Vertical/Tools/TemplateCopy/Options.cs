using System.Collections.Generic;
using Serilog.Events;

namespace Vertical.Tools.TemplateCopy
{
    /// <summary>
    /// Represents the orchestration options.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Gets the source paths.
        /// </summary>
        public ISet<string> SourcePaths { get; } = new HashSet<string>(5);
        
        /// <summary>
        /// Gets a collection of assembly reference locations.
        /// </summary>
        public ISet<string> AssemblyReferences { get; } = new HashSet<string>(5);
        
        /// <summary>
        /// Gets the target path.
        /// </summary>
        public string TargetPath { get; set; }
        
        /// <summary>
        /// Gets a set of file extensions that should be transformed for content.
        /// </summary>
        public ISet<string> ContentFileExtensions { get; } = new HashSet<string>();

        /// <summary>
        /// Gets/sets the log verbosity.
        /// </summary>
        public LogEventLevel Verbosity { get; set; } = LogEventLevel.Information;
        
        /// <summary>
        /// Gets/sets whether to display the plan and not make permanent changes.
        /// </summary>
        public bool PlanOnly { get; set; }
        
        /// <summary>
        /// Gets/sets whether to warn the user if symbols are found in content files
        /// that are not resolved.
        /// </summary>
        public bool WarnSymbolsMissing { get; set; }
        
        /// <summary>
        /// Gets whether to overwrite files if they exist.
        /// </summary>
        public bool OverwriteFiles { get; set; }
        
        /// <summary>
        /// Gets a collection of extension sources.
        /// </summary>
        public ISet<string> ExtensionScriptPaths { get; } = new HashSet<string>();
        
        /// <summary>
        /// Gets the property dictionary.
        /// </summary>
        public IDictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets the pattern to use when matching symbols.
        /// </summary>
        public string SymbolPattern { get; set; } = Constants.DefaultSymbolPattern;
    }
}