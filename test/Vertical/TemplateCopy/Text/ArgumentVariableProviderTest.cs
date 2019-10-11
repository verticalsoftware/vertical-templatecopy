using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Vertical.TemplateCopy.Abstractions;
using Vertical.TemplateCopy.Configuration;
using Xunit;

namespace Vertical.TemplateCopy.Text
{
    public class ArgumentVariableProviderTest
    {
        private static readonly ILogger<ArgumentVariableProvider> logger =
            new Mock<ILogger<ArgumentVariableProvider>>().Object;

        private static readonly Options options = new Options
        {
            Variables =
            {
                ["project"] = "vertical"
            }
        };

        private static readonly IPathContextAccessor pathContext =
            new Mock<IPathContextAccessor>().Object;

        [Fact]
        public void TransformContent_Replaces_Template()
        {
            var testInstance = new ArgumentVariableProvider(logger
                , options
                , new CommandLineVariables(options)
                , pathContext);

            testInstance.TransformContent("using $(project)")
                .ShouldBe("using vertical");
        }

        [Fact]
        public void TransformContent_Ignores_Non_Matching_Template()
        {
            var testInstance = new ArgumentVariableProvider(logger
                , options
                , new CommandLineVariables(options)
                , pathContext);

            testInstance.TransformContent("using $(project) $(solution)")
                .ShouldBe("using vertical $(solution)");
        }
    }
}