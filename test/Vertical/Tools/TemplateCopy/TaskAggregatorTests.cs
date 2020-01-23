using Infrastructure;
using Moq;
using Xunit;

namespace Vertical.Tools.TemplateCopy
{
    public class TaskAggregatorTests
    {
        [Fact]
        public void Run_Invokes_Sequence_Tasks()
        {
            var taskMock = new Mock<ISequenceTask>();
            var subject = new TaskAggregator(MockLogger.Default
                , new Mock<IOptionsProvider>().Object
                , new[]{taskMock.Object}); 
                
            taskMock.Setup(m => m.Execute()).Verifiable();
            
            subject.Run();

            taskMock.Verify(m => m.Execute());
        }
    }
}