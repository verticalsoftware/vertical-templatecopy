// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;

namespace Vertical.TemplateCopy
{
    /// <summary>
    /// Indicates a stop in process due to unrecoverable condition.
    /// </summary>
    public class AbortException : Exception
    {
        /// <summary>
        /// Creates a new isntance
        /// </summary>
        public AbortException() : base("Run step aborted")
        {
        }
    }
}
