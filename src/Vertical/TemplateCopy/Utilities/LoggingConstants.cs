namespace Vertical.TemplateCopy.Utilities
{
    /// <summary>
    /// Various constants related to logging.
    /// </summary>
    public static class LoggingConstants
    {
        /// <summary>
        /// Defines the new line string.
        /// </summary>
        public const string NewLine = "\r\n     ";

        /// <summary>
        /// Defines the output template.
        /// </summary>
        public const string OutputTemplate = "{Level:u3}: {step}{substep}{Message:lj}{NewLine}{Exception}";
    }
}