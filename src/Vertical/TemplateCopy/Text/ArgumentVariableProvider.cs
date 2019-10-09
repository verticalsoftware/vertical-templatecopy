using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using Vertical.TemplateCopy.Configuration;

namespace Vertical.TemplateCopy.Text
{
    public class ArgumentVariableProvider : ITextTransformProvider
    {
        private readonly ILogger<ArgumentVariableProvider> logger;
        private readonly Options options;
        private readonly CommandLineVariables variables;

        public ArgumentVariableProvider(ILogger<ArgumentVariableProvider> logger
            , Options options
            , CommandLineVariables variables)
        {
            this.logger = logger;
            this.options = options;
            this.variables = variables;
        }

        public string TransformContent(string source)
        {
            return Regex.Replace(source, options.ArgumentVariableMatchPattern, match =>
            {
                var key = match.Groups["key"].Value;
                if (variables.TryGetValue(key, out var value))
                {
                    logger.LogTrace("Replace {match} with command line variable value '{value}'"
                        , match.Value
                        , value);

                    return value;
                }

                // Not a match, return original string
                return match.Value;
            });
        }
    }
}
