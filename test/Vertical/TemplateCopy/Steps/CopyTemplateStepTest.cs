// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Vertical.TemplateCopy.Abstractions;
using Vertical.TemplateCopy.Configuration;
using Xunit;

namespace Vertical.TemplateCopy.Steps
{
    public class CopyTemplateStepTest
    {
        private static readonly Dictionary<string, MockFileData> files =
            new Dictionary<string, MockFileData>
        {
            [@"c:\template\project.csproj"] = new MockFileData("<project data>"),
            [@"c:\template\src\Vertical\TemplateCopy\Class1.cs"] = new MockFileData("public class Class1"),
            [@"c:\template\src\Vertical\TemplateCopy\Class2.cs"] = new MockFileData("public class Class2")
        };

        private static readonly string[] defaultFiles = new[]
        {
            @"c:\output\project.csproj",
            @"c:\output\src\Vertical\TemplateCopy\Class1.cs",
            @"c:\output\src\Vertical\TemplateCopy\Class2.cs"
        };

        private readonly Options options = new Options
        {
            OutputPath = @"c:\output",
            TemplatePath = @"c:\template"
        };

        private readonly IFileSystem fileSystem = new MockFileSystem(files);
        private readonly Mock<IPathContext> pathContextMock = new Mock<IPathContext>();
        private readonly Mock<IAddOnSteps> addOnStepsMock = new Mock<IAddOnSteps>();

        [Fact]
        public void Run_Creates_Artifacts()
        {
            var testInstance = new CopyTemplateStep(new Mock<ILogger<CopyTemplateStep>>().Object
                , options
                , fileSystem
                , TestMocks.RedundentTextTransformMock.Object
                , pathContextMock.Object
                , addOnStepsMock.Object);

            testInstance.Run();

            defaultFiles
                .All(path => fileSystem.File.Exists(path))
                .ShouldBeTrue();
        }

        [Fact]
        public void Run_Adds_RollbackSteps()
        {
            var steps = new List<IRunStep>();

            addOnStepsMock.Setup(m => m.AddRollbackStep(It.IsAny<IRunStep>())).Callback<IRunStep>(
                steps.Add);

            var testInstance = new CopyTemplateStep(new Mock<ILogger<CopyTemplateStep>>().Object
                , options
                , fileSystem
                , TestMocks.RedundentTextTransformMock.Object
                , pathContextMock.Object
                , addOnStepsMock.Object);

            testInstance.Run();

            // Simulate rollback
            foreach(var step in steps) { step.Run(); }

            defaultFiles
                .All(path => !fileSystem.File.Exists(path))
                .ShouldBeTrue();
        }

        [Theory, MemberData(nameof(TransformTheories))]
        public void Run_Invokes_Transforms_ForContent_And_ObjectNames(string content, string source, string dest)
        {
            var transformMock = new Mock<ITextTransform>();
            transformMock.Setup(m => m.TransformContent(content, source, dest)).Verifiable();
            transformMock.Setup(m => m.TransformContent(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string, string>((s, _, __) => s);

            var testInstance = new CopyTemplateStep(new Mock<ILogger<CopyTemplateStep>>().Object
                , options
                , fileSystem
                , transformMock.Object
                , pathContextMock.Object
                , addOnStepsMock.Object);

            testInstance.Run();

            transformMock.Verify(m => m.TransformContent(content, source, dest), Times.Once);
        }

        public static IEnumerable<object[]> TransformTheories => new[]
        {
            new[] { @"project.csproj", @"c:\template\project.csproj", @"resolveFileName:project.csproj" },
            new[] { @"<project data>", @"c:\template\project.csproj", @"c:\output\project.csproj" },
            new[] { @"src", @"c:\template\src", @"resolveDirectoryName:src" },
            new[] { @"Vertical", @"c:\template\src\Vertical", @"resolveDirectoryName:Vertical" },
            new[] { @"TemplateCopy", @"c:\template\src\Vertical\TemplateCopy", @"resolveDirectoryName:TemplateCopy" },
            new[] { @"Class1.cs", @"c:\template\src\Vertical\TemplateCopy\Class1.cs", @"resolveFileName:Class1.cs" },
            new[] { @"public class Class1", @"c:\template\src\Vertical\TemplateCopy\Class1.cs", @"c:\output\src\Vertical\TemplateCopy\Class1.cs" },
            new[] { @"Class2.cs", @"c:\template\src\Vertical\TemplateCopy\Class2.cs", @"resolveFileName:Class2.cs" },
            new[] { @"public class Class2", @"c:\template\src\Vertical\TemplateCopy\Class2.cs", @"c:\output\src\Vertical\TemplateCopy\Class2.cs" }
        };

        [Fact]
        public void Run_Throws_For_Duplicate_Object_With_No_Overwrite()
        {
            fileSystem.Directory.CreateDirectory(@"c:\output\src\Vertical\TemplateCopy");
            fileSystem.File.WriteAllText(@"c:\output\src\Vertical\TemplateCopy\Class1.cs", "data");

            var testInstance = new CopyTemplateStep(new Mock<ILogger<CopyTemplateStep>>().Object
                , options
                , fileSystem
                , TestMocks.RedundentTextTransformMock.Object
                , pathContextMock.Object
                , addOnStepsMock.Object);

            Should.Throw<AbortException>(() => testInstance.Run());
        }
    }
}