// Copyright(c) 2018-2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using Helpers;
using Moq;
using Shouldly;
using Vertical.TemplateCopy.Abstractions;
using Vertical.TemplateCopy.Configuration;
using Xunit;

namespace Vertical.TemplateCopy.Text
{
    public class EnvironmentVariableProviderTest : IClassFixture<EnvironmentVariableProviderTest.State>
    {
        private readonly State state;

        public EnvironmentVariableProviderTest(State state) => this.state = state;

        public class State : IDisposable
        {
            public State() => Environment.SetEnvironmentVariable(Key, Value, EnvironmentVariableTarget.Process);

            public string Key { get; } = Guid.NewGuid().ToString();

            public string Value => "test_value";

            public void Dispose() => Environment.SetEnvironmentVariable(Key, null);
        }

        private readonly EnvironmentVariableProvider testInstance = new EnvironmentVariableProvider(
            TestMocks.NopLogger<EnvironmentVariableProvider>.Default
            , new Options()
            , new Mock<IPathContextAccessor>().Object);

        [Fact]
        public void TransformContent_Replaces_Matched_Environment_Variable_Template()
        {
            testInstance.TransformContent($"input $(env:{state.Key})").ShouldBe($"input {state.Value}");
        }

        [Fact]
        public void TransformContent_Ignores_NonMatched_Environment_Variable_Tempalte()
        {
            var otherKey = Guid.NewGuid().ToString();
            testInstance.TransformContent($"input $(env:{state.Key}) $(env:{otherKey})")
                .ShouldBe($"input {state.Value} $(env:{otherKey})");
        }
    }
}