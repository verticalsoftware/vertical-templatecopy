using System.IO;
using Infrastructure;
using Moq;
using Shouldly;
using Xunit;

namespace Vertical.Tools.TemplateCopy.IO
{
    public class AssemblyResolverTests
    {
        private readonly Mock<IFileSystemAdapter> _fileSystemAdapterMock =
            new Mock<IFileSystemAdapter>();

        [Fact]
        public void GetAssemblyPath_Returns_Assembly_From_Short_Name()
        {
            _fileSystemAdapterMock
                .Setup(m => m.Validate("System.dll", false))
                .Returns(true);
            
            var subject = new AssemblyResolver(_fileSystemAdapterMock.Object, TestObjects.Logger);

            subject.GetAssemblyPath("System").ShouldBe("System.dll");
        }
        
        [Fact]
        public void GetAssemblyPath_Returns_Assembly_From_Short_Path()
        {
            _fileSystemAdapterMock
                .Setup(m => m.Validate("System.dll", false))
                .Returns(true);
            _fileSystemAdapterMock
                .Setup(m => m.GetFileExtension("System.dll"))
                .Returns(".dll");
            
            var subject = new AssemblyResolver(_fileSystemAdapterMock.Object, TestObjects.Logger);

            subject.GetAssemblyPath("System.dll").ShouldBe("System.dll");
        }
        
        [Fact]
        public void GetAssemblyPath_Returns_Assembly_From_Current_Directory_Path()
        {
            _fileSystemAdapterMock
                .Setup(m => m.Validate("System.dll", false))
                .Returns(true);
            _fileSystemAdapterMock
                .Setup(m => m.GetFileExtension("System.dll"))
                .Returns(".dll");
            _fileSystemAdapterMock.Setup(m => m.CurrentDirectory).Returns("/usr/bin");
            _fileSystemAdapterMock
                .Setup(m => m.CombinePaths(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((p1, p2) => $"{p1}/{p2}");
            _fileSystemAdapterMock
                .Setup(m => m.Validate("/usr/bin/System.dll", false))
                .Returns(true);
            
            var subject = new AssemblyResolver(_fileSystemAdapterMock.Object, TestObjects.Logger);

            subject.GetAssemblyPath("System.dll").ShouldBe("System.dll");
        }
        
        [Fact]
        public void GetAssemblyPath_Returns_Assembly_From_Core_Assembly_Path()
        {
            var netCoreLocation = Path.GetDirectoryName(typeof(object).Assembly.Location);
            var finalPath = $"{netCoreLocation}/System.dll";
            
            _fileSystemAdapterMock
                .Setup(m => m.GetFileExtension("System.dll"))
                .Returns(".dll");
            _fileSystemAdapterMock.Setup(m => m.CurrentDirectory).Returns("/usr/bin");
            _fileSystemAdapterMock
                .Setup(m => m.GetDirectoryName(It.IsAny<string>()))
                .Returns<string>(Path.GetDirectoryName);
            _fileSystemAdapterMock
                .Setup(m => m.CombinePaths(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((p1, p2) => $"{p1}/{p2}");
            _fileSystemAdapterMock
                .Setup(m => m.Validate(finalPath, false))
                .Returns(true);
            
            var subject = new AssemblyResolver(_fileSystemAdapterMock.Object, TestObjects.Logger);

            subject.GetAssemblyPath("System.dll").ShouldBe(finalPath);
        }

        [Fact]
        public void GetAssemblyPath_Throws_When_Not_Resolved()
        {
            var subject = new AssemblyResolver(_fileSystemAdapterMock.Object, TestObjects.Logger);
            Should.Throw<FileNotFoundException>(() => subject.GetAssemblyPath("Any.dll"));
        }
    }
}