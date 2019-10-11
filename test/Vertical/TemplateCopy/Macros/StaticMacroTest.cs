// Copyright(c) 2018-2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Shouldly;
using Xunit;

namespace Vertical.TemplateCopy.Macros
{
    public class StaticMacroTest
    {
        [Fact]
        public void ComputeValue_Returns_Static_Value()
        {
            var testInstance = new StaticValueMacro("value");
            testInstance.ComputeValue(null).ShouldBe("value");
        }
    }
}