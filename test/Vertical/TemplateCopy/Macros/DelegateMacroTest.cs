// Copyright(c) 2018-2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Shouldly;
using Xunit;

namespace Vertical.TemplateCopy.Macros
{
    public class DelegateMacroTest
    {
        [Fact]
        public void ComputeValue_Returns_Function_Result()
        {
            var testInstance = new DelegateMacro(v => v);
            testInstance.ComputeValue("test").ShouldBe("test");
        }
    }
}
