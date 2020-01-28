using Infrastructure;
using Shouldly;
using Vertical.Tools.TemplateCopy.Core;
using Xunit;

namespace Vertical.Tools.TemplateCopy.Providers
{
    public class OptionsSymbolStoreTests
    {
        [Fact]
        public void GetValueFunction_Returns_Function_For_Value()
        {
            var options = new Options { Properties = {["color"] = "blue" }};
            var subject = new OptionsSymbolStore(new OptionsProvider(options)
                , TestObjects.Logger);
            
            subject.GetValueFunction("color")().ShouldBe("blue");
        }

        [Fact]
        public void GetValueFunction_Returns_Null_For_Unmatched_Key()
        {
            var subject = new OptionsSymbolStore(new OptionsProvider(new Options())
                , TestObjects.Logger);
            subject.GetValueFunction("none").ShouldBeNull();
        }
    }
}