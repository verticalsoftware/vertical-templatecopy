using Shouldly;
using Xunit;

namespace Vertical.Tools.TemplateCopy
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