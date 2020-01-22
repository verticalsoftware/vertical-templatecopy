// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.Collections.Generic;
using System.Linq;
using Serilog;

namespace Vertical.Tools.TemplateCopy
{
    /// <summary>
    /// Implements the options validator.
    /// </summary>
    public class OptionsValidator : IOptionsValidator
    {
        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="fileSystem">File system</param>
        public OptionsValidator(ILogger logger, IFileSystem fileSystem)
        {
            _logger = logger;
            _fileSystem = fileSystem;
        }
        
        /// <summary>
        /// Validates all given files.
        /// </summary>
        private void ValidateFiles(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                _fileSystem.Validate(path);
            }
        }

        /// <inheritdoc />
        public void Validate(Options options)
        {
            _logger.Debug("Validating configuration");

            if (!options.SourcePaths.Any())
                throw Exceptions.NoSourcePaths();
            
            ValidateFiles(options.SourcePaths.Select(path => _fileSystem.ResolvePath(path)));
            ValidateFiles(options.ExtensionScriptPaths.Select(path => _fileSystem.ResolvePath(path)));
            ValidateFiles(options.AssemblyReferences.Select(path => _fileSystem.ResolvePath(path)));
        }
    }
}