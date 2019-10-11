// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using System.Linq;
using Vertical.TemplateCopy.Configuration;

namespace Vertical.TemplateCopy.Steps
{
    /// <summary>
    /// Dump configuration step (logs to debug)
    /// </summary>
    [ExcludeFromCodeCoverage]
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

        /// <summary>
        /// Executes the steps
        /// </summary>
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
                Logger.LogTrace("{key}:{value}", key, item.Item2);
            }
        }
    }
}
