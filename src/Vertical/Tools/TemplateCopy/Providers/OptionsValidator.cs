// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Serilog;
using Vertical.Tools.TemplateCopy.Core;
using Vertical.Tools.TemplateCopy.IO;

namespace Vertical.Tools.TemplateCopy.Providers
{
    /// <summary>
    /// Implements the options validator.
    /// </summary>
    public class OptionsValidator : IOptionsValidator
    {
        private readonly ILogger _logger;
        private readonly IFileSystemAdapter _fileSystemAdapter;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="fileSystemAdapter">File system</param>
        public OptionsValidator(ILogger logger, IFileSystemAdapter fileSystemAdapter)
        {
            _logger = logger;
            _fileSystemAdapter = fileSystemAdapter;
        }

        /// <inheritdoc />
        public void Validate(Options options)
        {
            _logger.Debug("Validating configuration");

            if (!options.SourcePaths.Any())
                throw Exceptions.NoSourcePaths();
            
            ValidateFiles(options.SourcePaths.Select(path => _fileSystemAdapter.ResolvePath(path)));
            ValidateFiles(options.ExtensionScriptPaths.Select(path => _fileSystemAdapter.ResolvePath(path)));
            ValidateFiles(options.AssemblyReferences.Select(path => _fileSystemAdapter.ResolvePath(path)));

            try
            {
                var _ = Regex.Match(string.Empty, options.SymbolPattern);
            }
            catch
            {
                throw Exceptions.InvalidSymbolPattern(options.SymbolPattern);
            }
        }
        
        /// <summary>
        /// Validates all given files.
        /// </summary>
        private void ValidateFiles(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                _fileSystemAdapter.Validate(path, true);
            }
        }
    }
}