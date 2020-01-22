// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Vertical.Tools.TemplateCopy
{
    /// <summary>
    /// Represents the service root.
    /// </summary>
    public class Services
    {
        private readonly IServiceProvider _serviceProvider;

        private Services(Options options) => _serviceProvider = BuildServiceProvider(options);

        /// <summary>
        /// Creates a services instance.
        /// </summary>
        /// <param name="options">Command line options</param>
        /// <returns><see cref="Services"/></returns>
        public static Services Create(Options options) => new Services(options);

        /// <summary>
        /// Gets the orchestrator.
        /// </summary>
        public Orchestrator Orchestrator => _serviceProvider.GetService<Orchestrator>();

        /// <summary>
        /// Gets the logger
        /// </summary>
        public ILogger Logger => _serviceProvider.GetService<ILogger>();

        private static IServiceProvider BuildServiceProvider(Options options)
        {
            var services = new ServiceCollection();

            services
                .AddSingleton(options)
                .AddSingleton(LoggerFactory.CreateLogger(options))
                .AddSingleton<Orchestrator>()
                .AddSingleton<ICompiler, CSharpCompiler>()
                .AddSingleton<IContentResolver, ContentResolver>()
                .AddSingleton<ISymbolStore, EnvironmentSymbolStore>()
                .AddSingleton<ISymbolStore, ExtensionScriptSymbolStore>()
                .AddSingleton<ISymbolStore, OptionsSymbolStore>()
                .AddSingleton<IOptionsValidator, OptionsValidator>()
                .AddSingleton<IFileSystem, FileSystem>()
                .AddSingleton<IOptionsProvider, OptionsProvider>()
                .AddSingleton<ISequenceTask, ValidateOptionsTask>()
                .AddSingleton<ISequenceTask, LoadSymbolsTask>()
                .AddSingleton<ISequenceTask, ProcessTemplatesTask>()
                ;
            
            return services.BuildServiceProvider();
        }
    }
}