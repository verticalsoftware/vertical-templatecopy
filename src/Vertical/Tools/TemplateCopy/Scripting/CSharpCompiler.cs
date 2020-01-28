// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using Serilog;
using Serilog.Events;
using Vertical.Tools.TemplateCopy.Core;
using Vertical.Tools.TemplateCopy.IO;
using Vertical.Tools.TemplateCopy.Providers;

namespace Vertical.Tools.TemplateCopy.Scripting
{
    /// <summary>
    /// Represents a compiler implemented using Roslyn and Microsoft CSharp code analysis.
    /// </summary>
    public class CSharpCompiler : ICompiler
    {
        internal static readonly string[] CoreAssemblies =
        {
            "mscorlib.dll",
            "netstandard.dll",
            "System.dll", 
            "System.Core.dll", 
            "System.Runtime.dll",
            "System.Collections.dll",
            "System.Data.dll",
            "System.Data.Common.dll",
            "System.IO.dll",
            "System.Linq.dll",
            "System.Net.dll",
            "System.Net.Http.dll",
            "System.Threading.dll",
            "System.Xml.dll",
            "System.Private.CoreLib.dll"
        };
        
        private readonly IOptionsProvider _options;
        private readonly ILogger _logger;
        private readonly IAssemblyResolver _assemblyResolver;
        private readonly IFileSystemAdapter _fileSystemAdapter;
        private readonly Lazy<IList<MetadataReference>> _lazyMetadataReferences;

        private static readonly CSharpCompilationOptions CompileOptions = 
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary
            , optimizationLevel: OptimizationLevel.Release
            , assemblyIdentityComparer: AssemblyIdentityComparer.Default);
        
        private static readonly CSharpParseOptions ParseOptions = CSharpParseOptions
            .Default
            .WithLanguageVersion(LanguageVersion.Latest);

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="options">Options.</param>
        /// <param name="logger">Logger</param>
        /// <param name="assemblyResolver">Assembly resolver</param>
        /// <param name="fileSystemAdapter">File system adapter</param>
        public CSharpCompiler(IOptionsProvider options
            , ILogger logger
            , IAssemblyResolver assemblyResolver
            , IFileSystemAdapter fileSystemAdapter)
        {
            _options = options;
            _logger = logger;
            _assemblyResolver = assemblyResolver;
            _fileSystemAdapter = fileSystemAdapter;
            _lazyMetadataReferences = new Lazy<IList<MetadataReference>>(BuildCoreReferences);
        }

        /// <inheritdoc />
        public Language Language => Language.CSharp;

        /// <inheritdoc />
        public byte[] Compile(string source)
        {
            using var peStream = new MemoryStream();

            source += Environment.NewLine;
            
            var codeString = SourceText.From(source);
            
            var syntaxTree = SyntaxFactory.ParseSyntaxTree(codeString, ParseOptions);

            var compilation = CSharpCompilation.Create("TemplateCopyMacros"
                , new[] {syntaxTree}
                , references: _lazyMetadataReferences.Value
                , options: CompileOptions);

            var emitResult = compilation.Emit(peStream);

            CheckCompilationResult(emitResult);

            peStream.Seek(0, SeekOrigin.Begin);

            return peStream.ToArray();
        }

        private static void CheckCompilationResult(EmitResult emitResult)
        {
            if (emitResult.Success) return;
            
            var exceptions = emitResult
                .Diagnostics
                .Select(entry => new InvalidOperationException(entry.GetMessage()));

            throw new AggregateException(exceptions);
        }

        private IList<MetadataReference> BuildCoreReferences()
        {
            using var _ = _logger.Indent(LogEventLevel.Verbose, "Building core metadata references");

            var coreAssemblyPaths = CoreAssemblies.Select(path => _assemblyResolver.GetAssemblyPath(path));
            
            var userAssemblyPaths = _options
                .AssemblyReferences
                .Select(_assemblyResolver.GetAssemblyPath);

            var combinedPaths = coreAssemblyPaths.Concat(userAssemblyPaths);

            return combinedPaths
                .Select(CreateMetadataReference)
                .ToList();
        }

        private MetadataReference CreateMetadataReference(string path)
        {
            using var stream = new MemoryStream(_fileSystemAdapter.ReadFileBytes(path));
            
            return MetadataReference.CreateFromStream(stream);
        }
    }
}