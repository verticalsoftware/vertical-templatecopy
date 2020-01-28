using Infrastructure;
using Moq;
using Vertical.Tools.TemplateCopy.Providers;
using Xunit;

namespace Vertical.Tools.TemplateCopy.Tasks
{
    public class TaskAggregatorTests
    {
        [Fact]
        public void Run_Invokes_Sequence_Tasks()
        {
            var taskMock = new Mock<ISequenceTask>();
            var subject = new TaskAggregator(TestObjects.Logger
                , new Mock<IOptionsProvider>().Object
                , new[]{taskMock.Object}); 
                
            taskMock.Setup(m => m.Execute()).Verifiable();
            
            subject.Run();

            taskMock.Verify(m => m.Execute());
        }
    }
}