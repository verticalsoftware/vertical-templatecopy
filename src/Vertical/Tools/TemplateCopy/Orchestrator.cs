// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.Collections.Generic;
using Serilog;

namespace Vertical.Tools.TemplateCopy
{
    /// <summary>
    /// Represents the object that orchestrates generation of the template.
    /// </summary>
    public class Orchestrator
    {
        private readonly ILogger _logger;
        private readonly IOptionsProvider _options;
        private readonly IEnumerable<ISequenceTask> _sequenceTasks;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="options">Options</param>
        /// <param name="sequenceTasks">Sequence tasks.</param>
        public Orchestrator(ILogger logger
            , IOptionsProvider options
            , IEnumerable<ISequenceTask> sequenceTasks)
        {
            _logger = logger;
            _options = options;
            _sequenceTasks = sequenceTasks;
        }

        /// <summary>
        /// Executes generation of the template.
        /// </summary>
        public void Run()
        {
            _logger.Information("Generating template assets to {target}", _options.TargetPath);

            foreach (var task in _sequenceTasks)
            {
                task.Execute();
            }

            _logger.Information("Operation completed successfully");
        }
    }
}