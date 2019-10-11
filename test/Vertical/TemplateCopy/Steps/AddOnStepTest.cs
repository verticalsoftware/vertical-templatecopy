// Copyright(c) 2018-2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Shouldly;
using Xunit;

namespace Vertical.TemplateCopy.Steps
{
    public class AddOnStepTest
    {
        [Fact]
        public void Run_Passes_State()
        {
            var testInstance = new AddOnStep<string>("name", "state", value =>
            {
                value.ShouldBe("state");
            });

            testInstance.Run();
        }
    }
}
