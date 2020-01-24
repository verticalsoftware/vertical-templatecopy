// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

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