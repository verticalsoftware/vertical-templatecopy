namespace Vertical.Tools.TemplateCopy
{
    /// <summary>
    /// Represents a step in the sequence of tasks.
    /// </summary>
    public interface ISequenceTask
    {
        /// <summary>
        /// Executes the task.
        /// </summary>
        void Execute();
    }
}