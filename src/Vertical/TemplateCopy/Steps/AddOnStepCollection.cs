// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using Vertical.TemplateCopy.Abstractions;

namespace Vertical.TemplateCopy.Steps
{
    /// <summary>
    /// Adds on step collection
    /// </summary>
    public class AddOnStepCollection : IAddOnSteps, IAddOnStepsRunner
    {
        private readonly ILogger<AddOnStepCollection> logger;

        private Stack<IRunStep> rollbackSteps = new Stack<IRunStep>();
        private Queue<IRunStep> finalizeSteps = new Queue<IRunStep>();

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public AddOnStepCollection(ILogger<AddOnStepCollection> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void AddFinalizeStep(IRunStep runStep) => finalizeSteps.Enqueue(runStep ?? throw new ArgumentNullException(nameof(runStep)));

        /// <inheritdoc />
        public void AddRollbackStep(IRunStep runStep) => rollbackSteps.Push(runStep ?? throw new ArgumentNullException(nameof(runStep)));

        /// <inheritdoc />
        public void RunFinalizeSteps()
        {
            while (finalizeSteps.Any()) RunStep(finalizeSteps.Dequeue());
        }

        /// <inheritdoc />
        public void RunRollbackSteps()
        {
            while (rollbackSteps.Any()) RunStep(rollbackSteps.Pop());
        }

        private void RunStep(IRunStep step)
        {
            using (LogContext.PushProperty("step", $"({step.Name}) "))
            {
                try
                {
                    step.Run();
                }
                catch(Exception ex)                
                {
                    logger.LogError("Caught exception during rollback or finalize step: {message}", ex.Message);
                }
            }
        }
    }
}
