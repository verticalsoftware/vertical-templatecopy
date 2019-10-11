// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using Shouldly;
using Xunit;

namespace Vertical.TemplateCopy
{
    public class ProgramTest
    {
        [Fact]
        public void Main_No_Throw_For_Bad_Arg()
        {
            Should.NotThrow(() => Program.Main(new[] {"--x"}));
        }
    }
}