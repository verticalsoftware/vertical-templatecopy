using Microsoft.Extensions.DependencyInjection;
using Vertical.Tools.TemplateCopy;

namespace Infrastructure
{
    public static class TestServices
    {
        public static Services Create(Options options)
        {
            return Services.Create(options, services =>
                {
                    services.AddSingleton<IFileSystemAdapter, TestFileSystemAdapter>();
                });
        }        
    }
}