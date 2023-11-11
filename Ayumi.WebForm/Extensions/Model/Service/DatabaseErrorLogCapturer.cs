using System;
using WebLib.Extensions.Model.Object;
using WebLib.Extensions.Model.Repository;

namespace WebLib.Extensions.Model.Service
{
    public class DatabaseErrorLogCapturer : IErrorLogCapturer
    {
        private readonly IErrorInfoRepository errorInfoRepo;
        private readonly ISimpleObjectFactory objectFactory;
        private readonly String currentUser;
        private readonly String currentModule;

        public DatabaseErrorLogCapturer(IErrorInfoRepository errorInfoRepo, ISimpleObjectFactory objectFactory, String currentUser, String currentModule)
        {
            if (errorInfoRepo == null)
                throw new ArgumentNullException("errorInfoRepo");
            if (objectFactory == null)
                throw new ArgumentNullException("objectFactory");

            this.errorInfoRepo = errorInfoRepo;
            this.objectFactory = objectFactory;
            this.currentUser = currentUser;
            this.currentModule = currentModule;
        }

        public void CaptureException(Exception ex)
        {
            IErrorInfo errorInfo = objectFactory.CreateErrorInfo();
            errorInfo.UserId = currentUser;
            errorInfo.ModuleId = currentModule;
            errorInfo.ErrorTime = DateTime.Now;
            errorInfo.ErrorMessage = ex.Message;

            errorInfoRepo.Insert(errorInfo);
        }
    }
}