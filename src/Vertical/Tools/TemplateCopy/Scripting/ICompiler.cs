// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Vertical.Tools.TemplateCopy.Core;

namespace Vertical.Tools.TemplateCopy.Scripting
{
    /// <summary>
    /// Represents a compiler.
    /// </summary>
    public interface ICompiler
    {
        /// <summary>
        /// Gets the language.
        /// </summary>
        Language Language { get; }
        
        /// <summary>
        /// Compiles the given source code.
        /// </summary>
        /// <param name="source">The source code to compile.</param>
        /// <returns>Emitted byte array.</returns>
        byte[] Compile(string source);
    }
}