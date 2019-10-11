// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

namespace Vertical.TemplateCopy.Abstractions
{
    /// <summary>
    /// Represents a path context accessor.
    /// </summary>
    public interface IPathContextAccessor
    {
        /// <summary>
        /// Gets the current template item.
        /// </summary>
        string Template { get; }

        /// <summary>
        /// Gets the current target.
        /// </summary>
        string Target { get; }
    }
}
