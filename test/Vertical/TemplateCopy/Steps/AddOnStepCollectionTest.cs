// Copyright(c) 2018-2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Microsoft.Extensions.Logging;
using Moq;
using System;
using Shouldly;
using Vertical.TemplateCopy.Abstractions;
using Xunit;

namespace Vertical.TemplateCopy.Steps
{
    public class AddOnStepCollectionTest
    {
        private readonly AddOnStepCollection testInstance = new AddOnStepCollection(
            new Mock<ILogger<AddOnStepCollection>>().Object);

        [Fact]
        public void Class_Manages_FinalizeSteps()
        {
            var step = new Mock<IRunStep>();
            step.Setup(m => m.Run()).Verifiable();

            var otherStep = new Mock<IRunStep>();
            otherStep.Setup(m => m.Run()).Verifiable();

            testInstance.AddFinalizeStep(step.Object);
            testInstance.RunFinalizeSteps();

            step.Verify(m => m.Run(), Times.Once);
            otherStep.Verify(m => m.Run(), Times.Never);
        }

        [Fact]
        public void Class_Manages_RollbackSteps()
        {
            var stepMock = new Mock<IRunStep>();
            stepMock.Setup(m => m.Run()).Verifiable();

            var otherStepMock = new Mock<IRunStep>();
            otherStepMock.Setup(m => m.Run()).Verifiable();

            testInstance.AddRollbackStep(stepMock.Object);
            testInstance.RunRollbackSteps();

            stepMock.Verify(m => m.Run(), Times.Once);
            otherStepMock.Verify(m => m.Run(), Times.Never);
        }

        [Fact]
        public void Run_AddOnStep_Catches_Exceptions_Supresses_Throw()
        {
            var stepThatThrowsMock = new Mock<IRunStep>();
            stepThatThrowsMock.Setup(m => m.Run()).Throws<Exception>();

            testInstance.AddRollbackStep(stepThatThrowsMock.Object);

            Should.NotThrow(() => testInstance.RunRollbackSteps());
        }
    }
}