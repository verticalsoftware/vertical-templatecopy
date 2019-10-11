// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.Linq;
using Vertical.TemplateCopy.Abstractions;

namespace Vertical.TemplateCopy.Text
{
    /// <summary>
    /// Composite text transformation component.
    /// </summary>
    public class CompositeTextTransform : ITextTransform
    {
        private readonly IEnumerable<ITextTransformProvider> providers;
        private readonly IPathContext context;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="providers">Providers</param>
        public CompositeTextTransform(IEnumerable<ITextTransformProvider> providers
            , IPathContext context)
        {
            this.providers = providers ?? throw new ArgumentNullException(nameof(providers));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Transforms the content.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string TransformContent(string source)
        {
            return providers.Aggregate(source, (str, provider) => provider.TransformContent(str));
        }

        /// <inheritdoc />
        public string TransformContent(string source, string templateContext, string targetContext)
        {
            using (context.BeginPathContext(templateContext, targetContext))
            {
                return TransformContent(source);
            }
        }
    }
}
