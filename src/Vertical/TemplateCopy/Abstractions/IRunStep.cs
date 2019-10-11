// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

namespace Vertical.TemplateCopy.Abstractions
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
