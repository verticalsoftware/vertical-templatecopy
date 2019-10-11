using Moq;
using System;
using System.Linq;
using Shouldly;
using Vertical.TemplateCopy.Abstractions;
using Xunit;

namespace Vertical.TemplateCopy.Text
{
    public class CompositeTextTransformTest
    {
        [Fact]
        public void TransformContent_Invokes_Provider()
        {
            var providerMock = new Mock<ITextTransformProvider>();
            providerMock.Setup(m => m.TransformContent("source"))
                .Returns("transformed")
                .Verifiable();

            var pathContextMock = new Mock<IPathContext>();
            pathContextMock.Setup(m => m.BeginPathContext("template", "target"))
                .Returns(new Mock<IDisposable>().Object);

            var testInstance = new CompositeTextTransform(new []{providerMock.Object}
                , new Mock<IPathContext>().Object);

            testInstance.TransformContent("source", "template", "target")
                .ShouldBe("transformed");

            providerMock.Verify(m => m.TransformContent("source"), Times.Once);
        }

        [Fact]
        public void TransformContent_Begins_PathContext()
        {
            var pathContextMock = new Mock<IPathContext>();
            pathContextMock.Setup(m => m.BeginPathContext("template", "target"))
                .Returns(new Mock<IDisposable>().Object)
                .Verifiable();

            var testInstance = new CompositeTextTransform(Enumerable.Empty<ITextTransformProvider>()
                , pathContextMock.Object);

            testInstance.TransformContent("source", "template", "target");

            pathContextMock.Verify(m => m.BeginPathContext("template", "target")
                , Times.Once);
        }
    }
}