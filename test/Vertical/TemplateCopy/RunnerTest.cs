// Copyright(c) 2018-2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Helpers;
using Moq;
using System;
using System.Collections.Generic;
using Vertical.TemplateCopy.Abstractions;
using Xunit;

namespace Vertical.TemplateCopy
{
    public class RunnerTest
    {
        [Fact]
        public void Run_Invokes_Steps()
        {
            var stepMock = new Mock<IRunStep>();
            stepMock.Setup(m => m.Run()).Verifiable();

            var testInstance = new Runner(new []{stepMock.Object}
                , TestMocks.AddOnStepsRunner.Object
                , TestMocks.NopLogger<Runner>.Default);

            testInstance.Run();

            stepMock.Verify(m => m.Run());
        }

        [Theory, MemberData(nameof(Run_RollbackStep_Theories))]
        public void Run_Runs_RollbackSteps_On_Exceptions(Exception exception)
        {
            var stepMock = new Mock<IRunStep>();
            stepMock.Setup(m => m.Run()).Throws(exception);

            var addOnStepsMock = TestMocks.AddOnStepsRunner;
            addOnStepsMock.Setup(m => m.RunRollbackSteps()).Verifiable();

            var testInstance = new Runner(new []{stepMock.Object}
                , addOnStepsMock.Object
                , TestMocks.NopLogger<Runner>.Default);

            testInstance.Run();

            addOnStepsMock.Verify(m => m.RunRollbackSteps());
        }

        public static IEnumerable<object[]> Run_RollbackStep_Theories => new[]
        {
            new object[] {new Exception()},
            new object[] {new AbortException()},
        };
    }
}