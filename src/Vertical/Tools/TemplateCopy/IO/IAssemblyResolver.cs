namespace Vertical.Tools.TemplateCopy.IO
{
    /// <summary>
    /// Represents an object that locates assemblies.
    /// </summary>
    public interface IAssemblyResolver
    {
        /// <summary>
        /// Finds an assembly.
        /// </summary>
        /// <param name="assembly">Assembly name or path.</param>
        /// <returns>Path</returns>
        string GetAssemblyPath(string assembly);
    }
}