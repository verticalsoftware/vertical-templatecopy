using System;

namespace Vertical.TemplateCopy.Macros
{
    public class DelegateMacro : IMacro
    {
        private readonly Func<string, string> transform;

        public DelegateMacro(Func<string, string> transform) => this.transform = transform;

        public string ComputeValue(string argument) => transform(argument);
    }
}
