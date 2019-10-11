namespace Vertical.TemplateCopy.Abstractions
{
    public interface IAddOnSteps
    {
        /// <summary>
        /// Adds a rollback step.
        /// </summary>
        /// <param name="runStep">Run step to add.</param>
        void AddRollbackStep(IRunStep runStep);

        /// <summary>
        /// Adds a finalization step.
        /// </summary>
        /// <param name="runStep">Run step to add.</param>
        void AddFinalizeStep(IRunStep runStep);        
    }
}
