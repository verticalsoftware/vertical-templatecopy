// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using Vertical.TemplateCopy.Abstractions;

namespace Vertical.TemplateCopy.Macros
{
    /// <summary>
    /// Represents a path context macro.
    /// </summary>
    public class PathContextMacro : IMacro
    {
        private readonly IPathContextAccessor contextAccessor;
        private readonly Func<IPathContextAccessor, string> selectorFunction;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="contextAccessor">Context accessor.</param>
        public PathContextMacro(IPathContextAccessor contextAccessor
            , Func<IPathContextAccessor, string> selectorFunction)
        {
            this.contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            this.selectorFunction = selectorFunction ?? throw new ArgumentNullException(nameof(selectorFunction));
        }

        /// <summary>
        /// Gets the current path context.
        /// </summary>
        public string ComputeValue(string argument) => selectorFunction(contextAccessor);
    }
}
