using Microsoft.Extensions.Logging;
using System.Linq;
using Vertical.TemplateCopy.Configuration;

namespace Vertical.TemplateCopy.Steps
{
    /// <summary>
    /// Dump configuration step (logs to debug)
    /// </summary>
    public class DumpConfigurationStep : RunStep
    {
        private readonly Options options;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="options">Options</param>
        public DumpConfigurationStep(ILogger<DumpConfigurationStep> logger, Options options) 
            : base(logger, "DumpConfiguration")
        {
            this.options = options;
        }

        public override void Run()
        {
            var values = new (string, object)[]
            {
                ("cleanOverwrite", options.CleanOverwrite),
                ("outputPath", options.OutputPath),
                ("logVerbosity", options.LoggerLevel),
                ("templatePath", options.TemplatePath),

            }
            .Concat(options.Variables.Select(kv => ($"$({kv.Key})", (object)kv.Value)))
            .ToArray();

            var maxLength = -values.Max(t => t.Item1.Length + 2);

            foreach(var item in values)
            {
                var key = string.Format($"{{0,{maxLength}}}", item.Item1);
                Logger.LogDebug("{key}:{value}", key, item.Item2);
            }
        }
    }
}
