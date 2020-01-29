using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Vertical.Tools.TemplateCopy.Core;
using Vertical.Tools.TemplateCopy.Scripting;
using Xunit;

namespace Vertical.Tools.TemplateCopy.Tasks
{
    public class ProcessTemplatesTaskTests
    {
        [Fact]
        public void Execute_Produces_Expected_Output()
        {
            var fileSystem = new MockFileSystem();
            const string basePath = "/";

            LoadNetCoreAssemblies(fileSystem);
            LoadTemplate(fileSystem, basePath);

            var options = new Options
            {
                SourcePaths = { Path.Combine(basePath, "template/project") },
                TargetPath = "/output/project",
                ExtensionScriptPaths = { Path.Combine(basePath, "Props.cs" )},
                Properties = { ["project"]="Vertical.TemplateCopy" }
            };

            fileSystem.Directory.CreateDirectory(options.TargetPath);

            var services = Services.Create(options, svc => svc.AddSingleton<IFileSystem>(fileSystem));
            
            services.TaskAggregator.Run();
            
            using var resultArchive = new ZipArchive(new FileStream("Resources/output.zip", FileMode.Open));

            foreach (var entry in resultArchive.Entries)
            { 
                var sourcePath = Path.GetFullPath(Path.Combine(options.TargetPath, entry.FullName));
                
                if (entry.Length > 0)
                {
                    using var reader = new StreamReader(entry.Open());
                    var writtenContent = fileSystem.File.ReadAllText(sourcePath);
                    var expectedContent = reader.ReadToEnd();
                    writtenContent.ShouldBe(expectedContent, $"Content mismatch in {entry.FullName}");
                    continue;
                }

                fileSystem.Directory.Exists(sourcePath).ShouldBeTrue($"Could not validate path {sourcePath}");
            }
        }

        private static void LoadTemplate(IFileSystem fileSystem, string basePath)
        {
            using var archive = new ZipArchive(new FileStream("Resources/template.zip", FileMode.Open));
            foreach (var entry in archive.Entries)
            {
                if (entry.Length > 0)
                {
                    var path = Path.Combine(basePath, entry.FullName);
                    using var reader = new StreamReader(entry.Open());
                    fileSystem.Directory.CreateDirectory(Path.GetDirectoryName(path));
                    fileSystem.File.WriteAllText(path, reader.ReadToEnd());
                    continue;
                }

                fileSystem.Directory.CreateDirectory(Path.Combine(basePath, entry.FullName));
            }
        }

        private static void LoadNetCoreAssemblies(IFileSystem fileSystem)
        {
            var coreAssemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
            
            foreach (var assembly in CSharpCompiler.CoreAssemblies.Concat(CSharpCompiler.AncillaryAssemblies))
            {
                var path = Path.IsPathRooted(assembly)
                    ? assembly
                    : Path.Combine(coreAssemblyPath, assembly);

                var directory = Path.GetDirectoryName(path);

                fileSystem.Directory.CreateDirectory(directory);
                fileSystem.File.WriteAllBytes(path, File.ReadAllBytes(path));
            }
        }
    }
}