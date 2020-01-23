using Moq;
using Serilog;

namespace Infrastructure
{
    public static class MockLogger
    {
        public static readonly ILogger Default = new Mock<ILogger>().Object;
    }
}