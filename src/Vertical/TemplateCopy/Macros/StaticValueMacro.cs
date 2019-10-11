// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Vertical.TemplateCopy.Abstractions;

namespace Vertical.TemplateCopy.Macros
{
    /// <summary>
    /// Represents a macro that returns a static value.
    /// </summary>
    public class StaticValueMacro : IMacro
    {
        private readonly string value;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="value">Value.</param>
        public StaticValueMacro(string value) => this.value = value;

        /// <summary>
        /// Returns the stored value.
        /// </summary>
        /// <param name="argument">Not used.</param>
        /// <returnsString</returns>
        public string ComputeValue(string argument) => value;
    }
}
