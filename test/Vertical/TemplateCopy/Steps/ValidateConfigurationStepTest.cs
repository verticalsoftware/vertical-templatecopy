// Copyright(c) 2018-2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Vertical.TemplateCopy.Configuration;
using Xunit;

namespace Vertical.TemplateCopy.Steps
{
    public class ValidateConfigurationStepTest
    {
        private readonly ILogger<ValidateConfigurationStep> logger = new Mock<ILogger<ValidateConfigurationStep>>().Object;
        private readonly IFileSystem fileSystem = new MockFileSystem();

        [Fact]
        public void Run_Throws_For_Null_OutputPath()
        {
            fileSystem.Directory.CreateDirectory(@"c:\template");

            var options = new Options
            {
                TemplatePath = @"c:\template"
            };

            Should.Throw<AbortException>(() => new ValidateConfigurationStep(logger, fileSystem, options).Run());
        }

        [Fact]
        public void Run_Throws_For_Null_TemplatePath()
        {
            var options = new Options
            {
                OutputPath = @"c:\output"
            };

            Should.Throw<AbortException>(() => new ValidateConfigurationStep(logger, fileSystem, options).Run());
        }

        [Fact]
        public void Run_Throws_For_Invalid_TemplatePath()
        {
            var options = new Options
            {
                OutputPath = @"c:\output",
                TemplatePath = @"c:\template"
            };

            Should.Throw<AbortException>(() => new ValidateConfigurationStep(logger, fileSystem, options).Run());
        }

        [Fact]
        public void Run_Throws_For_Invalid_ArgumentMatchPattern()
        {
            fileSystem.Directory.CreateDirectory(@"c:\template");

            var options = new Options
            {
                OutputPath = @"c:\output",
                TemplatePath = @"c:\template",
                ArgumentVariableMatchPattern = "("
            };

            Should.Throw<AbortException>(() => new ValidateConfigurationStep(logger, fileSystem, options).Run());
        }

        [Fact]
        public void Run_Throws_For_Invalid_EnvironemntMatchPattern()
        {
            fileSystem.Directory.CreateDirectory(@"c:\template");

            var options = new Options
            {
                OutputPath = @"c:\output",
                TemplatePath = @"c:\template",
                EnvironmentVariableMatchPattern = "("
            };

            Should.Throw<AbortException>(() => new ValidateConfigurationStep(logger, fileSystem, options).Run());
        }

        [Fact]
        public void Run_Throws_For_Invalid_MacroMatchPattern()
        {
            fileSystem.Directory.CreateDirectory(@"c:\template");

            var options = new Options
            {
                OutputPath = @"c:\output",
                TemplatePath = @"c:\template",
                MacroMatchPattern = "("
            };

            Should.Throw<AbortException>(() => new ValidateConfigurationStep(logger, fileSystem, options).Run());
        }
    }
}