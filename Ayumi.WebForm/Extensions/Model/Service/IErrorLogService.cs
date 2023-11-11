using System;

namespace WebLib.Extensions.Model.Service
{
    public interface IErrorLogService
    {
        void CaptureException(Exception ex);
    }
}