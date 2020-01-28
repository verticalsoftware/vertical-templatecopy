using System;
using Infrastructure;
using Moq;
using Shouldly;
using Vertical.Tools.TemplateCopy.Core;
using Vertical.Tools.TemplateCopy.IO;
using Vertical.Tools.TemplateCopy.Scripting;
using Xunit;

namespace Vertical.Tools.TemplateCopy.Providers
{
    public class ExtensionSymbolStoreTests
    {
        [Fact]
        public void GetValueFunction_Returns_Expected_Value()
        {
            var code = string.Join(Environment.NewLine, 
                "using System;", 
                "using System.Collections.Generic;", 
                "namespace T4Copy", 
                "{", 
                "    public class MyProps", 
                "    {", 
                "        public MyProps(IDictionary<string,string> properties)", 
                "        {", 
                "            Color = properties[\"Color\"];", 
                "            PropertyCount = properties.Count.ToString();", 
                "        }", 
                "        public string Color { get; }", 
                "        public string PropertyCount { get; }", 
                "    }", 
                "}");

            var fileSystemMock = new Mock<IFileSystemAdapter>();
            fileSystemMock.Setup(m => m.ReadFile("/src/script.cs")).Returns(code);
            fileSystemMock.Setup(m => m.ResolvePath(It.IsAny<string>())).Returns<string>(p => p);

            var options = new Options
            {
                ExtensionScriptPaths = {"/src/script.cs"},
                Properties = { ["Color"] = "blue" }
            };
            
            var optionsProvider = new OptionsProvider(options);
            var assemblyResolver = new AssemblyResolver(TestObjects.FileSystemAdapter, TestObjects.Logger);
            var compiler = new CSharpCompiler(optionsProvider
                , TestObjects.Logger
                , assemblyResolver
                , TestObjects.FileSystemAdapter);

            var subject = new ExtensionScriptSymbolStore(compiler
                , TestObjects.Logger
                , optionsProvider
                , fileSystemMock.Object
                , new ExtensionTypeActivator(TestObjects.Logger));
            
            subject.Build();
            
            subject.GetValueFunction("Color")().ShouldBe("blue");
            subject.GetValueFunction("PropertyCount")().ShouldBe("8");
        }
    }
}