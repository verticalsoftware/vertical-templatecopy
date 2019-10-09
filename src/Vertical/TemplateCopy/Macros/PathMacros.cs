using System;
using System.IO;
using static System.Environment;

namespace Vertical.TemplateCopy.Macros
{
    public static class PathMacros
    {
        public static IMacro SpecialFolder = new DelegateMacro(arg => GetFolderPath(Enum.Parse<SpecialFolder>(arg, true)));

        public static IMacro ExpandDotToPath = new DelegateMacro(arg => arg.Replace('.', Path.DirectorySeparatorChar));
    }
}
