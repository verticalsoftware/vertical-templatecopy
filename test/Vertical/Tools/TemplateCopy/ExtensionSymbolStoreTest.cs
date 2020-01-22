using System;
using Moq;
using Serilog;
using Shouldly;
using Xunit;

namespace Vertical.Tools.TemplateCopy
{
    public class ExtensionSymbolStoreTest
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

            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(m => m.ReadFile("/src/script.cs")).Returns(code);

            var options = new Options
            {
                ExtensionScriptPaths = {"/src/script.cs"},
                Properties = { ["Color"] = "blue" }
            };

            var subject = new ExtensionScriptSymbolStore(new CSharpCompiler(new Options())
                , new Mock<ILogger>().Object
                , options
                , fileSystemMock.Object);
            
            subject.Build();
            
            subject.GetValueFunction("Color")().ShouldBe("blue");
            subject.GetValueFunction("PropertyCount")().ShouldBe("1");
        }
    }
}