using System.Collections.Generic;
using System.Linq;
using Serilog.Events;
using Shouldly;
using Vertical.Tools.TemplateCopy.Core;
using Xunit;

namespace Vertical.Tools.TemplateCopy.Providers
{
    public class OptionsProviderTests
    {
        private readonly IOptionsProvider _subject = new OptionsProvider(new Options
        {
            Properties = { ["color"] = "blue" },
            Verbosity = LogEventLevel.Warning,
            AssemblyReferences = { "/ref/some.dll" },
            OverwriteFiles = true,
            PlanOnly = true,
            SourcePaths = { "/usr/template" },
            SymbolPattern = "pattern",
            TargetPath = "/usr/target",
            ContentFileExtensions = { ".cs" },
            ExtensionScriptPaths = { "/usr/src" },
            WarnSymbolsMissing = true
        });

        [Fact]
        public void Properties_Returns_Expected() => _subject.Properties.Single().ShouldBe(new KeyValuePair<string, string>("color", "blue"));

        [Fact]
        public void AssemblyReferences_Returns_Expected() => _subject.AssemblyReferences.Single().ShouldBe("/ref/some.dll");
        
        [Fact]
        public void OverwriteFiles_Returns_Expected() => _subject.OverwriteFiles.ShouldBeTrue();

        [Fact]
        public void PlanOnly_Returns_Expected() => _subject.PlanOnly.ShouldBeTrue();

        [Fact]
        public void SourcePaths_Returns_Expected() => _subject.SourcePaths.Single().ShouldBe("/usr/template");

        [Fact]
        public void SymbolPatten_Returns_Expected() => _subject.SymbolPattern.ShouldBe("pattern");

        [Fact]
        public void TargetPath_Returns_Expected() => _subject.TargetPath.ShouldBe("/usr/target");

        [Fact]
        public void ContentFileExtensions_Returns_Expected() => _subject.ContentFileExtensions.Single().ShouldBe(".cs");

        [Fact]
        public void ExtensionScriptPaths_Returns_Expected() => _subject.ExtensionScriptPaths.Single().ShouldBe("/usr/src");

        [Fact]
        public void WarnSymbolsMissing_Returns_Expected() => _subject.WarnSymbolsMissing.ShouldBeTrue();
    }
}