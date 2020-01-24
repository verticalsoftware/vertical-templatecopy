using System;
using System.Linq;
using System.Reflection;
using Infrastructure;
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

            var logger = MockLogger.Default;
            var assemblyResolver = new AssemblyResolver(new FileSystemAdapter(MockLogger.Default
                , new Mock<IOptionsProvider>().Object), logger);
            var subject = new CSharpCompiler(new OptionsProvider(options)
                , logger
                , assemblyResolver);
            
            var id = Guid.NewGuid().ToString();

            var code = $@"
                using System;
                using System.Collections.Generic;
                using System.Linq;
                namespace _Test {{
                    public class Props 
                    {{
                        public string Id => ${{id}};
                    }}                
                }}                 
            ".Replace("${id}", "\"" + id + "\"");

            var assemblyBytes = subject.Compile(code);

            var assembly = Assembly.Load(assemblyBytes);

            var instance = Activator.CreateInstance(assembly.ExportedTypes.Single());

            var idProp = instance.GetType().GetProperties().Single();

            var idValue = idProp.GetValue(instance);
            
            idValue.ShouldBe(id);
        }

        [Fact]
        public void Compile_With_Errors_Throws_AggregateException()
        {
            var logger = MockLogger.Default;
            var assemblyResolver = new AssemblyResolver(new FileSystemAdapter(MockLogger.Default
                , new Mock<IOptionsProvider>().Object), logger);
            var subject = new CSharpCompiler(new Mock<IOptionsProvider>().Object
                , logger
                , assemblyResolver);

            const string code = @"
                using System;
                using System.Collections.Generic;
                using System.Linq;
                namespace _Test {
                    public class Props 
                    {
                        public string IllegalPropertyName! {get;}
                    }                
                }              
            ";

            Should.Throw<AggregateException>(() => subject.Compile(code));
        }
    }
}