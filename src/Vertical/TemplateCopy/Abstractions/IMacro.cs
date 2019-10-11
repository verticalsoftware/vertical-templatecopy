// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

namespace Vertical.TemplateCopy.Abstractions
{
    /// <summary>
    /// Represents a runtime macro.
    /// </summary>
    public interface IMacro
    {
        /// <summary>
        /// Computes the value given the optional argument.
        /// </summary>
        /// <param name="argument">Argument</param>
        /// <returns>Result</returns>
        string ComputeValue(string argument);
    }
}
