// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using Vertical.CommandLine;
using Vertical.TemplateCopy.Configuration;
using Vertical.TemplateCopy.Utilities;

namespace Vertical.TemplateCopy
{
    /// <summary>
    /// Container for the main function.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point 
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {
            try
            {
                CommandLineApplication.Run(new OptionsConfiguration(Run), args);
            }
            catch (UsageException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Need help? (t4copy --help)");
            }
        }

        private static void Run(Options options)
        {
            using (var logger = LoggingBootstrapper.CreateLogger(options))
            using (var services = new Services(options, logger))
            {
                services.Runner.Run();
            }
        }
    }
}
