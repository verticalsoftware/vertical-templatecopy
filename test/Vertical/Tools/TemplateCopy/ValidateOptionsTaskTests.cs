using Infrastructure;
using Moq;
using Serilog;
using Xunit;

namespace Vertical.Tools.TemplateCopy
{
    public class ValidateOptionsTaskTests
    {
        [Fact]
        public void Execute_Invokes_Validate()
        {
            var validatorMock = new Mock<IOptionsValidator>();
            var optionsProvider = new OptionsProvider(new Options());
            var subject = new ValidateOptionsTask(validatorMock.Object
                , optionsProvider
                , MockLogger.Default);

            validatorMock.Setup(m => m.Validate(It.IsAny<Options>())).Verifiable();

            subject.Execute();
            
            validatorMock.Verify(m => m.Validate(It.IsAny<Options>()));
        }
    }
}