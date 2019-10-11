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

namespace Vertical.TemplateCopy
{
    /// <summary>
    /// Runs steps registered in DI.
    /// </summary>
    public class Runner
    {
        private readonly IRunStep[] runSteps;
        private readonly IAddOnStepsRunner addOnStepRunner;
        private readonly ILogger<Runner> logger;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        public Runner(IEnumerable<IRunStep> runSteps
            , IAddOnStepsRunner addOnStepRunner
            , ILogger<Runner> logger)
        {
            this.runSteps = runSteps.ToArray();
            this.addOnStepRunner = addOnStepRunner;
            this.logger = logger;
        }

        /// <summary>
        /// Runs the steps
        /// </summary>
        public void Run()
        {
            try
            {
                foreach (var step in runSteps)
                {
                    RunStep(step);
                }

                addOnStepRunner.RunFinalizeSteps();
            }
            catch (AbortException)
            {
                logger.LogError("Process aborted");
                addOnStepRunner.RunRollbackSteps();
            }
            catch (Exception ex)
            {
                logger.LogError("Execption occurred: {ex}, begin rollback", ex);
                addOnStepRunner.RunRollbackSteps();
            }
        }

        private void RunStep(IRunStep step)
        {
            using (LogContext.PushProperty("step", $"({step.Name}) "))
            {
                step.Run();
            }
        }
    }
}
