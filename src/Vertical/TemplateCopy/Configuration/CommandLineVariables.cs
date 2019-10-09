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
