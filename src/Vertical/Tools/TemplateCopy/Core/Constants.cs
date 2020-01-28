// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

namespace Vertical.Tools.TemplateCopy.Core
{
    /// <summary>
    /// Defines program constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Defines the default symbol matching pattern.
        /// </summary>
        public const string DefaultSymbolPattern = @"\$\{(?<symbol>[a-zA-Z0-9_]+)\}";
        
        /// <summary>
        /// Defines the extensions wildcard.
        /// </summary>
        public const string ExtensionWildcard = ".*";
    }
}