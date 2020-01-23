using System;
using Infrastructure;
using Moq;
using Serilog;
using Shouldly;
using Xunit;

namespace Vertical.Tools.TemplateCopy
{
    public class EnvironmentSymbolStoreTests
    {
        private static readonly string Id = Guid.NewGuid().ToString();
        
        private readonly ISymbolStore _subject = new Func<ISymbolStore>(() =>
        {
            var logger = MockLogger.Default;
            Environment.SetEnvironmentVariable("VERTICAL_TESTID", Id);
            var subject = new EnvironmentSymbolStore(logger);
            subject.Build();
            return subject;
        })();

        [Fact]
        public void GetValueFunction_Returns_Null_For_Missing_Variable()
        {
            // Could be a flaky test...
            _subject.GetValueFunction(Guid.NewGuid().ToString()).ShouldBeNull();
        }

        [Fact]
        public void GetValueFunction_Returns_Function_For_Known_Variable()
        {
            _subject.GetValueFunction("VERTICAL_TESTID")().ShouldBe(Id);
        }
    }
}