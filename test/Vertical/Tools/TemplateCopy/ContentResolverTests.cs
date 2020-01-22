using System.Collections.Generic;
using Moq;
using Serilog;
using Shouldly;
using Xunit;

namespace Vertical.Tools.TemplateCopy
{
    public class ContentResolverTests
    {
        private readonly IContentResolver _subject = new ContentResolver(new Mock<ILogger>().Object
            , new ISymbolStore[] {new OptionsSymbolStore(new OptionsProvider(new Options
            {
                Properties = {["Color"] = "blue"}
            }, new Mock<IFileSystem>().Object)
                , new Mock<ILogger>().Object)}, new OptionsProvider(new Options(), new Mock<IFileSystem>().Object));
        
        [Theory, MemberData(nameof(Theories))]
        public void ReplaceSymbols_Returns_Expected_Values(string content, string expected)
        {
            _subject.ReplaceSymbols(content).ShouldBe(expected);
        }
        
        public static IEnumerable<object[]> Theories => new[]
        {
            new object[]{ "", "" },
            new object[]{ "I have no symbols", "I have no symbols" },
            new object[]{ "I have a ${non-matching} symbol", "I have a ${non-matching} symbol" },
            new object[]{ "I have one symbol, value=${Color}", "I have one symbol, value=blue" },
            new object[]{ "I have one symbol, value=${Color}, then other content", "I have one symbol, value=blue, then other content" },
            new object[]{ "I have two symbols, value=${Color} and ${Color}", "I have two symbols, value=blue and blue"}
        };
    }
}