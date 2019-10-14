// Copyright(c) 2018-2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.IO;
using Shouldly;
using Xunit;
using static System.Environment;

namespace Vertical.TemplateCopy.Macros
{
    public class PathMacrosTest
    {
        [Fact]
        public void SpecialFolder_Returns_Value()
        {
            PathMacros.SpecialFolder.ComputeValue(SpecialFolder.AdminTools.ToString())
                .ShouldBe(GetFolderPath(SpecialFolder.AdminTools));
        }

        [Fact]
        public void ExpandDotToPath_Returns_Value()
        {
            PathMacros.ExpandDotToPath.ComputeValue("vertical.templatecopy")
                .ShouldBe($"vertical{Path.DirectorySeparatorChar}templatecopy");
        }

        [Fact]
        public void ExpandToPath_Returns_Value()
        {
            PathMacros.ExpandToPath.ComputeValue("vertical").ShouldBe("vertical");
            PathMacros.ExpandToPath.ComputeValue("vertical,templatecopy").ShouldBe(
                $"vertical{Path.DirectorySeparatorChar}templatecopy");
        }
    }
}