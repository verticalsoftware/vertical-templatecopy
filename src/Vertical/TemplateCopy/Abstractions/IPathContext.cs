// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

namespace Vertical.TemplateCopy.Abstractions
{
    /// <summary>
    /// Represents an object that can be used to manage items in the path context.
    /// </summary>
    public interface IPathContext : IPathContextAccessor
    {
        /// <summary>
        /// Begins a new path context.
        /// </summary>
        /// <param name="template">The template path to push onto the stack.</param>
        /// <param name="target">The target path to push on the stack.</param>
        /// <returns>Object that when disposed, restores the stack to its previous state.</returns>
        System.IDisposable BeginPathContext(string template, string target);
    }
}
