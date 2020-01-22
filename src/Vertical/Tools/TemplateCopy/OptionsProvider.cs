using System.Collections.Generic;
using System.Linq;

namespace Vertical.Tools.TemplateCopy
{
    /// <summary>
    /// Represents an options provider.
    /// </summary>
    public class OptionsProvider : IOptionsProvider
    {
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="values">Values</param>
        /// <param name="fileSystem">File system abstraciton.</param>
        public OptionsProvider(Options values, IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            Values = values;
        }
        
        public Options Values { get; }

        /// <inheritdoc />
        public IEnumerable<string> SourcePaths => Values.SourcePaths.Select(_fileSystem.ResolvePath);

        /// <inheritdoc />
        public IEnumerable<string> AssemblyReferences => Values.AssemblyReferences.Select(_fileSystem.ResolvePath);

        /// <inheritdoc />
        public string TargetPath => _fileSystem.ResolvePath(Values.TargetPath);

        /// <inheritdoc />
        public IEnumerable<string> ContentFileExtensions => Values.ContentFileExtensions;

        /// <inheritdoc />
        public bool PlanOnly => Values.PlanOnly;

        /// <inheritdoc />
        public bool WarnSymbolsMissing => Values.WarnSymbolsMissing;

        /// <inheritdoc />
        public bool OverwriteFiles => Values.OverwriteFiles;

        /// <inheritdoc />
        public IEnumerable<string> ExtensionScriptPaths => Values.ExtensionScriptPaths.Select(_fileSystem.ResolvePath);

        /// <inheritdoc />
        public IDictionary<string, string> Properties => Values.Properties;
    }
}