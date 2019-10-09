using System.Collections.Generic;
using System.Linq;

namespace Vertical.TemplateCopy.Text
{
    /// <summary>
    /// Composite text transformation component.
    /// </summary>
    public class CompositeTextTransform : ITextTransform
    {
        private readonly IEnumerable<ITextTransformProvider> providers;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="providers">Providers</param>
        public CompositeTextTransform(IEnumerable<ITextTransformProvider> providers)
        {
            this.providers = providers;
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
    }
}
