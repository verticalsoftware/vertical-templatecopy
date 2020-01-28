// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

namespace Vertical.Tools.TemplateCopy.IO
{
    /// <summary>
    /// Represents an object that locates assemblies.
    /// </summary>
    public interface IAssemblyResolver
    {
        /// <summary>
        /// Finds an assembly.
        /// </summary>
        /// <param name="assembly">Assembly name or path.</param>
        /// <returns>Path</returns>
        string GetAssemblyPath(string assembly);
    }
}