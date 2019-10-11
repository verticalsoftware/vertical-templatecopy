// Copyright(c) 2018-2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using Vertical.TemplateCopy.Abstractions;

namespace Vertical.TemplateCopy.Macros
{
    /// <summary>
    /// Represents a macro that wraps an underlying delegate.
    /// </summary>
    public class DelegateMacro : IMacro
    {
        private readonly Func<string, string> argumentFunction;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="argumentFunction">A function that acquires the macro result.</param>
        public DelegateMacro(Func<string, string> argumentFunction) => 
            this.argumentFunction = argumentFunction ?? throw new ArgumentNullException(nameof(argumentFunction));

        public string ComputeValue(string argument) => argumentFunction(argument);
    }
}
