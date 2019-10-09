using Microsoft.Extensions.Logging;
using System;

namespace Vertical.TemplateCopy
{
    /// <summary>
    /// Indicates a stop in process due to unrecoverable condition.
    /// </summary>
    public class AbortException : Exception
    {
        /// <summary>
        /// Creates a new isntance
        /// </summary>
        public AbortException() : base("Run step aborted")
        {
        }
    }
}
