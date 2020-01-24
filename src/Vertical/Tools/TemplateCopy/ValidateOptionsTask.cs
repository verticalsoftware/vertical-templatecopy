// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Serilog;

namespace Vertical.Tools.TemplateCopy
{
    /// <summary>
    /// Validate options task.
    /// </summary>
    public class ValidateOptionsTask : ISequenceTask
    {
        private readonly IOptionsValidator _optionsValidator;
        private readonly IOptionsProvider _options;
        private readonly ILogger _logger;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="optionsValidator">Options validator</param>
        /// <param name="options">Options provider</param>
        /// <param name="logger">Logger</param>
        public ValidateOptionsTask(IOptionsValidator optionsValidator
            , IOptionsProvider options
            , ILogger logger)
        {
            _optionsValidator = optionsValidator;
            _options = options;
            _logger = logger;
        }
        
        /// <inheritdoc />
        public void Execute()
        {
            _optionsValidator.Validate(_options.Values);
            
            _logger.LogOptions(_options.Values);
        }
    }
}