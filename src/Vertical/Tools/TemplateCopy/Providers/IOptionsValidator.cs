// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Vertical.Tools.TemplateCopy.Core;

namespace Vertical.Tools.TemplateCopy.Providers
{
    /// <summary>
    /// Validations command line options.
    /// </summary>
    public interface IOptionsValidator
    {
        /// <summary>
        /// Validates the options.
        /// </summary>
        /// <param name="options">Options</param>
        void Validate(Options options);
    }
}