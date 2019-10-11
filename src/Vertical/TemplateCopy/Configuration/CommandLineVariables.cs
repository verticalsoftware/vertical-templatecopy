// Copyright(c) 2018-2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;

namespace Vertical.TemplateCopy.Configuration
{
    /// <summary>
    /// Wrapper around option variables.
    /// </summary>
    public class CommandLineVariables : Dictionary<string, string>
    {
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="options">Options</param>
        public CommandLineVariables(Options options) : base(options.Variables, StringComparer.OrdinalIgnoreCase)
        {
        }
    }
}
