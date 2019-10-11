// Copyright(c) 2018-2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Shouldly;
using Xunit;

namespace Vertical.TemplateCopy.Context
{
    public class PathContextTest
    {
        private readonly PathContext testInstance = new PathContext();

        [Fact]
        public void AddEntry_Pushes_To_Stack()
        {
            using (testInstance.BeginPathContext("template", "target"))
            {
                testInstance.Target.ShouldBe("target");
                testInstance.Template.ShouldBe("template");
            }
        }

        [Fact]
        public void Dispose_Removes_Context()
        {
            using (testInstance.BeginPathContext("template", "target"))
            {
            }

            testInstance.Target.ShouldBeNull();
            testInstance.Template.ShouldBeNull();
        }
    }
}
