// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;
using Vertical.TemplateCopy.Abstractions;
using Vertical.TemplateCopy.Configuration;
using Vertical.TemplateCopy.Utilities;

namespace Vertical.TemplateCopy.Text
{
    /// <summary>
    /// Provider for environment variables
    /// </summary>
    public class EnvironmentVariableProvider : ITextTransformProvider
    {
        private readonly ILogger<EnvironmentVariableProvider> logger;
        private readonly Options options;
        private readonly IPathContextAccessor pathContext;

        /// <summary>
        /// Creates a new isntance.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="options">Options</param>
        public EnvironmentVariableProvider(ILogger<EnvironmentVariableProvider> logger
            , Options options
            , IPathContextAccessor pathContext)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.pathContext = pathContext ?? throw new ArgumentNullException(nameof(pathContext));
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

                var exists = Environment.GetEnvironmentVariables().Contains(key);

                if (exists)
                {
                    var value = Environment.GetEnvironmentVariable(key);

                    logger.LogTrace("Replace {match} with command line variable value '{value}' in {context} @char {pos}."
                        , match.Value
                        , value
                        , pathContext.Target
                        , match.Index);

                    if (string.IsNullOrWhiteSpace(value))
                    {
                        logger.LogWarning("Environment variable '{key}' written to {context} @char {pos} has a whitespace value."
                            + "{n}The variable is defined in template {template} @char {pos}."
                            , key
                            , pathContext.Target
                            , match.Index
                            , LoggingConstants.NewLine
                            , pathContext.Template
                            , match.Index);
                    }

                    return value;
                }

                logger.LogWarning("Environment variable {key} does not exist."
                    + "{n}Template '{match}' will not be replaced in {context} @char {pos}."
                    + "{n}The template is defined in {template} @char {pos}"
                    , key
                    , LoggingConstants.NewLine
                    , match.Value
                    , pathContext.Target
                    , match.Index
                    , LoggingConstants.NewLine
                    , pathContext.Template
                    , match.Index);

                // No key
                return match.Value;
            });
        }
    }
}
