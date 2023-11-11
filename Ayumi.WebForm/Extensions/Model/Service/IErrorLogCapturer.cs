using System;

namespace WebLib.Extensions.Model.Service
{
    /// <summary>
    /// Used for error capture when error occurs.
    /// </summary>
    public interface IErrorLogCapturer
    {
        /// <summary>
        /// Process error capture
        /// </summary>
        /// <param name="ex">Occured exception</param>
        void CaptureException(Exception ex);
    }
}