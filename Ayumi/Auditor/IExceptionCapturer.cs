using System;

namespace Ayumi.Auditor {
    /// <summary>
    /// Defines basic exception capture functionality when it occured.
    /// </summary>
    public interface IExceptionCapturer {
        /// <summary>
        /// Capture occured exception.
        /// </summary>
        /// <param name="ex">The occured exception.</param>
        void CaptureException(Exception ex);
    }
}