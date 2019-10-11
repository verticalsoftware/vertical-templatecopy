using System.IO;
using System.Text.RegularExpressions;

namespace Helpers
{
    public static class PathHelper
    {
        public static string _Path(string p)
        {
            return Path.GetFullPath(p);
        }
    }
}
