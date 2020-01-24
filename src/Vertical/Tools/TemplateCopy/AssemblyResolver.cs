using Serilog;

namespace Vertical.Tools.TemplateCopy
{
    /// <inheritdoc />
    public class AssemblyResolver : IAssemblyResolver
    {
        private readonly IFileSystemAdapter _fileSystemAdapter;
        private readonly ILogger _logger;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="fileSystemAdapter">File system adapter</param>
        /// <param name="logger">Logger</param>
        public AssemblyResolver(IFileSystemAdapter fileSystemAdapter, ILogger logger)
        {
            _fileSystemAdapter = fileSystemAdapter;
            _logger = logger;
        }
        
        /// <inheritdoc />
        public string GetAssemblyPath(string assembly)
        {
            var extension = _fileSystemAdapter.GetFileExtension(assembly);

            if (".dll" != extension)
            {
                // Automatically append
                assembly += ".dll";
            }

            var result = TryResolvePath(assembly)
                   ?? TryResolvePath(_fileSystemAdapter.CurrentDirectory, assembly)
                   ?? TryResolvePath(_fileSystemAdapter.GetDirectoryName(typeof(object).Assembly.Location), assembly)
                   ?? throw Exceptions.InvalidAssemblyReference(assembly);
            
            _logger.Debug("Located assembly reference {result}", result);

            return result;
        }

        private string TryResolvePath(string path)
        {
            return _fileSystemAdapter.Validate(path, false)
                ? path
                : null;
        }

        private string TryResolvePath(string root, string path)
        {
            return TryResolvePath(_fileSystemAdapter.CombinePaths(root, path));
        }
    }
}