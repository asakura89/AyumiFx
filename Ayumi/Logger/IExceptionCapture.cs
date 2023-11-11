using System;

namespace Ayumi.Logger
{
    /// <summary>
    /// Defines basic exception capture functionality when it occured.
    /// </summary>
    public interface IExceptionCapture
    {
        /// <summary>
        /// Capture occured exception.
        /// </summary>
        /// <param name="ex">The occured exception.</param>
        void CaptureException(Exception ex);
    }
}