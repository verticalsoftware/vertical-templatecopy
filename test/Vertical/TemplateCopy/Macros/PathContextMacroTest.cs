// Copyright(c) 2018-2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Moq;
using Shouldly;
using Vertical.TemplateCopy.Abstractions;
using Xunit;

namespace Vertical.TemplateCopy.Macros
{
    public class PathContextMacroTest
    {
        [Fact]
        public void ComputeValue_Returns_SelectorFunction_Result()
        {
            var context = new Mock<IPathContextAccessor>();
            context.SetupGet(m => m.Target).Returns("target");

            var testInstance = new PathContextMacro(context.Object, c => c.Target);
            testInstance.ComputeValue(null).ShouldBe(context.Object.Target);
        }
    }
}