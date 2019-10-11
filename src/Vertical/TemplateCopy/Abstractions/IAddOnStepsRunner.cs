// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

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
