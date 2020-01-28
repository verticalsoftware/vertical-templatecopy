// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Serilog;
using Serilog.Events;
using Vertical.Tools.TemplateCopy.Core;

namespace Vertical.Tools.TemplateCopy.Scripting
{
    /// <inheritdoc />
    public class ExtensionTypeActivator : IExtensionTypeActivator
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public ExtensionTypeActivator(ILogger logger)
        {
            _logger = logger;
        }
        
        /// <inheritdoc />
        public object CreateInstance(Type type, IDictionary<Type, object> injectables)
        {
            using var _ = _logger.Indent(LogEventLevel.Verbose, "Creating type '{type}' instance", type);
            
            return type
                .GetConstructors()
                .Select(ctor => BuildInstance(ctor, injectables))
                .FirstOrDefault(instance => instance != null)
                ?? throw Exceptions.IncompatibleConstructor();
        }

        /// <summary>
        /// Matches parameters to the constructor info
        /// </summary>
        private object BuildInstance(ConstructorInfo constructor, IDictionary<Type, object> injectableLookup)
        {
            var parameters = constructor.GetParameters();
            var description = $"{constructor.DeclaringType}.ctor({string.Join(',', parameters.Select(p => p.GetType()))})";
            
            if (!parameters.Any()) return constructor.Invoke(Array.Empty<object>());

            var parameterValues = new List<object>(parameters.Length);

            foreach (var parameter in parameters)
            {
                if (!injectableLookup.TryGetValue(parameter.ParameterType, out var value))
                {
                    _logger.Verbose("Failed to map {constructor}: no match for requested parameter type {type}"
                        , description
                        , parameter.ParameterType);
                    return null;
                }

                parameterValues.Add(value);
            }

            _logger.Verbose($"Mapped {constructor}", description);
            
            return constructor.Invoke(parameterValues.ToArray());
        }
    }
}