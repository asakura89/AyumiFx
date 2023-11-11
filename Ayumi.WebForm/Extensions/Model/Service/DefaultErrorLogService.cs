using System;

namespace WebLib.Extensions.Model.Service
{
    public abstract class DefaultErrorLogService : IErrorLogService
    {
        private readonly ConsoleErrorLogCapturer consoleCapturer;
        private readonly DatabaseErrorLogCapturer dbCapturer;
        private readonly WindowsEventLogErrorLogCapturer eventLogCapturer;

        protected DefaultErrorLogService(ConsoleErrorLogCapturer consoleCapturer, DatabaseErrorLogCapturer dbCapturer, WindowsEventLogErrorLogCapturer eventLogCapturer)
        {
            if (consoleCapturer == null)
                throw new ArgumentNullException("consoleCapturer");
            if (dbCapturer == null)
                throw new ArgumentNullException("dbCapturer");
            if (eventLogCapturer == null)
                throw new ArgumentNullException("eventLogCapturer");

            this.consoleCapturer = consoleCapturer;
            this.dbCapturer = dbCapturer;
            this.eventLogCapturer = eventLogCapturer;
        }

        public void CaptureException(Exception ex)
        {
            try
            {
                dbCapturer.CaptureException(ex);
            }
            catch (Exception exx)
            {
                try
                {
                    eventLogCapturer.CaptureException(ex);
                    eventLogCapturer.CaptureException(exx);
                }
                catch
                {
                    consoleCapturer.CaptureException(ex);
                    consoleCapturer.CaptureException(exx);
                }
            }
        }
    }
}
