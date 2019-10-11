// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO.Abstractions;
using Vertical.TemplateCopy.Abstractions;
using Vertical.TemplateCopy.Configuration;
using Vertical.TemplateCopy.Context;
using Vertical.TemplateCopy.Macros;
using Vertical.TemplateCopy.Steps;
using Vertical.TemplateCopy.Text;

namespace Vertical.TemplateCopy
{
    /// <summary>
    /// Service module.
    /// </summary>
    public class Services : IDisposable
    {
        private readonly IServiceProvider _provider;
        private readonly IServiceScope _scope;        

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="options">Options</param>
        /// <param name="logger">Logger instance.</param>
        public Services(Options options, ILogger logger, IFileSystem fileSystem = null)
        {
            var services = new ServiceCollection();

            services.AddSingleton(options);
            services.AddSingleton<CommandLineVariables>();
            services.AddLogging(config => config.AddSerilog(logger));
            services.AddSingleton(fileSystem ?? new FileSystem());
            services.AddSingleton<Runner>();
            services.AddSingleton<AddOnStepCollection>();
            services.AddSingleton<IAddOnSteps>(provider => provider.GetService<AddOnStepCollection>());
            services.AddSingleton<IAddOnStepsRunner>(provider => provider.GetService<AddOnStepCollection>());
            services.AddSingleton<IRunStep, DumpConfigurationStep>();
            services.AddSingleton<IRunStep, ValidateConfigurationStep>();
            services.AddSingleton<IRunStep, CopyTemplateStep>();
            services.AddSingleton<ITextTransform, CompositeTextTransform>();
            services.AddSingleton<ITextTransformProvider, ArgumentVariableProvider>();
            services.AddSingleton<ITextTransformProvider, EnvironmentVariableProvider>();
            services.AddSingleton<ITextTransformProvider, MacroVariableProvider>();
            services.AddMacros();
            services.AddSingleton<PathContext>();
            services.AddSingleton<IPathContextAccessor>(provider => provider.GetService<PathContext>());
            services.AddSingleton<IPathContext>(provider => provider.GetService<PathContext>());

            _provider = services.BuildServiceProvider();
            _scope = _provider.CreateScope();
        }

        /// <summary>
        /// Releases components in current the service scope.
        /// </summary>
        public void Dispose() => _scope.Dispose();

        /// <summary>
        /// Gets the runner component.
        /// </summary>
        public Runner Runner => _scope.ServiceProvider.GetService<Runner>();
    }
}
