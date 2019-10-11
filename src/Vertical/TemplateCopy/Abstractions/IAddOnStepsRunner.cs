namespace Vertical.TemplateCopy.Abstractions
{
    public interface IAddOnStepsRunner
    {
        /// <summary>
        /// Runs the rollback steps.
        /// </summary>
        void RunRollbackSteps();

        /// <summary>
        /// Runs the finalize steps.
        /// </summary>
        void RunFinalizeSteps();
    }
}
