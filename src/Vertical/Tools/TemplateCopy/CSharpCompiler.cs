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

namespace Vertical.Tools.TemplateCopy
{
    /// <summary>
    /// Represents a compiler implemented using Roslyn and Microsoft CSharp code analysis.
    /// </summary>
    public class CSharpCompiler : ICompiler
    {
        private readonly IOptionsProvider _options;

        private static readonly MetadataReference[] CoreReferences = 
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Data.DataColumn).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(File).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Net.Cookie).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Net.Http.HttpClient).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Xml.NameTable).Assembly.Location)
        };

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
        public CSharpCompiler(IOptionsProvider options)
        {
            _options = options;
        }

        /// <inheritdoc />
        public Language Language => Language.CSharp;

        /// <inheritdoc />
        public byte[] Compile(string source)
        {
            using var peStream = new MemoryStream();

            var codeString = SourceText.From(source);
            
            var syntaxTree = SyntaxFactory.ParseSyntaxTree(codeString, ParseOptions);

            var compilation = CSharpCompilation.Create("TemplateCopyMacros"
                , new[] {syntaxTree}
                , references: CoreReferences
                , options: CompileOptions);

            var emitResult = compilation.Emit(peStream);

            CheckCompilationResult(emitResult);

            peStream.Seek(0, SeekOrigin.Begin);

            return peStream.ToArray();
        }

        private IEnumerable<MetadataReference> BuildMetadataReferences => CoreReferences.Concat(
            _options.AssemblyReferences.Select(asmRef => MetadataReference.CreateFromFile(asmRef)));
        
        private static void CheckCompilationResult(EmitResult emitResult)
        {
            if (emitResult.Success) return;
            
            var exceptions = emitResult
                .Diagnostics
                .Select(entry => new InvalidOperationException(entry.GetMessage()));

            throw new AggregateException(exceptions);
        }
    }
}