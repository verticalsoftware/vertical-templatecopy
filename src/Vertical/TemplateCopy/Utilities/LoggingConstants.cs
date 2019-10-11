// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

namespace Vertical.TemplateCopy.Utilities
{
    /// <summary>
    /// Various constants related to logging.
    /// </summary>
    public static class LoggingConstants
    {
        /// <summary>
        /// Defines the new line string.
        /// </summary>
        public const string NewLine = "\r\n     ";

        /// <summary>
        /// Defines the output template.
        /// </summary>
        public const string OutputTemplate = "{Level:u3}: {step}{substep}{Message:lj}{NewLine}{Exception}";
    }
}