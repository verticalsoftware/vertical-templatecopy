using Moq;
using Serilog;
using Shouldly;
using Xunit;

namespace Vertical.Tools.TemplateCopy
{
    public class OptionsSymbolStoreTests
    {
        [Fact]
        public void GetValueFunction_Returns_Function_For_Value()
        {
            var options = new Options { Properties = {["color"] = "blue" }};
            var subject = new OptionsSymbolStore(new OptionsProvider(options, new Mock<IFileSystem>().Object)
                , new Mock<ILogger>().Object);
            
            subject.GetValueFunction("color")().ShouldBe("blue");
        }

        [Fact]
        public void GetValueFunction_Returns_Null_For_Unmatched_Key()
        {
            var subject = new OptionsSymbolStore(new OptionsProvider(new Options(), new Mock<IFileSystem>().Object)
                , new Mock<ILogger>().Object);
            subject.GetValueFunction("none").ShouldBeNull();
        }
    }
}