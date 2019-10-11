// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using Vertical.TemplateCopy.Abstractions;
using static Dawn.Guard;

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

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="name">The step name.</param>
        /// <param name="state">The state to encapsulate.</param>
        /// <param name="action">The action the performs the step.</param>
        public AddOnStep(string name
            , TState state
            , Action<TState> action)
        {
            Name = Argument(name, nameof(name)).NotNull().NotWhiteSpace();

            this.state = state;
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        /// <summary>
        /// Gets the step name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Handles the step logic.
        /// </summary>
        public void Run() => action(state);
    }
}
