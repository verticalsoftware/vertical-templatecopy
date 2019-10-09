namespace Vertical.TemplateCopy.Utilities
{
    /// <summary>
    /// Describes what to do for duplicate files in copy operations.
    /// </summary>
    public enum FileOperation
    {
        /// <summary>
        /// Replaces the file
        /// </summary>
        Replace,

        /// <summary>
        /// Skips the file operation.
        /// </summary>
        Skip,

        /// <summary>
        /// Cancels the current operation
        /// </summary>
        Cancel
    }
}
