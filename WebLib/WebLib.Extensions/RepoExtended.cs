using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using WebLib.Data;

namespace WebLib.Extensions
{
    public class RepoExtended<T> : RepoManager<T> where T : class
    {
        private readonly String userId;
        private readonly String moduleId;

        private class RepoOperation
        {
            public const String SELECT = "Select";
            public const String INSERT = "Insert";
            public const String UPDATE = "Update";
            public const String DELETE = "Delete";
            public const String EXECUTE = "Exec";
        }

        public RepoExtended(String userId, String moduleId)
        {
            this.userId = userId;
            this.moduleId = moduleId;
        }

        private Boolean IsUserGivenAccess(String operation)
        {
            switch(operation)
            {
                case RepoOperation.SELECT:
                    break;
                case RepoOperation.INSERT:
                    break;
                case RepoOperation.UPDATE:
                    break;
                case RepoOperation.DELETE:
                    break;
                case RepoOperation.EXECUTE:
                    break;
            }

            return true;
        }

        private void SetLastOperationWithCaution(ref T obj, String operation)
        {
            PropertyInfo lastOperationProperty = obj.GetType().GetProperty("last_operation");
            if (lastOperationProperty != null)
                lastOperationProperty.SetValue(obj, operation, null);
        }

        public virtual void Insert(T obj)
        {
            if (IsUserGivenAccess(RepoOperation.INSERT))
                DoInsert(obj);
            else
                throw new InvalidOperationException("You are not given access to Insert.");
        }

        private void DoInsert(T obj)
        {
            SetLastOperationWithCaution(ref obj, RepoOperation.INSERT);
            base.Insert(obj);
        }

        public virtual void Update(T obj)
        {
            if (IsUserGivenAccess(RepoOperation.UPDATE))
                DoUpdate(obj);
            else
                throw new InvalidOperationException("You are not given access to Update.");
        }

        private void DoUpdate(T obj)
        {
            SetLastOperationWithCaution(ref obj, RepoOperation.UPDATE);
            base.Update(obj);
        }

        public virtual void Delete(T obj)
        {
            if (IsUserGivenAccess(RepoOperation.DELETE))
                DoDelete(obj);
            else
                throw new InvalidOperationException("You are not given access to Delete.");
        }

        private void DoDelete(T obj)
        {
            SetLastOperationWithCaution(ref obj, RepoOperation.DELETE);
            base.Delete(obj);
        }

        public virtual List<T> GetDataList(List<ConditionData> listCondition)
        {
            if (IsUserGivenAccess(RepoOperation.SELECT))
                return base.GetDataList(listCondition);
            else
                throw new InvalidOperationException("You are not given access to Read.");
        }

        public virtual DataTable GetDataTable()
        {
            if (IsUserGivenAccess(RepoOperation.SELECT))
                return base.GetDataTable();
            else
                throw new InvalidOperationException("You are not given access to Read.");
        }

        public virtual DataTable GetDataTable(List<ConditionData> listCondition)
        {
            if (IsUserGivenAccess(RepoOperation.SELECT))
                return base.GetDataTable(listCondition);
            else
                throw new InvalidOperationException("You are not given access to Read.");
        }

        public virtual void ExecuteNonQuery(String query)
        {
            if (IsUserGivenAccess(RepoOperation.EXECUTE))
                base.ExecuteNonQuery(query);
            else
                throw new InvalidOperationException("You are not given access to Execute.");
        }
    }
}
