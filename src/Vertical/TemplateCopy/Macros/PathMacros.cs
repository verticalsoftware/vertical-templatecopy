// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.IO;
using Vertical.TemplateCopy.Abstractions;
using static System.Environment;

namespace Vertical.TemplateCopy.Macros
{
    /// <summary>
    /// Defines path macros.
    /// </summary>
    public static class PathMacros
    {
        /// <summary>
        /// Returns a special folder path
        /// </summary>
        public static IMacro SpecialFolder = new DelegateMacro(arg => GetFolderPath(Enum.Parse<SpecialFolder>(arg, true)));

        /// <summary>
        /// Returns a string that expands dot notation identifier to a path.
        /// </summary>
        public static IMacro ExpandDotToPath = new DelegateMacro(arg => arg.Replace('.', Path.DirectorySeparatorChar));
    }
}
