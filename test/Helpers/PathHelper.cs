using System.IO;
using System.Text.RegularExpressions;

namespace Helpers
{
    public static class PathHelper
    {
        public static string _Path(params string[] p)
        {
            return Path.GetFullPath((string.Join('/', p)));
        }
    }
}
