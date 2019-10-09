using System;
using Vertical.CommandLine;
using Vertical.TemplateCopy.Configuration;

namespace Vertical.TemplateCopy
{
    class Program
    {
        static void Main(string[] args) => CommandLineApplication.Run(new OptionsConfiguration(Run), args);

        private static void Run(Options options)
        {
            using (var logger = Logging.CreateLogger(options))
            using (var services = new Services(options, logger))
            {
                try
                {
                    services.Runner.Run();
                }
                catch (Exception ex)
                {
                    // Our exception
                    logger.Error(ex.Message);
                }
            }
        }
    }
}
