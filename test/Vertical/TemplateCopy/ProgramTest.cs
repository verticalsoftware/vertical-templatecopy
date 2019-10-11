using Microsoft.VisualStudio.TestPlatform.TestHost;
using Shouldly;
using Xunit;

namespace Vertical.TemplateCopy
{
    public class ProgramTest
    {
        [Fact]
        public void Main_No_Throw_For_Bad_Arg()
        {
            Should.NotThrow(() => Program.Main(new[] {"--x"}));
        }
    }
}