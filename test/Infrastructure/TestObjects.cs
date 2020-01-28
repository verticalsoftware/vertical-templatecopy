using System.IO.Abstractions;
using Moq;
using Serilog;
using Vertical.Tools.TemplateCopy;
using Vertical.Tools.TemplateCopy.Core;
using Vertical.Tools.TemplateCopy.IO;
using Vertical.Tools.TemplateCopy.Providers;

namespace Infrastructure
{
    public static class TestObjects
    {
        public static ILogger Logger => new Mock<ILogger>().Object;
        
        public static IFileSystemAdapter FileSystemAdapter => new FileSystemAdapter(Logger
            , OptionsProvider, new FileSystem());
        
        public static IOptionsProvider OptionsProvider => new OptionsProvider(new Options());
    }
}