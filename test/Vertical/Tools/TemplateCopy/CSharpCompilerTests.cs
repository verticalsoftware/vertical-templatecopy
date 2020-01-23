using System;
using System.Linq;
using System.Reflection;
using Moq;
using Shouldly;
using Xunit;

namespace Vertical.Tools.TemplateCopy
{
    public class CSharpCompilerTests
    {
        [Fact]
        public void Compile_Returns_Assembly_Bytes()
        {
            var options = new Options
            {
                AssemblyReferences = { typeof(System.Security.SecureString).Assembly.Location }
            };

            var subject = new CSharpCompiler(new OptionsProvider(options));
            
            var id = Guid.NewGuid().ToString();

            var code = @"
                using System;
                namespace _Test {
                    public class Props {
                        public string Id => ${id}; 
                    }                
                }                 
            ".Replace("${id}", "\"" + id + "\"");

            var assemblyBytes = subject.Compile(code);

            var assembly = Assembly.Load(assemblyBytes);

            var instance = Activator.CreateInstance(assembly.ExportedTypes.Single());

            var idProp = instance.GetType().GetProperties().Single();

            var idValue = idProp.GetValue(instance);
            
            idValue.ShouldBe(id);
        }
    }
}