using System.Linq;
using Serilog.Events;
using Shouldly;
using Vertical.CommandLine;
using Xunit;

namespace Vertical.Tools.TemplateCopy
{
    public class OptionsConfigurationTests
    {
        [Fact]
        public void Configuration_Maps_Parameters()
        {
            var args = new[]
            {
                "-s", "/usr/sources",
                "-s", "/usr/templates",
                "-t", "/usr/lib",
                "-c", ".csproj;.cs;.sln",
                "-v", "debug",
                "--script", "/usr/scripts/source.cs",
                "-p", "color=blue",
                "-p", "count=6",
                "--asm", "/usr/bin/netcore/core.dll",
                "--plan",
                "--overwrite"
            };
                
            var config = new OptionsConfiguration(_ => { });
            var options = CommandLineApplication.ParseArguments<Options>(config, args);
            
            options.SourcePaths.Contains("/usr/sources").ShouldBeTrue();
            options.SourcePaths.Contains("/usr/templates").ShouldBeTrue();
            options.TargetPath.ShouldBe("/usr/lib");
            options.ContentFileExtensions.Contains(".csproj").ShouldBeTrue();
            options.ContentFileExtensions.Contains(".cs").ShouldBeTrue();
            options.ContentFileExtensions.Contains(".sln").ShouldBeTrue();
            options.Verbosity.ShouldBe(LogEventLevel.Debug);
            options.ExtensionScriptPaths.Single().ShouldBe("/usr/scripts/source.cs");
            options.Properties["color"].ShouldBe("blue");
            options.Properties["count"].ShouldBe("6");
            options.AssemblyReferences.Single().ShouldBe("/usr/bin/netcore/core.dll");
            options.PlanOnly.ShouldBeTrue();
            options.OverwriteFiles.ShouldBeTrue();
        }
    }
}