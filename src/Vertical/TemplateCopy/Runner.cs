using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using Vertical.TemplateCopy.Steps;

namespace Vertical.TemplateCopy
{
    /// <summary>
    /// Runs steps registered in DI.
    /// </summary>
    public class Runner
    {
        private readonly IRunStep[] runSteps;
        private readonly AddOnStepCollection addOnStepCollection;
        private readonly ILogger<Runner> logger;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        public Runner(IEnumerable<IRunStep> runSteps
            , AddOnStepCollection addOnStepCollection
            , ILogger<Runner> logger)
        {
            this.runSteps = runSteps.ToArray();
            this.addOnStepCollection = addOnStepCollection;
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
                ExecuteAddOnSteps(() => addOnStepCollection.FinalizeSteps.Any()
                    ? addOnStepCollection.FinalizeSteps.Dequeue()
                    : null);
            }
            catch (AbortException)
            {
                logger.LogError("Abort occurred, begin rollback");
                ExecuteAddOnSteps(() => addOnStepCollection.RollbackSteps.Any()
                    ? addOnStepCollection.RollbackSteps.Pop()
                    : null);
            }
        }        

        private void ExecuteAddOnSteps(Func<IRunStep> provider)
        {
            IRunStep step;

            while((step = provider()) != null)
            {
                RunStep(step);
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
