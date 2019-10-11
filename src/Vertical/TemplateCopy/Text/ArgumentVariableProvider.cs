// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using Vertical.TemplateCopy.Abstractions;
using Vertical.TemplateCopy.Configuration;
using Vertical.TemplateCopy.Utilities;

namespace Vertical.TemplateCopy.Text
{
    /// <summary>
    /// Replaces template placeholders with argument variable values.
    /// </summary>
    public class ArgumentVariableProvider : ITextTransformProvider
    {
        private readonly ILogger logger;
        private readonly Options options;
        private readonly CommandLineVariables variables;
        private readonly IPathContextAccessor pathContext;

        public ArgumentVariableProvider(ILogger<ArgumentVariableProvider> logger
            , Options options
            , CommandLineVariables variables
            , IPathContextAccessor pathContext)
        {
            this.logger = logger;
            this.options = options;
            this.variables = variables;
            this.pathContext = pathContext;
        }

        /// <summary>
        /// Transforms the content using provided values.
        /// </summary>
        public string TransformContent(string source)
        {
            return Regex.Replace(source, options.ArgumentVariableMatchPattern, match =>
            {
                var key = match.Groups["key"].Value;

                if (variables.TryGetValue(key, out var value))
                {
                    logger.LogTrace("Replace template {match} with argument variable value '{value}' in {context} @char {pos}"
                        , match.Value
                        , value
                        , pathContext.Target
                        , match.Index);

                    return value;
                }

                logger.LogWarning("Argument template pattern {match} defined in {context} @char {pos} not matched to any variable keys."
                    + "{n}The value will not be replaced in {target}."
                    , match.Value
                    , pathContext.Template
                    , match.Index
                    , LoggingConstants.NewLine
                    , pathContext.Target);

                // Not a match, return original string
                return match.Value;
            });
        }
    }
}
