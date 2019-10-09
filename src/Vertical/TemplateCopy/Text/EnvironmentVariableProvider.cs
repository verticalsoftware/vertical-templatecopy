using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;
using Vertical.TemplateCopy.Configuration;

namespace Vertical.TemplateCopy.Text
{
    /// <summary>
    /// Provider for environment variables
    /// </summary>
    public class EnvironmentVariableProvider : ITextTransformProvider
    {
        private readonly ILogger<EnvironmentVariableProvider> logger;
        private readonly Options options;

        /// <summary>
        /// Creates a new isntance.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="options">Options</param>
        public EnvironmentVariableProvider(ILogger<EnvironmentVariableProvider> logger
            , Options options)
        {
            this.logger = logger;
            this.options = options;
        }

        /// <summary>
        /// Transforms content
        /// </summary>
        /// <param name="source">Source</param>
        /// <returns>Transformed content</returns>
        public string TransformContent(string source)
        {
            return Regex.Replace(source, options.EnvironmentVariableMatchPattern, match =>
            {
                var key = match.Groups["key"].Value;
                if (!string.IsNullOrWhiteSpace(key))
                {
                    var value = Environment.GetEnvironmentVariable(key);

                    logger.LogTrace("Replace {match} with command line variable value '{value}'"
                        , match.Value
                        , value);

                    return value;
                }

                // No key
                return match.Value;
            });
        }
    }
}
