// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Vertical.TemplateCopy.Abstractions;
using Vertical.TemplateCopy.Configuration;
using Vertical.TemplateCopy.Utilities;

namespace Vertical.TemplateCopy.Text
{
    /// <summary>
    /// Provider for macro
    /// </summary>
    public class MacroVariableProvider : ITextTransformProvider
    {
        private readonly ILogger<MacroVariableProvider> logger;
        private readonly Options options;
        private readonly IDictionary<string, IMacro> macroDictionary;
        private readonly IPathContextAccessor pathContext;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="options">User options</param>
        /// <param name="macroDictionary">Dictionary where the keys are macro names</param>
        /// <param name="pathContext">Path context access</param>
        public MacroVariableProvider(ILogger<MacroVariableProvider> logger
            , Options options
            , IDictionary<string, IMacro> macroDictionary
            , IPathContextAccessor pathContext)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.macroDictionary = macroDictionary ?? throw new ArgumentNullException(nameof(macroDictionary));
            this.pathContext = pathContext ?? throw new ArgumentNullException(nameof(pathContext));
        }

        public string TransformContent(string source)
        {
            return Regex.Replace(source, options.MacroMatchPattern, match =>
            {
                var fn = match.Groups["fn"].Value;

                if (string.IsNullOrWhiteSpace(fn))
                    return match.Value;

                if (!macroDictionary.TryGetValue(fn, out var macro))
                {
                    logger.LogWarning("Macro pattern {match} not matched to existing function in {context} @char {pos}."
                        + "{n}The macro was defined in template {template} @char {pos}"
                        , new
                        {
                            match = match.Value,
                            context = pathContext.Target,
                            pos = match.Index,
                            template = pathContext.Template,
                            LoggingConstants.NewLine
                        });
                    return match.Value;
                }

                var arg = match.Groups["arg"].Value;
                var value = CompareMacroValue(macro, arg, match.Index);

                logger.LogTrace("Replace {match} with macro value '{value}' in {context} @char {pos}"
                    , match.Value
                    , value
                    , pathContext.Target
                    , match.Index);

                return value;
            });
        }

        private string CompareMacroValue(IMacro macro, string arg, int pos)
        {
            try
            {
                return macro.ComputeValue(arg);
            }
            catch (Exception ex)
            {
                var message = ex.Message + "{n}The macro was defined in template {template} @char {pos}";
                throw logger.LogErrorWithAbort(message, LoggingConstants.NewLine, pos);
            }
        }
    }
}
