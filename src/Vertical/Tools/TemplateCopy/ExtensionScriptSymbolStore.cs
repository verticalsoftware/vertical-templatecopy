// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Serilog;
using Serilog.Events;

namespace Vertical.Tools.TemplateCopy
{
    /// <summary>
    /// Symbol store that uses compiled code.
    /// </summary>
    public class ExtensionScriptSymbolStore : ISymbolStore
    {
        private readonly ICompiler _compiler;
        private readonly ILogger _logger;
        private readonly IOptionsProvider _options;
        private readonly IFileSystemAdapter _fileSystemAdapter;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="compiler">Compiler</param>
        /// <param name="logger">Logger</param>
        /// <param name="options">Options</param>
        /// <param name="fileSystemAdapter">File system</param>
        public ExtensionScriptSymbolStore(ICompiler compiler
            , ILogger logger
            , IOptionsProvider options
            , IFileSystemAdapter fileSystemAdapter)
        {
            _compiler = compiler;
            _logger = logger;
            _options = options;
            _fileSystemAdapter = fileSystemAdapter;
        }

        private IDictionary<string, Func<string>> Functions { get; set; }

        /// <inheritdoc />
        public Func<string> GetValueFunction(string key)
        {
            return Functions.TryGetValue(key, out var function)
                ? function
                : null;
        }
        
        /// <inheritdoc />
        public void Build()
        {
            _logger.Debug("Loading symbols from extension scripts");

            using var _ = _logger.Indent();
            
            var entries = _options
                .ExtensionScriptPaths
                .Select(_fileSystemAdapter.ResolvePath)
                .SelectMany(LoadEntriesFromSource)
                .ToArray();
            
            var dictionary = new Dictionary<string, Func<string>>(entries.Length);

            foreach (var (key, value) in entries)
            {
                dictionary[key] = value;
            }

            var properties = dictionary.Select(kvp => (kvp.Key, $"(function)={kvp.Value()}"));
            
            _logger.LogProperties(LogEventLevel.Verbose, properties);
            
            Functions = dictionary;
        }

        private IEnumerable<KeyValuePair<string, Func<string>>> LoadEntriesFromSource(string path)
        {
            _logger.Debug("Building extension script {path}", path);

            using var _ = _logger.Indent();
            
            var source = _fileSystemAdapter.ReadFile(path);
            var assembly = CompileToAssembly(path, source);
            var exportedType = GetExportedType(assembly, path);
            var instance = CreateExportedTypeInstance(exportedType, path);
            var instanceXp = Expression.Constant(instance);

            return exportedType
                .GetProperties()
                .Where(pInfo => pInfo.PropertyType == typeof(string))
                .Select(pInfo => BuildCompiledPropertyEntry(pInfo, instanceXp))
                .ToArray();
        }

        private KeyValuePair<string, Func<string>> BuildCompiledPropertyEntry(PropertyInfo propertyInfo
            , Expression instanceXp)
        {
            _logger.Verbose("Compiling lambda for {type}.{property}"
                , propertyInfo.DeclaringType
                , propertyInfo.Name);

            var propertyXp = Expression.Property(instanceXp, propertyInfo);
            var lambda = Expression.Lambda<Func<string>>(propertyXp);
            var function = lambda.Compile();

            return new KeyValuePair<string, Func<string>>(propertyInfo.Name, function);
        }

        private object CreateExportedTypeInstance(Type exportedType, string path)
        {
            var propertyConstructor = exportedType.GetConstructor(new[] {typeof(IDictionary<string, string>)});

            if (propertyConstructor != null)
            {
                _logger.Verbose("Exported type {type} in {path} uses property dictionary constructor."
                    , exportedType
                    , path);
                return propertyConstructor.Invoke(new object[] {_options.Properties});
            }

            var defaultConstructor = exportedType.GetConstructor(Array.Empty<Type>());

            if (defaultConstructor == null) throw Exceptions.IncompatibleTypeConstructor(exportedType, path);
            
            _logger.Verbose("Exported type {type} in {path} uses default constructor."
                , exportedType
                , path);
            
            return defaultConstructor.Invoke(Array.Empty<object>());

        }

        private Assembly CompileToAssembly(string path, string source)
        {
            _logger.Debug("Compiling extension source @: {path}", path);

            var assemblyBytes = _compiler.Compile(source);
            return Assembly.Load(assemblyBytes);
        }

        private Type GetExportedType(Assembly assembly, string path)
        {
            try
            {
                var type = assembly.ExportedTypes.Single();
                _logger.Debug("Found exported type {type} in source {path}", type.Name, path);
                return type;
            }
            catch (InvalidOperationException)
            {
                throw Exceptions.NoSingleExportedClass(path);
            }
        }
    }
}