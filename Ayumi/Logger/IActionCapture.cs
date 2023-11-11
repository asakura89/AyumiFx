using System;

namespace Ayumi.Logger
{
    /// <summary>
    /// Defines basic user action capture functionality.
    /// </summary>
    public interface IActionCapture
    {
        /// <summary>
        /// Capture current performed user action.
        /// </summary>
        /// <param name="action">Performed user action.</param>
        /// <param name="description">Action description.</param>
        void CaptureAction(String action, String description);
    }
}