using System.Collections.Generic;
using Infrastructure;
using Moq;
using Shouldly;
using Vertical.Tools.TemplateCopy.Core;
using Vertical.Tools.TemplateCopy.Providers;
using Xunit;

namespace Vertical.Tools.TemplateCopy
{
    public class ContentResolverTests
    {
        private readonly IContentResolver _subject = new ContentResolver(TestObjects.Logger
            , new ISymbolStore[] {new OptionsSymbolStore(new OptionsProvider(new Options
            {
                Properties = {["Color"] = "blue"}
            })
                , TestObjects.Logger)}, new OptionsProvider(new Options()));
        
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

        [Fact]
        public void LoadSymbols_Invokes_SymbolStores()
        {
            var symbolStoreMock = new Mock<ISymbolStore>();
            var subject = new ContentResolver(TestObjects.Logger
                , new[]{symbolStoreMock.Object}
                , new Mock<IOptionsProvider>().Object);

            symbolStoreMock.Setup(m => m.Build()).Verifiable();
            
            subject.LoadSymbols();
            
            symbolStoreMock.Verify(m => m.Build());
        }
    }
}