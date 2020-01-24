using Infrastructure;
using Xunit;

namespace Vertical.Tools.TemplateCopy
{
    public class ProcessTemplateTaskTests
    {
        [Fact]
        public void Execute_Creates_Expected_Output()
        {
            // Mostly end-to-end
            var options = new Options
            {
                
            };
            var services = TestServices.Create(options);
            var fileSystem = services.GetService<IFileSystemAdapter>();
        }
    }
}