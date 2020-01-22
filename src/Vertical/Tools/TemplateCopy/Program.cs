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
            CommandLineApplication.Run(new OptionsConfiguration(Run), args);
        }

        /// <summary>
        /// Handles the program
        /// </summary>
        private static void Run(Options options)
        {
            var services = Services.Create(options);

            try
            {
                services.Orchestrator.Run();
            }
            catch (Exception ex)
            {
                var logger = services.Logger;
                logger.Error(ex.Message);
                logger.Debug(ex, "Error occurred");
            }
        }
    }
}
