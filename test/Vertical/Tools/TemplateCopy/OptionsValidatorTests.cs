using System;
using System.IO;
using Moq;
using Serilog;
using Shouldly;
using Xunit;

namespace Vertical.Tools.TemplateCopy
{
    public class OptionsValidatorTests
    {
        private readonly ILogger _logger = new Mock<ILogger>().Object;

        [Fact]
        public void Validate_Throws_For_Missing_Source_Paths()
        {
            var subject = new OptionsValidator(_logger, new Mock<IFileSystem>().Object);
            Should.Throw<ApplicationException>(() => subject.Validate(new Options()));
        }

        [Fact]
        public void Validate_Throws_For_Invalid_Source_Path()
        {
            var path = Path.GetFullPath("/usr/template");
            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(m => m.Validate(path)).Throws<ApplicationException>();
            fileSystemMock.Setup(m => m.ResolvePath(It.IsAny<string>())).Returns<string>(Path.GetFullPath);
            
            var options = new Options {SourcePaths = {path}};
            var subject = new OptionsValidator(_logger, fileSystemMock.Object);

            Should.Throw<ApplicationException>(() => subject.Validate(options));
        }
        
        [Fact]
        public void Validate_Throws_For_Invalid_Extension_Script_Path()
        {
            var path = Path.GetFullPath("/usr/extension.txt");
            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(m => m.Validate(path)).Throws<ApplicationException>();
            fileSystemMock.Setup(m => m.ResolvePath(It.IsAny<string>())).Returns<string>(Path.GetFullPath);

            var options = new Options
            {
                SourcePaths = {"/usr/templates"},
                ExtensionScriptPaths = { path }
            };
            var subject = new OptionsValidator(_logger, fileSystemMock.Object);

            Should.Throw<ApplicationException>(() => subject.Validate(options));
        }
        
        [Fact]
        public void Validate_Throws_For_Invalid_Assembly_Reference_Path()
        {
            var path = Path.GetFullPath("/usr/bin/core.dll");
            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(m => m.Validate(path)).Throws<ApplicationException>();
            fileSystemMock.Setup(m => m.ResolvePath(It.IsAny<string>())).Returns<string>(Path.GetFullPath);

            var options = new Options
            {
                SourcePaths = {"/usr/templates"},
                AssemblyReferences = { path }
            };
            var subject = new OptionsValidator(_logger, fileSystemMock.Object);

            Should.Throw<ApplicationException>(() => subject.Validate(options));
        }

        [Fact]
        public void Validate_Does_Not_Throw_With_Valid_Options()
        {
            var fileSystemMock = new Mock<IFileSystem>();
            var options = new Options {SourcePaths = {"/usr/templates"}};
            var subject = new OptionsValidator(_logger, fileSystemMock.Object);

            subject.Validate(options);
        }
    }
}