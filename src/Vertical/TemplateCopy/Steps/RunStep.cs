using Microsoft.Extensions.Logging;

namespace Vertical.TemplateCopy.Steps
{
    /// <summary>
    /// Base class for steps.
    /// </summary>
    public abstract class RunStep : IRunStep
    {
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="logger">The component logger.</param>
        /// <param name="name">Step name.</param>
        protected RunStep(ILogger logger, string name)
        {
            Name = name;
            Logger = logger;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Gets the step name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// When implemented by a class, runs a step.
        /// </summary>
        public abstract void Run();
    }
}
