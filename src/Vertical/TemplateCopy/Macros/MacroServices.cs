// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Vertical.TemplateCopy.Abstractions;
using Vertical.TemplateCopy.Configuration;

namespace Vertical.TemplateCopy.Macros
{
    public static class MacroServices
    {
        /// <summary>
        /// Adds macros to the service collection.
        /// </summary>
        /// <param name="services">Services collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddMacros(this IServiceCollection services) 
        {
            services.AddSingleton<PathContextMacro>();

            services.AddSingleton<IDictionary<string, IMacro>>(provider => new Dictionary<string, IMacro>
            {
                ["now"] = SystemMacros.Now,
                ["utcNow"] = SystemMacros.UtcNow,
                ["currentDirectory"] = SystemMacros.CurrentDirectory,
                ["machineName"] = SystemMacros.MachineName,
                ["systemDirectory"] = SystemMacros.SystemDirectory,
                ["userDomainName"] = SystemMacros.UserDomainName,
                ["userName"] = SystemMacros.UserName,

                ["outputDirectory"] = new StaticValueMacro(provider.GetService<Options>().OutputPath),
                ["templateDirectory"] = new StaticValueMacro(provider.GetService<Options>().TemplatePath),

                ["specialFolder"] = PathMacros.SpecialFolder,
                ["expandDotToPath"] = PathMacros.ExpandDotToPath,
                ["expandToPath"] = PathMacros.ExpandToPath,

                ["targetContext"] = new PathContextMacro(provider.GetService<IPathContextAccessor>()
                    , context => context.Target),
                ["templateContext"] = new PathContextMacro(provider.GetService<IPathContextAccessor>()
                    , context => context.Template)
            }) ;

            return services;
        }
    }
}
