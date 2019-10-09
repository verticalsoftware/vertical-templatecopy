using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using System;
using Vertical.TemplateCopy.Configuration;
using Vertical.TemplateCopy.Macros;
using Vertical.TemplateCopy.Steps;
using Vertical.TemplateCopy.Text;
using Vertical.TemplateCopy.Utilities;

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
        public Services(Options options, Logger logger)
        {
            var services = new ServiceCollection();

            services.AddSingleton(options);
            services.AddSingleton<CommandLineVariables>();
            services.AddLogging(config => config.AddSerilog(logger));
            services.AddSingleton<FileSystem>();
            services.AddSingleton<Runner>();
            services.AddSingleton<AddOnStepCollection>();
            services.AddSingleton<IRunStep, DumpConfigurationStep>();
            services.AddSingleton<IRunStep, ValidateConfigurationStep>();
            services.AddSingleton<IRunStep, CopyTemplateStep>();
            services.AddSingleton<ITextTransform, CompositeTextTransform>();
            services.AddSingleton<ITextTransformProvider, ArgumentVariableProvider>();
            services.AddSingleton<ITextTransformProvider, EnvironmentVariableProvider>();
            services.AddSingleton<ITextTransformProvider, MacroVariableProvider>();
            services.AddMacros();

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
