using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Moq;
using Serilog;
using Shouldly;
using Vertical.TemplateCopy.Configuration;
using Vertical.TemplateCopy.Macros;
using Xunit;

namespace Vertical.TemplateCopy
{
    public partial class Master
    {
        [Fact]
        public void Run_The_Entire_Kit_N_Kaboodle_Returns_Excepted_Output()
        {
            const string solutionKey = "solution";
            const string solutionValue = "template-copy";
            const string solutionContent = "<solution content>";
            const string projectKey = "project";
            const string projectValue = "Vertical.TemplateCopy";
            const string projectContent = "<project content>";

            var options = new Options
            {
                TemplatePath = @"c:\template",
                OutputPath = @"c:\output",
                Variables =
                {
                    [solutionKey] = solutionValue,
                    [projectKey] = projectValue
                }
            };

            var fileSystem = new MockFileSystem();
            var templateRoot = fileSystem.Directory.CreateDirectory(options.TemplatePath).FullName;
            var slnFolder = fileSystem.Directory.CreateDirectory(Path.Combine(templateRoot, $"$({solutionKey})"))
                .FullName;
            var projectFolder = fileSystem.Directory.CreateDirectory(Path.Combine(slnFolder, $"$({projectKey})"))
                .FullName;
            var sourceFolder = fileSystem.Directory
                .CreateDirectory(Path.Combine(projectFolder, $"$(@expandDotToPath($({projectKey})))"))
                .FullName;
            fileSystem.Directory.CreateDirectory(options.OutputPath);
            fileSystem.File.WriteAllText(Path.Combine(slnFolder, $"$({solutionKey}).sln"), solutionContent);
            fileSystem.Directory.CreateDirectory(projectFolder);
            fileSystem.File.WriteAllText(Path.Combine(projectFolder, $"$({projectKey}).csproj"), projectContent);

            var macroFileContent =
                "date=$(@now(MM-yyyy)) "
                + "utcDate=$(@utcNow(MM-yyyy)) "
                + "currentDirectory=$(@currentDirectory()) "
                + "machineName=$(@machineName()) "
                + "systemDirectory=$(@systemDirectory()) "
                + "userDomainName=$(@userDomainName()) "
                + "userName=$(@userName()) "
                + "outputDirectory=$(@outputDirectory()) "
                + "templateDirectory=$(@templateDirectory()) "
                + "specialFolder=$(@specialFolder(ProgramFiles)) "
                + "targetContext=$(@targetContext()) "
                + "templateContext=$(@templateContext())";

            var settingsTemplatePath = Path.Combine(projectFolder, $"$({projectKey}).settings");
            fileSystem.File.WriteAllText(settingsTemplatePath, macroFileContent);

            fileSystem.File.WriteAllText(Path.Combine(sourceFolder, "Class1.cs"), $"namespace $({projectKey})");

            using (var services = new Services(options, new Mock<ILogger>().Object, fileSystem))
            {
                services.Runner.Run();
            }

            var outputRoot = Path.Combine(options.OutputPath, solutionValue);
            var outputProjectFolder = Path.Combine(outputRoot, projectValue);
            var outputSourceFolder = Path.Combine(outputProjectFolder, projectValue.Replace('.', Path.DirectorySeparatorChar));
            fileSystem.Directory.Exists(outputRoot).ShouldBeTrue();
            fileSystem.File.ReadAllText(Path.Combine(outputRoot, $"{solutionValue}.sln")).ShouldBe(solutionContent);
            fileSystem.Directory.Exists(outputProjectFolder).ShouldBeTrue();
            fileSystem.File.ReadAllText(Path.Combine(outputProjectFolder, $"{projectValue}.csproj")).ShouldBe(projectContent);
            fileSystem.Directory.Exists(outputSourceFolder).ShouldBeTrue();
            fileSystem.File.ReadAllText(Path.Combine(outputSourceFolder, "Class1.cs")).ShouldBe($"namespace {projectValue}");

            var outputMacroFileContent = fileSystem.File.ReadAllText(Path.Combine(outputProjectFolder
                , $"{projectValue}.settings"));

            var expectedMacroFileContent =
                $"date={SystemMacros.Now.ComputeValue("MM-yyyy")} "
                + $"utcDate={SystemMacros.UtcNow.ComputeValue("MM-yyyy")} "
                + $"currentDirectory={SystemMacros.CurrentDirectory.ComputeValue(null)} "
                + $"machineName={SystemMacros.MachineName.ComputeValue(null)} "
                + $"systemDirectory={SystemMacros.SystemDirectory.ComputeValue(null)} "
                + $"userDomainName={SystemMacros.UserDomainName.ComputeValue(null)} "
                + $"userName={SystemMacros.UserName.ComputeValue(null)} "
                + $"outputDirectory={options.OutputPath} "
                + $"templateDirectory={options.TemplatePath} "
                + $"specialFolder={(PathMacros.SpecialFolder.ComputeValue("ProgramFiles"))} "
                + $"targetContext={outputProjectFolder}\\{projectValue}.settings "
                + $"templateContext={settingsTemplatePath}";

            outputMacroFileContent.ShouldBe(expectedMacroFileContent);
        }
    }
}