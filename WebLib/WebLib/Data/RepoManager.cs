using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace WebLib.Data
{
    /*public class Repository
    {
        protected readonly MSSQL mssql;
        protected readonly String userName;
        protected readonly String module;
        protected readonly Boolean isUsingAuditTrail = true;

        public Repository()
        {
            mssql = new MSSQL();
        }

         public Repository(string pAppConfigName="DBConfig")
        {
            mssql = new MSSQL(pAppConfigName);
        }

        public Repository(Boolean pIsUsingAuditTrail, string pAppConfigName="DBConfig")
        {
            mssql = new MSSQL(pAppConfigName);
            isUsingAuditTrail = pIsUsingAuditTrail;
        }

        public Repository(Boolean pIsUsingAuditTrail, String pUsername, string pAppConfigName = "DBConfig")
        {
            mssql = new MSSQL(pAppConfigName);
            userName = pUsername;
            isUsingAuditTrail = pIsUsingAuditTrail;
        }

        public Repository(String pUsername, String pModule)
        {
            mssql = new MSSQL();
            userName = pUsername;
            module = pModule;
        }

        public void ExecSP(String spQuery, SqlParameter[] paramList)
        {
            mssql.Create();
            mssql.ExecuteStoredProcedure(spQuery, paramList);
        }

        public void ExecSP(String spQuery, string param)
        {
            mssql.Create();
            mssql.ExecuteStoredProcedure(spQuery, param);
        }

        public virtual DataTable GetDataTable(String query)
        {
            mssql.Create();
            DataTable dt = mssql.ExecuteDataTable(query);
            return dt;
        }

        public virtual DataTable GetDataTable(String query, params Object[] paramList)
        {
            mssql.Create();
            DataTable dt = mssql.ExecuteDataTable(query, paramList);
            return dt;
        }

        /// <summary>
        /// Execute Non Query 
        /// </summary>
        /// <param name="query">query string</param>
        public virtual void ExecuteNonQuery(string query)
        {
            mssql.Create();
            mssql.ExecuteNonQuery(query);
        }

        /// <summary>
        /// Execute Non Query 
        /// </summary>
        /// <param name="query">query string</param>
        public virtual void ExecuteNonQuery(String query, params Object[] paramList)
        {
            mssql.Create();
            mssql.ExecuteNonQuery(query, paramList);
        }

        public DataSet ExecDataSet(String spQuery)
        {
            mssql.Create();
            return mssql.ExecuteDataSet(spQuery, CommandType.Text);
        }

        public DataSet ExecDataSet(String spQuery, CommandType cmdType)
        {
            mssql.Create();
            return mssql.ExecuteDataSet(spQuery, cmdType);
        }
    }

    public class RepoManager<T> : Repository where T : class
    {
        public RepoManager() : base() { }

        public RepoManager(Boolean pIsUsingAuditTrail) : base(pIsUsingAuditTrail,"DBConfig") { }

        public RepoManager(string pAppConfigName) : base(pAppConfigName) { }

        public RepoManager(Boolean pIsUsingAuditTrail, string pAppConfigName) : base(pIsUsingAuditTrail, pAppConfigName) { }

        public RepoManager(Boolean pIsUsingAuditTrail, string pAppConfigName, String pUsername) : base(pIsUsingAuditTrail, pUsername, pAppConfigName) { }

        private static class AuditTrailConstant
        {
            public const string CREATED_DATE = "created_date";
            public const string LAST_UPDATE = "last_update";
            public const string CREATED_BY = "created_by";
            public const string LAST_ACCESS = "last_access";
            public const string LAST_OPERATION = "last_operation";
            public const string LAST_MODULES = "last_modules";
        }

        public T ExecScalar<T>(String query)
        {
            mssql.Create();
            Object result = mssql.ExecuteDataScalar(query);
            return result == DBNull.Value ? default(T) : (T) result;
        }

        public T ExecScalar<T>(String query, params Object[] queryParamList)
        {
            mssql.Create();
            Object result = mssql.ExecuteDataScalar(query, queryParamList);
            return result == DBNull.Value ? default(T) : (T)result;
        }

        public  DataTable ExecDataTable(string query, params Object[] paramList)
        {
            mssql.Create();
            return  mssql.ExecuteDataTable(query, paramList);
          
        }

        private void ExecSP(T obj, string status)
        {
            mssql.Create();
            List<ObjectRepoData> listRepo = ObjectHandler.GetListObjectRepoFromObject<T>();
            SqlParameter[] listParam =  ObjectHandler.GetListParamForSP<T>(obj, status);
            FillNullWithEmptyString(listParam);
            mssql.ExecuteStoredProcedure(ObjectRepoConstant.EXT_PROC + listRepo[0].Type, listParam);
        }

        /// <summary>
        /// Fill Null Parameter with string empty
        /// </summary>
        /// <param name="listParam"></param>
        private void FillNullWithEmptyString(SqlParameter[] listParam)
        {
            foreach (SqlParameter sqlParam in listParam)
            {
                if (sqlParam.Value == null)
                    sqlParam.Value = string.Empty;
            }
        }

        /// <summary>
        /// Execute SQL Store procedure Command Insert
        /// </summary>
        /// <param name="obj">object to be inserted</param>
        public virtual void Insert(T obj)
        {
            if (IsAuditTrailedObject(obj))
                 SetAuditTrailObjectInsert(obj);
            ExecSP(obj, SPStatus.SP_INSERT);
        }

        /// <summary>
        /// Execute SQL Store procedure Command Update
        /// </summary>
        /// <param name="obj">Object to be Updated</param>
        public virtual void Update(T obj)
        {
            if (IsAuditTrailedObject(obj))
                SetAuditTrailObjectUpdate(obj);
            ExecSP(obj, SPStatus.SP_UPDATE);
        }

        /// <summary>
        /// Execute SQL Store procedure command delete
        /// </summary>
        /// <param name="obj">Object to be Deleted</param>
        public virtual void Delete(T obj)
        {
            if (IsAuditTrailedObject(obj))
                SetAuditTrailObjectDelete(obj);
            ExecSP(obj, SPStatus.SP_DELETE);
        }

        private Boolean IsAuditTrailedObject(T obj)
        {
            return obj is IAuditTrail;
        }

        /// <summary>
        /// Get List of Object from SQL
        /// </summary>
        /// <param name="listCondition">filter condition</param>
        /// <returns></returns>
        public virtual List<T> GetDataList(List<ConditionData> listCondition)
        {
            mssql.Create();
            string query = ObjectHandler.GetSelectQuery<T>(listCondition);
            DataTable dt = mssql.ExecuteDataTable(query, CommandType.Text);
            return CommonFunction.ConvertDataTableToListObject<T>(dt);
        }

        /// <summary>
        /// Get List of Object from SQL
        /// </summary>
        /// <param name="listCondition">filter condition</param>
        /// <returns></returns>
        public virtual List<T> GetDataList(ConditionData condition)
        {
            mssql.Create();
            List<ConditionData> listCondition = new List<ConditionData>();
            if (!String.IsNullOrEmpty(condition.ColumnName) &&
                !String.IsNullOrEmpty(condition.Connector) &&
                !String.IsNullOrEmpty(condition.Operator))
            listCondition.Add(condition);
            String query = ObjectHandler.GetSelectQuery<T>(listCondition);
            DataTable dt = mssql.ExecuteDataTable(query, CommandType.Text);
            return CommonFunction.ConvertDataTableToListObject<T>(dt);
        }

        public List<T> GetDataList(String query)
        {
            mssql.Create();
            DataTable dt = mssql.ExecuteDataTable(query, CommandType.Text);
            return CommonFunction.ConvertDataTableToListObject<T>(dt);
        }

        public List<T> GetDataList(String query, params Object[] paramList)
        {
            mssql.Create();
            DataTable dt = mssql.ExecuteDataTable(query, paramList);
            return CommonFunction.ConvertDataTableToListObject<T>(dt);
        }

      

        /// <summary>
        /// Get Data Table of Object from sql 
        /// </summary>
        /// <returns>Data table</returns>
        public virtual DataTable GetDataTable()
        {
            return GetDataTable(new List<ConditionData>());
        }

        /// <summary>
        /// Get Data Table of Object from sql with condition
        /// </summary>
        /// <param name="listCondition">filter condition</param>
        /// <returns></returns>
        public virtual DataTable GetDataTable(List<ConditionData> listCondition)
        {
            mssql.Create();
            string query = ObjectHandler.GetSelectQuery<T>(listCondition);
            return mssql.ExecuteDataTable(query, CommandType.Text);
        }

        /// <summary>
        /// Get Data Table of Object from sql with condition
        /// </summary>
        /// <param name="listCondition">filter condition</param>
        /// <returns></returns>
        public virtual DataTable GetDataTable(List<ConditionData> listCondition, string sortCondition)
        {
            mssql.Create();
            string query = ObjectHandler.GetSelectQuery<T>(listCondition);
            if (!string.IsNullOrEmpty(sortCondition))
                query += " order by " + sortCondition;
            return mssql.ExecuteDataTable(query, CommandType.Text);
        }

        private void SetAuditTrailObjectInsert(T obj)
        {
            PropertyInfo propInfo= obj.GetType().GetProperty(AuditTrailConstant.CREATED_DATE);
            propInfo.SetValue(obj, DateTime.Now, null);
            propInfo= obj.GetType().GetProperty(AuditTrailConstant.LAST_UPDATE);
            propInfo.SetValue(obj, DateTime.Now, null);
            propInfo = obj.GetType().GetProperty(AuditTrailConstant.CREATED_BY);
            propInfo.SetValue(obj, userName, null);
            propInfo = obj.GetType().GetProperty(AuditTrailConstant.LAST_ACCESS);
            propInfo.SetValue(obj, userName, null);
            propInfo = obj.GetType().GetProperty(AuditTrailConstant.LAST_OPERATION);
            propInfo.SetValue(obj, SPStatus.SP_INSERT, null);
            propInfo = obj.GetType().GetProperty(AuditTrailConstant.LAST_MODULES);
            propInfo.SetValue(obj, module, null);
            
        }

        private void SetAuditTrailObjectDelete(T obj)
        {

            PropertyInfo propInfo = obj.GetType().GetProperty(AuditTrailConstant.LAST_UPDATE);
            propInfo.SetValue(obj, DateTime.Now, null);
            propInfo = obj.GetType().GetProperty(AuditTrailConstant.LAST_ACCESS);
            propInfo.SetValue(obj, userName, null);
            propInfo = obj.GetType().GetProperty(AuditTrailConstant.LAST_MODULES);
            propInfo.SetValue(obj, module, null);
            propInfo = obj.GetType().GetProperty(AuditTrailConstant.LAST_OPERATION);
            propInfo.SetValue(obj, SPStatus.SP_DELETE, null);

        }


        private void SetAuditTrailObjectUpdate(T obj)
        {

            PropertyInfo propInfo = obj.GetType().GetProperty(AuditTrailConstant.LAST_UPDATE);
            propInfo.SetValue(obj, DateTime.Now, null);
            propInfo = obj.GetType().GetProperty(AuditTrailConstant.LAST_ACCESS);
            propInfo.SetValue(obj, userName, null);
            propInfo = obj.GetType().GetProperty(AuditTrailConstant.LAST_MODULES);
            propInfo.SetValue(obj, module, null);
            propInfo = obj.GetType().GetProperty(AuditTrailConstant.LAST_OPERATION);
            propInfo.SetValue(obj, SPStatus.SP_UPDATE, null);

        }

        //public T GetDataByPrimaryKey(T obj)
        //{
        //    List<T> listT = GetDataList(CreateListConditionOfPrimaryKey(obj));
        //    if (listT != null && listT.Count != 0)
        //        return listT[0];
        //    return null;
        //}

        //private List<ConditionData> CreateListConditionOfPrimaryKey(T obj)
        //{
        //    List<string> listPrimaryKey = GetPrimaryKeysOfObject();
        //    List<ConditionData> listCondition = new List<ConditionData>();
        //    foreach (string primeKey in listPrimaryKey)
        //    {
        //        PropertyInfo propInfo = obj.GetType().GetProperty(primeKey);
        //        Type returnType = propInfo.PropertyType;
        //        ConditionData condData = new ConditionData(ConnectorConstant.CON_AND,primeKey,OperatorConstant.OP_EQUAL,propInfo.GetValue(obj, null).ToString());
        //        listCondition.Add(condData);
        //    }
        //    return listCondition;
        //}

        //private  List<string> GetPrimaryKeysOfObject()
        //{
        //    List<string> listPrimary = new List<string>();
        //    T objData = Activator.CreateInstance<T>();
        //    List<ObjectRepoData> listProp = ObjectHandler.GetListObjectRepoFromObject<T>();
        //    foreach (ObjectRepoData objRepo in listProp)
        //    {
        //        if (objRepo.Name.Equals(ObjectRepoConstant.TABLE_NAME))
        //            continue;
        //        Type type = objData.GetType();
        //        PropertyInfo propInfo = type.GetProperty(objRepo.Name);
        //        Type returnType = propInfo.PropertyType;
        //        if ((returnType.IsGenericType &&
        //    returnType.GetGenericTypeDefinition() == typeof(List<>)))
        //            continue;
        //        object[] attr = propInfo.GetCustomAttributes(typeof(ColumnAttribute), false);
        //        bool isPrimaryKey = ((ColumnAttribute)attr[0]).IsPrimaryKey;
        //        if (isPrimaryKey)
        //            listPrimary.Add(objRepo.Name);
        //    }
        //    return listPrimary;
        //}
    }*/
}
