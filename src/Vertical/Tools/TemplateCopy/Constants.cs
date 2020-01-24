namespace Vertical.Tools.TemplateCopy
{
    /// <summary>
    /// Defines program constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Defines the default symbol matching pattern.
        /// </summary>
        public const string DefaultSymbolPattern = @"\$\{(?<symbol>[a-zA-Z0-9_]+)\}";
        
        /// <summary>
        /// Defines the extensions wildcard.
        /// </summary>
        public const string ExtensionWildcard = ".*";
    }
}