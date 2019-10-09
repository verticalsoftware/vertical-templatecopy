namespace Vertical.TemplateCopy.Steps
{
    /// <summary>
    /// Represents a run step.
    /// </summary>
    public interface IRunStep
    {
        /// <summary>
        /// Name of the step.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Executes the logic of the step
        /// </summary>
        void Run();
    }
}
