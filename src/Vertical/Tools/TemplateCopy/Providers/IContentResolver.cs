// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

namespace Vertical.Tools.TemplateCopy.Providers
{
    /// <summary>
    /// Represents an object that resolves symbols in content.
    /// </summary>
    public interface IContentResolver
    {
        /// <summary>
        /// Initializes the internal symbol stores.
        /// </summary>
        void LoadSymbols();
        
        /// <summary>
        /// Resolves mapped symbols in the given content.
        /// </summary>
        /// <param name="content">Content to resolve.</param>
        /// <returns>Transformed string</returns>
        string ReplaceSymbols(string content);
    }
}