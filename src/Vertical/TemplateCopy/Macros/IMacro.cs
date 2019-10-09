namespace Vertical.TemplateCopy.Macros
{
    public interface IMacro
    {
        /// <summary>
        /// Computes the value given the optional argument.
        /// </summary>
        /// <param name="argument">Argument</param>
        /// <returns>Result</returns>
        string ComputeValue(string argument);
    }
}
