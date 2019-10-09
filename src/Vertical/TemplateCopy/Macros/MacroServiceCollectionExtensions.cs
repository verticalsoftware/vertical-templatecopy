using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Vertical.TemplateCopy.Macros
{
    public static class MacroServiceCollectionExtensions
    {
        public static IServiceCollection AddMacros(this IServiceCollection services) 
        {
            services.AddSingleton<IDictionary<string, IMacro>>(provider => new Dictionary<string, IMacro>
            {
                ["now"] = SystemMacros.Now,
                ["specialFolder"] = PathMacros.SpecialFolder,
                ["expandDotToPath"] = PathMacros.ExpandDotToPath
            });

            return services;
        }
    }
}
