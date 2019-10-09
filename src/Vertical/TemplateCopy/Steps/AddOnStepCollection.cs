using System.Collections.Generic;

namespace Vertical.TemplateCopy.Steps
{
    /// <summary>
    /// Adds on step collection
    /// </summary>
    public class AddOnStepCollection
    {
        /// <summary>
        /// Steps to be executed on rollback
        /// </summary>
        public Stack<IRunStep> RollbackSteps { get; } = new Stack<IRunStep>();

        /// <summary>
        /// Steps to be executed after last run step completes successfully.
        /// </summary>
        public Queue<IRunStep> FinalizeSteps { get; } = new Queue<IRunStep>();
    }
}
