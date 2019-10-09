using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Vertical.TemplateCopy.Configuration;
using Vertical.TemplateCopy.Macros;

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

        public MacroVariableProvider(ILogger<MacroVariableProvider> logger
            , Options options
            , IDictionary<string, IMacro> macroDictionary)
        {
            this.logger = logger;
            this.options = options;
            this.macroDictionary = macroDictionary;
        }

        public string TransformContent(string source)
        {
            return Regex.Replace(source, options.MacroMatchPattern, match =>
            {
                var fn = match.Groups["fn"].Value;

                if (string.IsNullOrWhiteSpace(fn))
                    return match.Value;

                if (!macroDictionary.TryGetValue(fn, out var macro))
                    return match.Value;

                var arg = match.Groups["arg"].Value;
                var value = macro.ComputeValue(arg);

                logger.LogTrace("Replace {match} with macro value '{value}'"
                    , match.Value
                    , value);

                return value;
            });
        }
    }
}
