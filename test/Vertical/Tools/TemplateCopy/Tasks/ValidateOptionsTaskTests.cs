using Infrastructure;
using Moq;
using Vertical.Tools.TemplateCopy.Core;
using Vertical.Tools.TemplateCopy.Providers;
using Xunit;

namespace Vertical.Tools.TemplateCopy.Tasks
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
                , TestObjects.Logger);

            validatorMock.Setup(m => m.Validate(It.IsAny<Options>())).Verifiable();

            subject.Execute();
            
            validatorMock.Verify(m => m.Validate(It.IsAny<Options>()));
        }
    }
}