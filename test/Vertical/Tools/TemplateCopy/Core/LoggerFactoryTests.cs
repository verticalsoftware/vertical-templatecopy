using Shouldly;
using Xunit;

namespace Vertical.Tools.TemplateCopy.Core
{
    public class LoggerFactoryTests
    {
        [Fact]
        public void CreateLogger_Smoke()
        {
            LoggerFactory.CreateLogger(new Options()).ShouldNotBeNull();
        }
    }
}