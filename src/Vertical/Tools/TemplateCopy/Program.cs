// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Diagnostics.CodeAnalysis;
using Vertical.CommandLine;

namespace Vertical.Tools.TemplateCopy
{
    /// <summary>
    /// Program entry point.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                CommandLineApplication.Run(new OptionsConfiguration(options =>
                    Services.Create(options).TaskAggregator.Run()), new[]{"-h"});
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
