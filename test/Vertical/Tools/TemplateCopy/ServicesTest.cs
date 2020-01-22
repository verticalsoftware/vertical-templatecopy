using Serilog.Events;
using Shouldly;
using Xunit;

namespace Vertical.Tools.TemplateCopy
{
    public class ServicesTest
    {
        [Fact]
        public void GetOrchestrator_Builds_Provider()
        {
            var options = new Options
            {
                Verbosity = LogEventLevel.Information,
                SourcePaths = { "/usr/templates/package" },
                TargetPath = "/usr/src",
                ContentFileExtensions = { ".cs", ".sln", ".csproj" }
            };

            var services = Services.Create(options);

            services.Orchestrator.ShouldNotBeNull();
        }
    }
}