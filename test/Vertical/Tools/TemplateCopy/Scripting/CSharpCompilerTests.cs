using System;
using System.Linq;
using System.Reflection;
using Infrastructure;
using Shouldly;
using Vertical.Tools.TemplateCopy.Core;
using Vertical.Tools.TemplateCopy.IO;
using Vertical.Tools.TemplateCopy.Providers;
using Xunit;

namespace Vertical.Tools.TemplateCopy.Scripting
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

            var logger = TestObjects.Logger;
            var assemblyResolver = new AssemblyResolver(TestObjects.FileSystemAdapter, logger);
            var subject = new CSharpCompiler(new OptionsProvider(options)
                , logger
                , assemblyResolver
                , TestObjects.FileSystemAdapter);
            
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
            var logger = TestObjects.Logger;
            var assemblyResolver = new AssemblyResolver(TestObjects.FileSystemAdapter, logger);
            var subject = new CSharpCompiler(TestObjects.OptionsProvider
                , logger
                , assemblyResolver
                , TestObjects.FileSystemAdapter);

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