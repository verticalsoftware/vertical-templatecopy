// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Vertical.Tools.TemplateCopy.Core;
using Vertical.Tools.TemplateCopy.IO;
using Vertical.Tools.TemplateCopy.Providers;
using Vertical.Tools.TemplateCopy.Scripting;
using Vertical.Tools.TemplateCopy.Tasks;

namespace Vertical.Tools.TemplateCopy
{
    /// <summary>
    /// Represents the service root.
    /// </summary>
    public class Services
    {
        private readonly IServiceProvider _serviceProvider;

        private Services(Options options, Action<IServiceCollection> configureServices) => 
            _serviceProvider = BuildServiceProvider(options, configureServices);

        /// <summary>
        /// Creates a services instance.
        /// </summary>
        /// <param name="options">Command line options</param>
        /// <returns><see cref="Services"/></returns>
        public static Services Create(Options options) => new Services(options, null);
        
        internal static Services Create(Options options, Action<IServiceCollection> configureServices) =>
            new Services(options, configureServices);

        /// <summary>
        /// Gets the orchestrator.
        /// </summary>
        public TaskAggregator TaskAggregator => _serviceProvider.GetService<TaskAggregator>();

        /// <summary>
        /// Gets a service instance.
        /// </summary>
        internal T GetService<T>() => _serviceProvider.GetService<T>();

        private static IServiceProvider BuildServiceProvider(Options options
            , Action<IServiceCollection> configureServices)
        {
            var services = new ServiceCollection();

            services
                .AddSingleton(options)
                .AddSingleton(LoggerFactory.CreateLogger(options))
                .AddSingleton<TaskAggregator>()
                .AddSingleton<ICompiler, CSharpCompiler>()
                .AddSingleton<IContentResolver, ContentResolver>()
                .AddSingleton<ISymbolStore, EnvironmentSymbolStore>()
                .AddSingleton<ISymbolStore, ExtensionScriptSymbolStore>()
                .AddSingleton<ISymbolStore, OptionsSymbolStore>()
                .AddSingleton<IExtensionTypeActivator, ExtensionTypeActivator>()
                .AddSingleton<IOptionsValidator, OptionsValidator>()
                .AddSingleton<IFileSystemAdapter, FileSystemAdapter>()
                .AddSingleton<IOptionsProvider, OptionsProvider>()
                .AddSingleton<ISequenceTask, ValidateOptionsTask>()
                .AddSingleton<ISequenceTask, LoadSymbolsTask>()
                .AddSingleton<ISequenceTask, ProcessTemplatesTask>()
                .AddSingleton<IAssemblyResolver, AssemblyResolver>()
                .AddSingleton<IFileSystem, FileSystem>()
                ;
            
            configureServices?.Invoke(services);
            
            return services.BuildServiceProvider();
        }
    }
}