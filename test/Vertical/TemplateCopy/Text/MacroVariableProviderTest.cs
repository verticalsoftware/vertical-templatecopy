// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using Helpers;
using Moq;
using Shouldly;
using Vertical.TemplateCopy.Abstractions;
using Vertical.TemplateCopy.Configuration;
using Xunit;

namespace Vertical.TemplateCopy.Text
{
    public class MacroVariableProviderTest
    {
        private static readonly IDictionary<string, IMacro> macroDictionary = new Dictionary<string, IMacro>
        {
            ["function"] = new Func<IMacro>(() =>
            {
                var mock = new Mock<IMacro>();
                mock.Setup(m => m.ComputeValue(It.IsAny<string>())).Returns<string>(s => s);
                return mock.Object;
            })()
        };

        private readonly MacroVariableProvider testInstance = new MacroVariableProvider(
            TestMocks.NopLogger<MacroVariableProvider>.Default
            , new Options()
            , macroDictionary
            , TestMocks.PathContextAccessor.Object);

        [Fact]
        public void TransformContent_Returns_Empty_String_For_No_Argument()
        {
            testInstance.TransformContent("$(@function())").ShouldBe(string.Empty);
        }

        [Fact]
        public void TransformContent_Returns_Arg_Value()
        {
            testInstance.TransformContent("$(@function(arg))").ShouldBe("arg");
        }

        [Fact]
        public void TransformContent_Ignores_Function_Not_In_Macro_Dictionary()
        {
            testInstance.TransformContent("$(@function(arg)) $(@notafunction())").ShouldBe("arg $(@notafunction())");
        }
    }
}