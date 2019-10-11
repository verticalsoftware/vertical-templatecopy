// Copyright(c) 2018-2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Vertical.TemplateCopy.Abstractions;
using Xunit;

namespace Vertical.TemplateCopy.Macros
{
    public class MacroServicesTest
    {
        [Fact]
        public void AddMacros_Registers_Services()
        {
            var services = new ServiceCollection();

            services.AddMacros();

            services.Single(d => d.ServiceType == typeof(PathContextMacro));
            services.Single(d => d.ServiceType == typeof(IDictionary<string, IMacro>));
        }
    }
}