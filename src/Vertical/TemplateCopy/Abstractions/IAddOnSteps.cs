// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

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
