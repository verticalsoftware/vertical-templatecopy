// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Moq;
using System;
using Vertical.TemplateCopy.Abstractions;
using Microsoft.Extensions.Logging;

namespace Helpers
{
    public static class TestMocks
    {
        public static Mock<ITextTransform> RedundentTextTransformMock => new Func<Mock<ITextTransform>>(() =>
        {
            var mock = new Mock<ITextTransform>();
            mock.Setup(m => m.TransformContent(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string, string>((src, _, __) => src);
            return mock;
        })();

        public static Mock<IPathContextAccessor> PathContextAccessor => new Mock<IPathContextAccessor>();

        public static class NopLogger<T>
        {
            public static ILogger<T> Default => new Mock<ILogger<T>>().Object;
        }

        public static Mock<IAddOnStepsRunner> AddOnStepsRunner => new Mock<IAddOnStepsRunner>();
    }
}
