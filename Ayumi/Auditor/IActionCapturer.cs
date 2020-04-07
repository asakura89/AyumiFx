using System;

namespace Ayumi.Auditor {
    /// <summary>
    /// Defines basic user action capture functionality.
    /// </summary>
    public interface IActionCapturer {
        /// <summary>
        /// Capture current performed user action.
        /// </summary>
        /// <param name="action">Performed user action.</param>
        /// <param name="description">Action description.</param>
        void CaptureAction(String action, String description);
    }
}