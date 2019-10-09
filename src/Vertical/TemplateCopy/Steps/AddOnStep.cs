using Microsoft.Extensions.Logging;
using System;

namespace Vertical.TemplateCopy.Steps
{
    /// <summary>
    /// Represents an add-on step.
    /// </summary>
    /// <typeparam name="TState">State</typeparam>
    public class AddOnStep<TState> : IRunStep
    {
        private readonly TState state;
        private readonly Action<TState> action;

        public AddOnStep(string name
            , TState state
            , Action<TState> action)
        {
            Name = name;
            this.state = state;
            this.action = action;
        }

        public string Name { get; }

        public void Run() => action(state);
    }
}
