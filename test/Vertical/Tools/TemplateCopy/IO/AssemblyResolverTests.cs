using System.IO;
using Infrastructure;
using Moq;
using Shouldly;
using Xunit;

namespace Vertical.Tools.TemplateCopy.IO
{
    public class AssemblyResolverTests
    {
        [Fact]
        public void GetAssemblyPath_Returns_Assembly_From_Short_Name()
        {
            var fsAdapterMock = new Mock<IFileSystemAdapter>();
            fsAdapterMock
                .Setup(m => m.Validate("System.dll", false))
                .Returns(true);
            
            var subject = new AssemblyResolver(fsAdapterMock.Object, TestObjects.Logger);

            subject.GetAssemblyPath("System").ShouldBe("System.dll");
        }
        
        [Fact]
        public void GetAssemblyPath_Returns_Assembly_From_Short_Path()
        {
            var fsAdapterMock = new Mock<IFileSystemAdapter>();
            fsAdapterMock
                .Setup(m => m.Validate("System.dll", false))
                .Returns(true);
            fsAdapterMock
                .Setup(m => m.GetFileExtension("System.dll"))
                .Returns(".dll");
            
            var subject = new AssemblyResolver(fsAdapterMock.Object, TestObjects.Logger);

            subject.GetAssemblyPath("System.dll").ShouldBe("System.dll");
        }
        
        [Fact]
        public void GetAssemblyPath_Returns_Assembly_From_Current_Directory_Path()
        {
            var fsAdapterMock = new Mock<IFileSystemAdapter>();
            fsAdapterMock
                .Setup(m => m.Validate("System.dll", false))
                .Returns(true);
            fsAdapterMock
                .Setup(m => m.GetFileExtension("System.dll"))
                .Returns(".dll");
            fsAdapterMock.Setup(m => m.CurrentDirectory).Returns("/usr/bin");
            fsAdapterMock
                .Setup(m => m.CombinePaths(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((p1, p2) => $"{p1}/{p2}");
            fsAdapterMock
                .Setup(m => m.Validate("/usr/bin/System.dll", false))
                .Returns(true);
            
            var subject = new AssemblyResolver(fsAdapterMock.Object, TestObjects.Logger);

            subject.GetAssemblyPath("System.dll").ShouldBe("System.dll");
        }
        
        [Fact]
        public void GetAssemblyPath_Returns_Assembly_From_Core_Assembly_Path()
        {
            var fsAdapterMock = new Mock<IFileSystemAdapter>();
            var netCoreLocation = Path.GetDirectoryName(typeof(object).Assembly.Location);
            
            fsAdapterMock
                .Setup(m => m.Validate("System.dll", false))
                .Returns(true);
            fsAdapterMock
                .Setup(m => m.GetFileExtension("System.dll"))
                .Returns(".dll");
            fsAdapterMock.Setup(m => m.CurrentDirectory).Returns("/usr/bin");
            fsAdapterMock
                .Setup(m => m.CombinePaths(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((p1, p2) => $"{p1}/{p2}");
            fsAdapterMock
                .Setup(m => m.Validate("/usr/bin/System.dll", false))
                .Returns(true);
            
            var subject = new AssemblyResolver(fsAdapterMock.Object, TestObjects.Logger);

            subject.GetAssemblyPath("System.dll").ShouldBe("System.dll");
        }
    }
}