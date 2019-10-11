// Copyright(c) 2018-2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.Linq;
using Vertical.TemplateCopy.Abstractions;

namespace Vertical.TemplateCopy.Context
{
    /// <summary>
    /// Represents the current path context.
    /// </summary>
    public class PathContext : IPathContext
    {
        private readonly Stack<(string,string)> pathStack = new Stack<(string,string)>();

        /// <summary>
        /// Provides a mechanism that restores the stack when disposed.
        /// </summary>
        private sealed class PopStackItemContext : IDisposable
        {
            private readonly Stack<(string, string)> stack;

            public PopStackItemContext(Stack<(string, string)> stack, string template, string target)
            {
                this.stack = stack;
                stack.Push((template, target));
            }

            public void Dispose() => stack.Pop();
        }

        /// <summary>
        /// Gets the current item.
        /// </summary>
        public string Template => pathStack.Any() ? pathStack.Peek().Item1 : null;

        public string Target => pathStack.Any() ? pathStack.Peek().Item2 : null;

        /// <summary>
        /// Begins a new path context.
        /// </summary>
        /// <param name="template">The template path to push onto the stack.</param>
        /// <param name="target">The target path to push onto the stack.</param>
        /// <returns>Object that when disposed, restores the stack to its previous state.</returns>
        public IDisposable BeginPathContext(string template, string target) => 
            new PopStackItemContext(pathStack, template, target);
    }
}
