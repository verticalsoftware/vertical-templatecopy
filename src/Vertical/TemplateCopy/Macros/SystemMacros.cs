using System;

namespace Vertical.TemplateCopy.Macros
{
    public static class SystemMacros
    {
        public static readonly IMacro Now = new DelegateMacro(arg => string.IsNullOrWhiteSpace(arg)
            ? DateTime.Now.ToString()
            : DateTime.Now.ToString(arg));
    }
}
