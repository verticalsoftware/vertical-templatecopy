using Moq;
using Xunit;

namespace Vertical.Tools.TemplateCopy
{
    public class LoadSymbolsTaskTests
    {
        [Fact]
        public void Execute_Invokes_Build_On_All_Symbol_Stores()
        {
            var storeMock = new Mock<ISymbolStore>();
            storeMock.Setup(m => m.Build()).Verifiable();
            
            var subject = new LoadSymbolsTask(new ISymbolStore[]{storeMock.Object});
            
            subject.Execute();
            
            storeMock.Verify(m => m.Build());
        }
    }
}