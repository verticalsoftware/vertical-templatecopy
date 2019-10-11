// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

namespace Vertical.TemplateCopy.Abstractions
{
    /// <summary>
    /// Provider for <see cref="ITextTransform"/>
    /// </summary>
    public interface ITextTransformProvider
    {
        /// <summary>
        /// Transforms content.
        /// </summary>
        /// <param name="source">The source string to transform.</param>
        /// <returns>String</returns>
        string TransformContent(string source);
    }
}
