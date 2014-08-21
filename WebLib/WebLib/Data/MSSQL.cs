using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Net;
using System.Reflection;
using EnumExperimentation;
using WebLib.Constant;
using WebLib.Security;

namespace WebLib.Data
{
    public class MSSQL : IDisposable
    {
        /*private WebSecurity cSec = new WebSecurity();
        private string _ConnectionString;
        private SqlConnection _Connection = new SqlConnection();
        public string _UserID;
        public string _Server;
        public string _Database;
        public string _DataSource;
        public string _Password;
        private string _Transaction="";
        private string _AppConfigName = "DBConfig";
        private int _TimeOut;*/

        /*public MSSQL(string appConfigName)
        {
            if (!string.IsNullOrEmpty(appConfigName)) 
                _AppConfigName = appConfigName;
        }

        public MSSQL()
            : this("DBConfig") { }*/

        protected const String SqlServerProvider = "System.Data.SqlClient";
        private DbConnection connection;
        private DbProviderFactory factory;
        private readonly WebSecurity security = new WebSecurity();


        public class DefaultMSSQLConfiguration : IMSSQLConfiguration
        {
            public String ConnectionString { get; set; }
            public String ConnectionProvider { get; set; }
            public ConnectionStringSource ConnectionStringSource { get; set; }
            public ConnectionStringEncryptionType ConnectionStringEncryptionType { get; set; }

            public DefaultMSSQLConfiguration()
            {
                ConnectionString = "DbConfig";
                ConnectionProvider = SqlServerProvider;
                ConnectionStringSource = ConnectionStringSource.AppConfig;
                ConnectionStringEncryptionType = ConnectionStringEncryptionType.Normal;
            }
        }

        public enum ConnectionStringEncryptionType
        {
            Normal,
            Registry
        }

        public enum ConnectionStringSource
        {
            ConnectionString,
            AppConfig
        }

        /*public MSSQL(String connectionString, String provider, ConnectionStringSource connectionStringSource)
        {
            OpenConnection(connectionString, provider, connectionStringSource);
        }

        public MSSQL(String connectionStringName, String provider) : this(connectionStringName, provider, ConnectionStringSource.AppConfig) { }

        public MSSQL(String connectionStringName) : this(connectionStringName, SqlServerProvider) { }*/

        public MSSQL(IMSSQLConfiguration mssqlConfig)
        {
            //ValidateConfiguration(mssqlConfig);

            //const String ConnectionStringTemplate = "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};";
            String decryptedConnectionString = String.Empty;
            String provider = String.IsNullOrEmpty(mssqlConfig.ConnectionProvider) ? SqlServerProvider : mssqlConfig.ConnectionProvider;
            if (mssqlConfig.ConnectionStringSource == ConnectionStringSource.AppConfig)
            {
                switch (mssqlConfig.ConnectionStringEncryptionType)
                {
                    case ConnectionStringEncryptionType.Normal:
                        String encryptedConnectionString = ConfigurationManager.AppSettings[mssqlConfig.ConnectionString];
                        decryptedConnectionString = security.DecryptTripleDes(encryptedConnectionString, false);
                        break;
                    case ConnectionStringEncryptionType.Registry:
                        decryptedConnectionString = CommonFunction.GetWebConfigValue(mssqlConfig.ConnectionString);
                        break;
                }
            }
            else
                decryptedConnectionString = mssqlConfig.ConnectionString;

            OpenConnection(decryptedConnectionString, provider);
        }

        /*private void ValidateConfiguration(Configuration mssqlConfiguration)
        {
            throw new NotImplementedException();
        }*/

        /*/// <summary>
        /// property of SQL Transaction
        /// </summary>
        /// 


        public string SQLTransaction
        {
            get
            {
                return _Transaction;
            }
            set
            {
                _Transaction = value;
            }
        }*/
        /*/// <summary>
        /// to get parameter value with name and parameter
        /// </summary>
        /// <param name="_Name">name</param>
        /// <param name="_Parameter">parameter</param>
        /// <returns></returns>
        public string GetParameterValue(string _Name, string _Parameter)
        {
            try
            {
                string _Return = "" + _Parameter.Split(';').GetUpperBound(0) + "";
                if (_Parameter.Split(';').GetUpperBound(0) > 0)
                {

                    for (int i = 0; i <= _Parameter.Split(';').GetUpperBound(0); i++)
                    {
                        if (_Parameter.Split(';')[i].Split('=').GetUpperBound(0) > 0)
                        {
                            if ((_Parameter.Split(';')[i].Split('=')[0].ToString().Trim().ToLower() == _Name.ToLower().Trim()) || (_Parameter.Split(';')[i].Split('=')[0].ToString().Trim() == "@" + _Name.ToLower().Trim()))
                            {
                                _Return = _Parameter.Split(';')[i].Split('=')[1].ToString();
                            }
                        }
                    }
                }
                return _Return;
            }catch(Exception e)
            {
                return "";
            }

        }*/

        /*//#region Connection Handling
        /// <summary>
        /// Open database connection
        /// </summary>*/
        public void OpenConnection(String connectionString, String provider)
        {
            /*CloseConnection();
            try
            {
                _Connection.Open();
                if (SQLTransaction != "")
                    _Connection.BeginTransaction(SQLTransaction);
                /*else
                    _Connection.BeginTransaction();#1#
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }*/

            if (connection != null)
                CloseConnection();

            factory = DbProviderFactories.GetFactory(provider);
            connection = factory.CreateConnection();
            if (connection == null)
                throw new InvalidOperationException("Connection creation from factory failed.");

            connection.ConnectionString = connectionString;
            connection.Open();
        }

        /*/// <summary>
        /// close database connection
        /// </summary>*/
        private void CloseConnection()
        {
            /*if (_Connection.State != ConnectionState.Closed)
            {
                try
                {
                    _Connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }*/
            connection.Close();
            connection.Dispose();
            connection = null;
        }

        private DbCommand BuildProcedureCommand(String procedureName)
        {
            return BuildProcedureCommand(procedureName, null);
        }

        private DbCommand BuildProcedureCommand(String procedureName, Object[] args)
        {
            return BuildDbCommand(procedureName, args, CommandType.StoredProcedure);
        }

        private DbCommand BuildSqlCommand(String queryString)
        {
            return BuildSqlCommand(queryString, null);
        }

        private DbCommand BuildSqlCommand(String queryString, Object[] args)
        {
            return BuildDbCommand(queryString, args, CommandType.Text);
        }

        private DbCommand BuildDbCommand(String queryString, Object[] args, CommandType dbCommandType)
        {
            DbCommand builtDbCommand = connection.CreateCommand();
            builtDbCommand.CommandText = queryString;
            builtDbCommand.CommandType = dbCommandType;
            if (args != null)
                BuildDbCommandParameter(ref builtDbCommand, args);

            return builtDbCommand;
        }

        private void BuildDbCommandParameter(ref DbCommand builtDbCommand, Object[] queryParameters)
        {
            builtDbCommand.Parameters.Clear();
            for (Int32 paramIdx = 0; paramIdx < queryParameters.Length; paramIdx++)
            {
                Object currentArgs = queryParameters[paramIdx] ?? DBNull.Value;
                DbParameter param = builtDbCommand.CreateParameter();
                param.ParameterName = "@" + paramIdx.ToString();
                param.Value = currentArgs;
                builtDbCommand.Parameters.Add(param);
            }
        }

        private DbDataAdapter BuildSelectDataAdapter(DbCommand builtSqlCommand)
        {
            DbDataAdapter builtDataAdapter = factory.CreateDataAdapter();
            if (builtDataAdapter == null)
                throw new Exception("Data Adapter creation from factory failed.");

            builtDataAdapter.SelectCommand = builtSqlCommand;
            return builtDataAdapter;
        }

        /*/// <summary>
        /// get webconfig dbscript consist of
        /// use, server, database, datasource, password, timeout and globaltransact        /// 
        /// </summary>
        /// <returns></returns>
        public string WebConfigDBScript()
        {
            return "user=" + _UserID + ";server=" + _Server + ";database=" + _Database + ";datasource=" + _DataSource + ";password=" + _Password + ";timeout=" + _TimeOut.ToString().Trim() + ";globaltransact="+_Transaction+"";
        }*/
        /*/// <summary>
        /// Init Database considt of
        /// userid,data_source,database and password
        /// </summary>
        public void InitDatabase()
        {
            try
            {
                _ConnectionString = "workstation id=@SERVER;packet size=4096;user id=@USER_ID;data source=@DATA_SOURCE;persist security info=True;initial catalog=@DATABASE;password=@PASSWORD";
                string SetConfig = cSec.DecryptTripleDes(System.Configuration.ConfigurationManager.AppSettings.Get(_AppConfigName), false);
                if (GetParameterValue("cfgmode", SetConfig).ToLower() == "dbconfig")
                {
                    _UserID = GetParameterValue("User", SetConfig);       //User
                    _Server = GetParameterValue("Server", SetConfig);     //Server
                    _Database = GetParameterValue("Database", SetConfig); //Database
                    _DataSource = GetParameterValue("Source", SetConfig); //Source
                    if (_DataSource.Trim().Length == 0)
                        _DataSource = _Server;
                    _Password = GetParameterValue("password", SetConfig);                //Password
                    _TimeOut = Convert.ToInt16(GetParameterValue("TimeOut", SetConfig)); //Timeout'
                    _Transaction = GetParameterValue("globaltransact", SetConfig);
                }
                else
                    throw new Exception("Error setting configuration!");
            }
            catch(Exception ex)
            {
                Console.WriteLine("DBConnHandler.SetConnDetailsFromConfig. Error in setup db connection details. Error Message : " + ex.Message);
                EventLog appLog = new EventLog();
                appLog.Source = "UOBI Framework";
                appLog.WriteEntry("DBConnHandler.SetConnDetailsFromConfig. Error in setup db connection details. Error Message : " + ex.Message);
                Environment.Exit(20);
            }
        }*/

        /*/// <summary>
        /// update database connection
        /// </summary>
        public void UpdateConnection()
        {
            try
            {
                if (_DataSource.Trim().Length == 0)
                    _DataSource = _Server;
                CloseConnection();
                _Connection.ConnectionString = _ConnectionString.Replace("@USER_ID", _UserID).Replace("@SERVER", _Server).Replace("@DATABASE", _Database).Replace("@DATA_SOURCE", _DataSource).Replace("@DATABASE", _Database).Replace("@PASSWORD", _Password);
            }
            catch(Exception ex)
            {
                Console.WriteLine("DBConnHandler.SetConnDetailsFromConfig. Error in setup db connection details. Error Message : " + ex.Message);
                EventLog appLog = new EventLog();
                appLog.Source = "UOBI Framework";
                appLog.WriteEntry("DBConnHandler.SetConnDetailsFromConfig. Error in setup db connection details. Error Message : " + ex.Message);
                Environment.Exit(20);
            }
            
        }
        #endregion*/
        //#region Data Handling
        /*/// <summary>
        /// Function to get DataTable from Tabel, View, T-SQL
        /// </summary>
        /// <param name="_TableOrVieworSQL">name of table or view</param>
        /// <param name="_CmdType">cmdType</param>
        /// <returns>datatable</returns>
        public string GetBranchCode(string _TSQL)
        {
            string result = "";
            SqlCommand SQLCommand = new SqlCommand();
            SqlDataAdapter SqlClient = new SqlDataAdapter();
            SqlDataReader _DataValue;
            //Preparing Connection
            OpenConnection();
            SQLCommand.Connection = _Connection;
            SQLCommand.CommandText = _TSQL.Trim();
            if (_TimeOut > 0)
                SQLCommand.CommandTimeout = _TimeOut;
            else
                SQLCommand.CommandTimeout = 0;
            SQLCommand.CommandType = CommandType.Text;
            _DataValue = SQLCommand.ExecuteReader();

            while (_DataValue.Read())
            {
                result = _DataValue["OfficeCode"].ToString();
            }
            return result;
        }

        public string GetDebetCode(string _TSQL)
        {
            string result = "";
            SqlCommand SQLCommand = new SqlCommand();
            SqlDataAdapter SqlClient = new SqlDataAdapter();
            SqlDataReader _DataValue;
            //Preparing Connection
            OpenConnection();
            SQLCommand.Connection = _Connection;
            SQLCommand.CommandText = _TSQL.Trim();
            if (_TimeOut > 0)
                SQLCommand.CommandTimeout = _TimeOut;
            else
                SQLCommand.CommandTimeout = 0;
            SQLCommand.CommandType = CommandType.Text;
            _DataValue = SQLCommand.ExecuteReader();

            while (_DataValue.Read())
            {
                result = _DataValue["DebetID"].ToString();
            }
            return result;
        }

        public string GetCreditTransaction(string _TSQL)
        {
            string result = "";
            SqlCommand SQLCommand = new SqlCommand();
            SqlDataAdapter SqlClient = new SqlDataAdapter();
            SqlDataReader _DataValue;
            //Preparing Connection
            OpenConnection();
            SQLCommand.Connection = _Connection;
            SQLCommand.CommandText = _TSQL.Trim();
            if (_TimeOut > 0)
                SQLCommand.CommandTimeout = _TimeOut;
            else
                SQLCommand.CommandTimeout = 0;
            SQLCommand.CommandType = CommandType.Text;
            _DataValue = SQLCommand.ExecuteReader();

            while (_DataValue.Read())
            {
                result = _DataValue["CreditTransactionID"].ToString();
            }
            return result;
        }

        public string GetBranchCodeName(string _TSQL)
        {
            string result = "";
            SqlCommand SQLCommand = new SqlCommand();
            SqlDataAdapter SqlClient = new SqlDataAdapter();
            SqlDataReader _DataValue;
            //Preparing Connection
            OpenConnection();
            SQLCommand.Connection = _Connection;
            SQLCommand.CommandText = _TSQL.Trim();
            if (_TimeOut > 0)
                SQLCommand.CommandTimeout = _TimeOut;
            else
                SQLCommand.CommandTimeout = 0;
            SQLCommand.CommandType = CommandType.Text;
            _DataValue = SQLCommand.ExecuteReader();

            while (_DataValue.Read())
            {
                result = _DataValue["BranchName"].ToString() ;
            }
            return result;
        }*/
        /*public DataTable ExecuteDataTable(string _TableOrVieworSQL, CommandType _CmdType)
        {
            SqlCommand SQLCommand = new SqlCommand();
            SqlDataAdapter SQLAdapter = new SqlDataAdapter();
            DataTable _DataTable = new DataTable();
            //Preparing Connection
            OpenConnection();
            SQLCommand.Connection = _Connection;
            if (_CmdType == CommandType.TableDirect)
                SQLCommand.CommandText = "Select * From " + _TableOrVieworSQL + "";
            else if (_CmdType == CommandType.Text)
                SQLCommand.CommandText = _TableOrVieworSQL;
            if (_TimeOut > 0)
                SQLCommand.CommandTimeout = _TimeOut;
            else
                SQLCommand.CommandTimeout = 0;
            SQLCommand.CommandType = CommandType.Text;
            //Preparing Adapter
            SQLAdapter.SelectCommand = SQLCommand;            
            SQLAdapter.Fill(_DataTable);
            CloseConnection();
            return _DataTable;
        }*/

        public DataSet ExecuteDataSet(String queryString)
        {
            var targetDataSet = new DataSet();
            using (DbCommand cmd = BuildSqlCommand(queryString))
            {
                using (DbDataAdapter dataAdapter = BuildSelectDataAdapter(cmd))
                    dataAdapter.Fill(targetDataSet);
            }

            return targetDataSet;
        }

        public DataSet ExecuteDataSet(String queryString, params Object[] queryParamList)
        {
            var targetDataSet = new DataSet();
            using (DbCommand cmd = BuildSqlCommand(queryString, queryParamList))
            {
                using (DbDataAdapter dataAdapter = BuildSelectDataAdapter(cmd))
                    dataAdapter.Fill(targetDataSet);
            }

            return targetDataSet;
        }

        public DataTable ExecuteDataTable(String queryString)
        {
            var targetDataTable = new DataTable();
            using (DbCommand cmd = BuildSqlCommand(queryString))
            {
                using (DbDataAdapter dataAdapter = BuildSelectDataAdapter(cmd))
                    dataAdapter.Fill(targetDataTable);
            }

            return targetDataTable;
        }

        public DataTable ExecuteDataTable(String queryString, params Object[] queryParamList)
        {
            var targetDataTable = new DataTable();
            using (DbCommand cmd = BuildSqlCommand(queryString, queryParamList))
            {
                using (DbDataAdapter dataAdapter = BuildSelectDataAdapter(cmd))
                    dataAdapter.Fill(targetDataTable);
            }

            return targetDataTable;
        }

        public void ExecuteNonQuery(String queryString)
        {
            using (DbCommand cmd = BuildSqlCommand(queryString))
                cmd.ExecuteNonQuery();
        }

        public void ExecuteNonQuery(String queryString, params Object[] queryParamList)
        {
            using (DbCommand cmd = BuildSqlCommand(queryString, queryParamList))
                cmd.ExecuteNonQuery();
        }

        /*private void CreateSqlParameter(SqlCommand cmd, Object[] paramList)
        {
            cmd.Parameters.Clear();
            for (int index = 0; index < paramList.Length; index++)
                cmd.Parameters.AddWithValue("@" + index, paramList[index]);
        }*/

        /*/// <summary>
        /// Function to get DataSet from Tabel, View, T-SQL
        /// </summary>
        /// <param name="_TableOrViewOrQueryOrSP">name of table or view</param>
        /// <param name="_CmdType">cmdType</param>
        /// <returns>dataset</returns>
        public DataSet ExecuteDataSet(string _TableOrViewOrQueryOrSP, CommandType _CmdType)
        {
            SqlCommand SQLCommand = new SqlCommand();
            SqlDataAdapter SQLAdapter = new SqlDataAdapter();
            DataSet _DataSet = new DataSet();
            //Preparing Connection
            OpenConnection();
            SQLCommand.Connection = _Connection;
            SQLCommand.CommandText = _TableOrViewOrQueryOrSP;
            if (_TimeOut > 0)
                SQLCommand.CommandTimeout = _TimeOut;
            else
                SQLCommand.CommandTimeout = 0;
            if (_CmdType == CommandType.StoredProcedure)
                SQLCommand.CommandType = CommandType.StoredProcedure;
            else
            {
                SQLCommand.CommandType = CommandType.Text;
                if (_CmdType == CommandType.TableDirect)
                    SQLCommand.CommandText = "SELECT * FROM "+_TableOrViewOrQueryOrSP+"";
            }
            //Preparing Adapter
            SQLAdapter.SelectCommand = SQLCommand;
            SQLAdapter.Fill(_DataSet);
            CloseConnection();
            return _DataSet;
        }
        /// <summary>
        /// Function to ExecuteNonQuery
        /// </summary>
        /// <param name="_Query">query that you want to execute</param>
        /// <returns>1/0(true/false)</returns>

        public DataSet ExecuteDataSet(string _TableOrViewOrQueryOrSP, CommandType _CmdType, SqlParameter[] _Params)
        {
            SqlCommand SQLCommand = new SqlCommand();
            SqlDataAdapter SQLAdapter = new SqlDataAdapter();
            DataSet _DataSet = new DataSet();
            //Preparing Connection
            OpenConnection();
            SQLCommand.Connection = _Connection;
            SQLCommand.CommandText = _TableOrViewOrQueryOrSP;
            SQLCommand.Parameters.Clear();
            if (_TimeOut > 0)
                SQLCommand.CommandTimeout = _TimeOut;
            else
                SQLCommand.CommandTimeout = 0;
            if (_CmdType == CommandType.StoredProcedure)
            {
                SQLCommand.CommandType = CommandType.StoredProcedure;
                if (_Params.GetUpperBound(0) >= 0)
                {
                    for (int i = 0; i <= _Params.GetUpperBound(0); i++)
                    {
                        SQLCommand.Parameters.Add(_Params[i]);
                    }
                }
            }
            else
            {
                SQLCommand.CommandType = CommandType.Text;
                if (_CmdType == CommandType.TableDirect)
                    SQLCommand.CommandText = "SELECT * FROM " + _TableOrViewOrQueryOrSP + "";
            }
            //Preparing Adapter
            SQLAdapter.SelectCommand = SQLCommand;
            try
            {
                SQLAdapter.Fill(_DataSet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (_CmdType == CommandType.StoredProcedure)
                {
                    for (int i = 0; i <= _Params.GetUpperBound(0); i++)
                    {
                        //_Params[i].Value = SQLCommand.Parameters.
                    }
                }
            }
            CloseConnection();
            return _DataSet;
        }
        public int ExecuteNonQuery(string _Query)
        {
            SqlCommand SQLCommand = new SqlCommand();
            DataSet _DataSet = new DataSet();
            int _ReturnValue = 0;
            //Preparing Connection
            OpenConnection();
            SQLCommand.CommandType = CommandType.Text;
            SQLCommand.Connection = _Connection;
            SQLCommand.CommandText = _Query;
            if (_TimeOut > 0)
                SQLCommand.CommandTimeout = _TimeOut;
            else
                SQLCommand.CommandTimeout = 0;
            _ReturnValue = SQLCommand.ExecuteNonQuery();
            //Preparing Adapter
            CloseConnection();
            return _ReturnValue;
        }
        /// <summary>
        /// Function to ExecuteNonQuery with input parameter array of string
        /// </summary>
        /// <param name="_Query">query that you want to execute</param>
        /// <returns>1/0(true/false)</returns>
        public int ExecuteNonQuery(string[] _Query)
        {
            SqlCommand SQLCommand = new SqlCommand();
            DataSet _DataSet = new DataSet();
            int _ReturnValue = 0;
            //Preparing Connection
            OpenConnection();
            SQLCommand.CommandType = CommandType.Text;
            SQLCommand.Connection = _Connection;
            if (_TimeOut > 0)
                SQLCommand.CommandTimeout = _TimeOut;
            else
                SQLCommand.CommandTimeout = 0;
            for (int i = 0; i <= _Query.GetUpperBound(0); i++)
            {
                SQLCommand.CommandText = _Query[i];
                _ReturnValue = SQLCommand.ExecuteNonQuery();   
            }
            //Preparing Adapter
            CloseConnection();
            return _ReturnValue;
        }*/
        /*/// <summary>
        /// Function to ExecuteStoredProcedure
        /// </summary>
        /// <param name="_StoredProcedure">name of storedprocedure</param>
        /// <param name="_Parameter">paramater</param>
        /// <returns>1/0(true/false)</returns>*/
        public void ExecuteStoredProcedure(String procedureName, params Object[] queryParamList)
        {
            using (DbCommand cmd = BuildProcedureCommand(procedureName, queryParamList))
                cmd.ExecuteNonQuery();
        }

        public void ExecuteStoredProcedure<TSource>(TSource obj, String status)
        {
            Type objType = obj.GetType();
            String procedureName = objType.Name;
            using (DbCommand cmd = BuildProcedureCommand(procedureName))
            {
                IEnumerable<DbParameter> queryParamList = CreateDbParameterList(obj, status, cmd, false);
                foreach (DbParameter parameter in queryParamList)
                    cmd.Parameters.Add(parameter);

                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<String> ExecuteStoredProcedureWithOutput<TSource>(TSource obj, String status)
        {
            Type objType = obj.GetType();
            String procedureName = objType.Name;
            using (DbCommand cmd = BuildProcedureCommand(procedureName))
            {
                IEnumerable<DbParameter> queryParamList = CreateDbParameterList(obj, status, cmd, true);
                foreach (DbParameter parameter in queryParamList)
                    cmd.Parameters.Add(parameter);

                cmd.ExecuteNonQuery();

                IEnumerable<DbParameter> parameterOutputList = GetOutputParameterList(cmd.Parameters);
                foreach (DbParameter parameter in parameterOutputList)
                    yield return parameter.Value.ToString();
            }
        }

        private IEnumerable<DbParameter> CreateDbParameterList<TSource>(TSource obj, String status, DbCommand cmd, Boolean isUsingOutputParam)
        {
            Type objType = obj.GetType();
            PropertyInfo[] propertyList = objType.GetProperties();
            foreach (PropertyInfo property in propertyList)
            {
                DbParameter parameter = cmd.CreateParameter();
                parameter.ParameterName = "@" + property.Name;
                parameter.Value = property.GetValue(obj, null);
                if (isUsingOutputParam)
                    parameter.Direction = ParameterDirection.InputOutput;

                yield return parameter;
            }

            DbParameter statusParameter = cmd.CreateParameter();
            statusParameter.ParameterName = "@status";
            statusParameter.Value = status;

            yield return statusParameter;
        }

        private IEnumerable<DbParameter> GetOutputParameterList(IEnumerable dbParameterList)
        {
            foreach (DbParameter parameter in dbParameterList)
                if (parameter.Direction == ParameterDirection.InputOutput)
                    yield return parameter;
        }

        /*/// <summary>
        /// Function to ExecuteDataScallar
        /// </summary>
        /// <param name="_TSQL">query that you want to execute</param>
        /// <returns>object</returns>*/
        public Object ExecuteDataScalar(String queryString)
        {
            using (DbCommand cmd = BuildSqlCommand(queryString))
                return cmd.ExecuteScalar();
        }

        public Object ExecuteDataScalar(String queryString, params Object[] queryParamList)
        {
            using (DbCommand cmd = BuildSqlCommand(queryString, queryParamList))
                return cmd.ExecuteScalar();
        }

        public TResult ExecuteDataScalar<TResult>(String queryString)
        {
            Object result = ExecuteDataScalar(queryString);
            return result == DBNull.Value ? default(TResult) : (TResult)result;
        }

        public TResult ExecuteDataScalar<TResult>(String queryString, params Object[] queryParamList)
        {
            Object result = ExecuteDataScalar(queryString, queryParamList);
            return result == DBNull.Value ? default(TResult) : (TResult)result;
        }

        /*/// <summary>
        /// Function to ExecuteDataReader
        /// </summary>
        /// <param name="_TSQL">query that you want to execute</param>
        /// <returns>SqlDataReader</returns>*/
        public DbDataReader ExecuteDataReader(String queryString)
        {
            /*SqlCommand SQLCommand = new SqlCommand();
            SqlDataAdapter SqlClient = new SqlDataAdapter();
            SqlDataReader _DataValue;
            //Preparing Connection
            OpenConnection();
            SQLCommand.Connection = _Connection;
            SQLCommand.CommandText = _TSQL.Trim();
            if (_TimeOut > 0)
                SQLCommand.CommandTimeout = _TimeOut;
            else
                SQLCommand.CommandTimeout = 0;
            SQLCommand.CommandType = CommandType.Text;
            _DataValue = SQLCommand.ExecuteReader();
            CloseConnection();
            return _DataValue;*/
            using (DbCommand cmd = BuildSqlCommand(queryString))
                return cmd.ExecuteReader();
        }

        public DbDataReader ExecuteDataReader(String queryString, params Object[] queryParamList)
        {
            using (DbCommand cmd = BuildSqlCommand(queryString, queryParamList))
                return cmd.ExecuteReader();
        }

        public List<String> Insert<TSource>(TSource obj, String currentUser)
        {
            if (IsAuditTrailedObject(obj))
                SetAuditTrail(ref obj, currentUser, SPStatus.Insert);
            return CollectionExtension.ToList(ExecuteStoredProcedureWithOutput(obj, SPStatus.Insert));
        }

        public List<String> Update<TSource>(TSource obj, String currentUser)
        {
            if (IsAuditTrailedObject(obj))
                SetAuditTrail(ref obj, currentUser, SPStatus.Update);
            return CollectionExtension.ToList(ExecuteStoredProcedureWithOutput(obj, SPStatus.Update));
        }

        public List<String> Delete<TSource>(TSource obj, String currentUser)
        {
            if (IsAuditTrailedObject(obj))
                SetAuditTrail(ref obj, currentUser, SPStatus.Delete);
            return CollectionExtension.ToList(ExecuteStoredProcedureWithOutput(obj, SPStatus.Delete));
        }

        private Boolean IsAuditTrailedObject<TSource>(TSource obj)
        {
            return obj is IAuditTrail;
        }

        private void SetAuditTrail<TSource>(ref TSource obj, String currentUser, String spStatus)
        {
            var auditTrail = obj as IAuditTrail;
            switch (spStatus)
            {
                case SPStatus.Insert:
                    auditTrail.CreatedOn = DateTime.Now;
                    auditTrail.CreatedBy = currentUser;
                    break;
                case SPStatus.Delete:
                    auditTrail.IsDeleted = true;
                    auditTrail.IsActive = false;
                    break;
            }

            auditTrail.UpdatedOn = DateTime.Now;
            auditTrail.UpdatedBy = currentUser;
        }

        public Boolean IsExist<TResult>(IEnumerable<Condition> conditionList)
        {
            String tableName = String.Empty;
            Type tType = typeof (TResult);
            Object[] attributeList = tType.GetCustomAttributes(typeof (Table), false);
            if (attributeList.Length > 0)
            {
                var tableAttribute = (Table) attributeList[0];
                tableName = tableAttribute.TableName;
            }
            else
                tableName = tType.Name;

            String condition = CommonFunction.TrimStart(ObjectHandler.GenerateConditionToQuery(conditionList), "AND").Trim();
            String query = String.Format("USP_IS_EXIST {0}, {1}", tableName, condition);
            var isExist = (Boolean) ExecuteDataScalar(query);

            return isExist;
        }

        public Boolean IsExist<TResult>(Condition condition)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");

            var conditionList = new List<Condition>();
            conditionList.Add(condition);
            // TODO: check condition
            /*if (!String.IsNullOrEmpty(condition.ColumnName) &&
                !String.IsNullOrEmpty(condition.Connector) &&
                !String.IsNullOrEmpty(condition.Operator))
                conditionList.Add(condition);*/

            return IsExist<TResult>(conditionList);
        }

        public List<TResult> GetDataList<TResult>(IEnumerable<Condition> conditionList, Boolean isColumnAttributeAware = true)
        {
            String query = ObjectHandler.GetSelectQuery<TResult>(conditionList);
            DataTable targetedDataTable = ExecuteDataTable(query);

            if (isColumnAttributeAware)
                return CommonFunction.ConvertDataTableToListWithColumnAttributeAwareness<TResult>(targetedDataTable);

            return CommonFunction.ConvertDataTableToList<TResult>(targetedDataTable);
        }

        public List<TResult> GetDataList<TResult>(Condition condition, Boolean isColumnAttributeAware = true)
        {
            var conditionList = new List<Condition>();
            if (!String.IsNullOrEmpty(condition.ColumnName))
                conditionList.Add(condition);
            String query = ObjectHandler.GetSelectQuery<TResult>(conditionList);
            DataTable targetedDataTable = ExecuteDataTable(query);

            if (isColumnAttributeAware)
                return CommonFunction.ConvertDataTableToListWithColumnAttributeAwareness<TResult>(targetedDataTable);

            return CommonFunction.ConvertDataTableToList<TResult>(targetedDataTable);
        }

        public List<TResult> GetDataList<TResult>(String query, Boolean isColumnAttributeAware = true)
        {
            DataTable targetedDataTable = ExecuteDataTable(query);

            if (isColumnAttributeAware)
                return CommonFunction.ConvertDataTableToListWithColumnAttributeAwareness<TResult>(targetedDataTable);

            return CommonFunction.ConvertDataTableToList<TResult>(targetedDataTable);
        }

        public List<TResult> GetDataList<TResult>(String query, Boolean isColumnAttributeAware = true, params Object[] paramList)
        {
            DataTable targetedDataTable = ExecuteDataTable(query, paramList);

            if (isColumnAttributeAware)
                return CommonFunction.ConvertDataTableToListWithColumnAttributeAwareness<TResult>(targetedDataTable);

            return CommonFunction.ConvertDataTableToList<TResult>(targetedDataTable);
        }

        /*/// <summary>
        /// Function to ExecuteXMLReader
        /// </summary>
        /// <param name="_TSQL">query that you want to execute</param>
        /// <returns>XMLReader</returns>*/
        /*public XmlReader ExecuteXMLReader(string _TSQL)
        {
            SqlCommand SQLCommand = new SqlCommand();
            SqlDataAdapter SqlClient = new SqlDataAdapter();
            XmlReader _DataValue;
            //Preparing Connection
            OpenConnection();
            SQLCommand.Connection = _Connection;
            SQLCommand.CommandText = _TSQL.Trim();
            if (_TimeOut > 0)
                SQLCommand.CommandTimeout = _TimeOut;
            else
                SQLCommand.CommandTimeout = 0;
            SQLCommand.CommandType = CommandType.Text;
            _DataValue = SQLCommand.ExecuteXmlReader();
            CloseConnection();
            return _DataValue;
        }
        /// <summary>
        /// Function to StoredProcedure
        /// </summary>
        /// <param name="_StoredProcedure">name of store procedure</param>
        /// <param name="_ParameterString">string paramater</param>
        public void ExecuteStoredProcedure(string _StoredProcedure, string _ParameterString)
        {
            ExecuteNonQuery("EXEC " + _StoredProcedure + " " + _ParameterString);
        }*/
        /*/// <summary>
        /// Function to RunDTS
        /// </summary>
        /// <param name="_Provider">name of provider to run DTS</param>
        /// <param name="_Server">name of server to run DTS</param>
        /// <param name="_UserID">userid</param>
        /// <param name="_Password">password</param>
        /// <param name="_InitQuery">initquery</param>
        /// <param name="_SourceQuery">sourcequery</param>
        private void RunDTS(string _Provider, string _Server, string _UserID, string _Password, string _InitQuery, string _SourceQuery)
        {
            string SQL = ""+_InitQuery+" OPENROWSET ('" + _Provider + "','" + _Server + "';'" + _UserID + "';'" + _Password + "','" + _SourceQuery + "') as DS";
            ExecuteNonQuery(SQL);
        }
        /// <summary>
        /// Function to get DTSScript
        /// </summary>
        /// <param name="_Provider">name of provider to run DTS</param>
        /// <param name="_Server">name of server to run DTS</param>
        /// <param name="_UserID">userid</param>
        /// <param name="_Password">password</param>
        /// <param name="_InitQuery">initquery</param>
        /// <param name="_SourceQuery">sourcequery</param>
        /// <returns>string DTSScript</returns>
        private string DTSScript(string _Provider, string _Server, string _UserID, string _Password, string _InitQuery, string _SourceQuery)
        {
            string SQL = "" + _InitQuery + " OPENROWSET ('" + _Provider + "','" + _Server + "';'" + _UserID + "';'" + _Password + "','" + _SourceQuery + "') as DS";
            return SQL;
        }
        /// <summary>
        /// Function to run DTS with provider IBMDA400
        /// </summary>
        /// <param name="_Server">name of server to run DTS</param>
        /// <param name="_UserID">userid</param>
        /// <param name="_Password">password</param>
        /// <param name="_InitQuery">initquery</param>
        /// <param name="_SourceQuery">sourcequery</param>
        public void RunDTSForDB2AS400(string _Server, string _UserID, string _Password, string _InitQuery, string _SourceQuery)
        {
            RunDTS("IBMDA400", _Server, _UserID, _Password, _InitQuery, _SourceQuery);
        }
        /// <summary>
        /// Function to get DTSScript with IBMDA400 frovider
        /// </summary>
        /// <param name="_Provider">name of provider to run DTS</param>
        /// <param name="_Server">name of server to run DTS</param>
        /// <param name="_UserID">userid</param>
        /// <param name="_Password">password</param>
        /// <param name="_InitQuery">initquery</param>
        /// <param name="_SourceQuery">sourcequery</param>
        /// <returns>string DTSScript</returns>
        public string DTSForDB2AS400Script(string _Server, string _UserID, string _Password, string _InitQuery, string _SourceQuery)
        {
            return DTSScript("IBMDA400", _Server, _UserID, _Password, _InitQuery, _SourceQuery);
        }
        //#endregion
        #region SQLDMO Handling
        /// <summary>
        /// Function to check row that you want to use exist or not
        /// </summary>
        /// <param name="sSqlStrCmd">your query</param>
        /// <returns>true/false</returns>
        public bool IsRowExists(string sSqlStrCmd)
        {
            bool flag2 = false;
            try
            {
                DataSet ds = ExecuteDataSet(sSqlStrCmd, CommandType.Text);
                if (ds == null)
                    return flag2;
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count >= 1) 
                    flag2 =true;
                dt.Dispose();
                ds.Dispose();                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return flag2;
            }
            return true;
        }
        /// <summary>
        /// Function to check that database role exist or not
        /// </summary>
        /// <param name="DBName">your database name</param>
        /// <param name="RoleName">your role name</param>
        /// <returns>true/false</returns>
        public bool IsDatabaseRoleExists(string DBName, string RoleName)
        {
            string text1 = "USE "+DBName+";"+(char)13+""+(char)10+"";
            text1 = "" + text1 + " SELECT b.name FROM sysusers b WHERE b.name = '" + RoleName + "' AND b.uid=b.gid";
            return IsRowExists(text1);
        }
        /// <summary>
        /// Function to check that database exist or not
        /// </summary>
        /// <param name="DBName">your database name</param>
        /// <returns>true/false</returns>
        public bool IsDatabaseExists(string DBName)
        {
            string text1= ("SELECT * FROM master.dbo.sysdatabases WHERE name ='" + DBName + "'");
            return IsRowExists(text1);
        }
        /// <summary>
        /// Function to grant all access to database
        /// </summary>
        /// <param name="DBName">your database name</param>
        /// <param name="Credentials">credentials</param>
        /// <returns>true/false</returns>
        public bool GrantAllAccessToDatabase(string DBName, string Credentials)
        {
            if (!this.IsDatabaseExists(DBName))
                return false;
            string text1 = "USE " + DBName + ";" + (char)13 + "" + (char)10 + "";
            text1 = "" + text1 + "SELECT 'GRANT ALL ON ' + name + ' TO " + Credentials + "' FROM sysobjects where type in ('P','U','V')";
            //bool flag2=false;
            DataSet set1 = ExecuteDataSet(text1, CommandType.Text);
            if (set1 != null)
            {
                DataTable table1 = set1.Tables[0];
                text1 = "USE " + DBName + ";" + (char)13 + "" + (char)10 + "";
                int num2 = (table1.Rows.Count - 1),num1=0;
                do
                {
                    if (num1 == (table1.Rows.Count - 1))
                        text1 = "" + text1 + "" + table1.Rows[num1][0].ToString().Trim() + "; " + (char)13 + "" + (char)10 + "";
                    else
                        text1 = "" + text1 + "" + table1.Rows[num1][0].ToString().Trim() + "" + (char)13 + "" + (char)10 + "";
                    num1++;
                } while (num1 <= num2);
            }
            return Convert.ToBoolean(ExecuteNonQuery(text1));
        }
        /// <summary>
        /// Function to add database role
        /// </summary>
        /// <param name="DBName">your database name</param>
        /// <param name="RoleName">role that you want to add</param>
        /// <returns>true/false</returns>
        public bool AddDatabaseRole(string DBName, string RoleName)
        {
            bool flag2 = false;
            if (!IsDatabaseExists(DBName))
                return false;
            string text1 = "USE " + DBName + ";" + (char)13 + "" + (char)10 + "";
            try
            {
                if (!IsDatabaseRoleExists(DBName, RoleName))
                {
                    text1 = "" + text1 + "EXECUTE sp_addrole'" + RoleName + "'";
                    if (Convert.ToBoolean(ExecuteNonQuery(text1)))
                        this.GrantAllAccessToDatabase(DBName, RoleName);
                    return false;
                }
                flag2 = true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Function to check database user exist or not
        /// </summary>
        /// <param name="DBName">your database name</param>
        /// <param name="UserName">your username</param>
        /// <param name="RoleName">your rolename</param>
        /// <returns>true/false</returns>
        public bool IsDatabaseUserExists(string DBName, string UserName, string RoleName)
        {
            string text1 = "USE " + DBName + ";" + (char)13 + "" + (char)10 + "";
            if (RoleName == null)
                text1 = "" + text1 + "SELECT a.name from sysusers a where a.name = '" + UserName + "'";
            else
            {
                string[] textArray1 ={ text1, "SELECT a.name from sysusers a where a.name = '", UserName, "' AND a.gid IN ( SELECT b.uid FROM sysusers b WHERE b.name = '", RoleName, "' AND b.uid=b.gid ) " };
                text1 = String.Concat(textArray1);
            }
            return this.IsRowExists(text1);
        }
        /// <summary>
        /// Function to add user to database role
        /// <param name="DBName">your database name</param>
        /// <param name="UserName">your username</param>
        /// <param name="RoleName">your rolename</param>
        /// <returns>true/false</returns>
        public bool AddUserToDatabaseRole(string DBName, string UserName, string RoleName)
        {
            bool flag2 = false;
            string text1 = "USE " + DBName + ";" + (char)13 + "" + (char)10 + "";
            try
            {
                if (!IsDatabaseRoleExists(DBName, RoleName))
                    return flag2;
                if (this.IsDatabaseUserExists(DBName, UserName, RoleName))
                {
                    string[] textArray1 ={ text1, "EXECUTE sp_addrolemember '", RoleName, "','", UserName, "'" };
                    text1 = String.Concat(textArray1);
                    return Convert.ToBoolean(ExecuteNonQuery(text1));
                }
                flag2 = true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Function to add database user
        /// <param name="DBName">your database name</param>
        /// <param name="UserName">your username</param>
        /// <param name="RoleName">your rolename</param>
        /// <returns>true/false</returns>
        public bool AddDatabaseUser(string DBName, string UserName, string DefaultRole)
        {
            bool flag2 = false;
            string text1 = "USE " + DBName + ";" + (char)13 + "" + (char)10 + "";
            try
            {
                if(!this.IsDatabaseUserExists(DBName, UserName, null))
                {
                    if (!this.IsDatabaseExists(DBName))
                        return flag2;
                    string[] textArray1 ={ text1, "EXECUTE sp_grantdbaccess '", UserName, "','", UserName, "'" };
                    text1 = String.Concat(textArray1);
                    ExecuteNonQuery(text1);
                    if (DefaultRole != null)
                        this.AddUserToDatabaseRole(DBName, DefaultRole, UserName);
                    return true;
                }
                flag2 = true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Function to check SqlUser Exist or not
        /// </summary>
        /// <param name="sLoginName">LoginName</param>
        /// <param name="IsSysAdminRoleMember"></param>
        /// <returns>true/false</returns>
        public bool IsSqlUserExists(string sLoginName, bool IsSysAdminRoleMember)
        {
            string text1;
            if (IsSysAdminRoleMember)
                text1 = ("SELECT * FROM master.dbo.syslogins Where loginname = '" + sLoginName + "' AND sysadmin = 1 ");
            else
                text1 = ("SELECT * FROM master.dbo.syslogins Where loginname = '" + sLoginName + "'");
            return this.IsRowExists(text1);
        }
        /// <summary>
        /// Function to change login password
        /// </summary>
        /// <param name="UserName">your username</param>
        /// <param name="OldPassword">your old password</param>
        /// <param name="NewPassword">your new password</param>
        /// <returns>true/false</returns>
        public bool ChangeLoginPassword(string UserName, string OldPassword, string NewPassword)
        {
            string text1 = "";
            if (this.IsSqlUserExists(UserName, false))
            {
                string[] textArray1 ={ text1, "EXECUTE master.dbo.sp_password Null, '", NewPassword, "','", UserName, "'" };
                text1 = String.Concat(textArray1);
                ExecuteNonQuery(text1);
            }
            return false;
        }
        /// <summary>
        /// Function to CreateSQLUsers
        /// </summary>
        /// <param name="UserName">your username</param>
        /// <param name="UserPassword">your password</param>
        /// <returns>true/false</returns>
        public bool CreateSQLUsers(string UserName, string UserPassword)
        {
            string text1 = "";
            if (!this.IsSqlUserExists(UserName, false))
            {
                text1 = "" + text1 + " EXECUTE sp_addlogin '" + UserName + "','" + UserPassword + "', 'master'"+(char)13+""+(char)10+"";
                return Convert.ToBoolean(ExecuteNonQuery(text1));
            }
            return true;
        }
        /// <summary>
        /// Function to delete all database users
        /// </summary>
        /// <param name="DBName">your database name</param>
        /// <returns>true/false</returns>
        public bool DeleteAllDatabaseUsers(string DBName)
        {
            string text2 = "USE " + DBName + ";" + (char)13 + "" + (char)10 + "";
            text2 = "" + text2 + "SELECT name from sysusers where isSqlUser = 1 and name not in ('dbo','guest','sa')";
            bool flag2 = false;
            try
            {
                DataSet set1 = ExecuteDataSet(text2, CommandType.Text);
                if (set1 == null)
                    return flag2;
                DataTable table1 = set1.Tables[0];
                if (table1.Rows.Count > 0)
                {
                    string text1 = "USE " + DBName + ";" + (char)13 + "" + (char)10 + "";
                    int num2=table1.Rows.Count - 1,num1=0;
                    do{
                        if (num1 == (table1.Rows.Count - 1))
                            text1 = "" + text1 + " EXECUTE sp_dropuser '" + table1.Rows[num1][0].ToString().Trim() + "' ";
                        else
                            text1 = "" + text1 + "EXECUTE sp_dropuser '" + table1.Rows[num1][0].ToString().Trim() + "';"+(char)13+""+(char)10+"";
                        num1++;
                    }while(num1<=num2);
                    ExecuteNonQuery(text1);
                }
                table1.Dispose();
                flag2 = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Function to get list role member
        /// </summary>
        /// <param name="DBName">your database name</param>
        /// <param name="RoleName">your role name</param>
        /// <returns>datatable</returns>
        public DataTable ListRoleMember(string DBName, string RoleName)
        {
            DataTable table1, table2;
            string text1 = "USE " + DBName + ";" + (char)13 + "" + (char)10 + "";
            text1 = "" + text1 + "SELECT a.name from sysusers a where a.uid <> a.gid AND a.gid IN ( SELECT b.uid FROM sysusers b WHERE b.name = '" + RoleName + "' AND b.uid=b.gid ) ";
            try
            {
                DataSet set1 = ExecuteDataSet(text1, CommandType.Text);
                if (set1 != null)
                    table1 = set1.Tables[0];
                else
                    table1 = null;
                table2 = table1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            return table2;
        }
        /// <summary>
        /// Function to delete database role
        /// </summary>
        /// <param name="DBName">your database name</param>
        /// <param name="RoleName">your role name</param>
        /// <returns>true/false</returns>
        public bool DeleteDatabaseRole(string DBName, string RoleName)
        {
            bool flag2 = false;
            string text1 = "USE " + DBName + ";" + (char)13 + "" + (char)10 + "";
            try
            {
                if (this.IsDatabaseRoleExists(DBName, RoleName))
                {
                    DataTable table1 = this.ListRoleMember(DBName, RoleName);
                    if(table1!=null)
                    {
                        int num2 = (table1.Rows.Count - 1),num1=0;
                        do{
                            text1 = ""+text1+" EXECUTE sp_droprolemember '" + RoleName + "', '" + table1.Rows[num1][0].ToString().Trim() + "';"+(char)13+""+(char)10+"";
                            num1++;
                        } while (num1 <= num2);
                    }
                    text1 = ""+text1+" EXECUTE sp_droprole '" + RoleName+ "'";
                    ExecuteNonQuery(text1);
                    return flag2;
                }
                flag2 = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Function to delete database user
        /// </summary>
        /// <param name="DBName">your database name</param>
        /// <param name="UserName">your username</param>
        /// <returns>true/false</returns>
        public bool DeleteDatabaseUser(string DBName, string UserName)
        {
            bool flag2 = false;
            string text1 = "USE " + DBName + ";" + (char)13 + "" + (char)10 + "";
            try
            {
                if (this.IsDatabaseUserExists(DBName, UserName,null))
                {
                    text1 = ""+text1+" EXECUTE sp_revokedbaccess '" + UserName + "'";
                    ExecuteNonQuery(text1);
                    return true;
                }
                flag2 = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Function to delete SQL Users
        /// </summary>
        /// <param name="UserName">your username</param>
        /// <returns>true/false</returns>
        public bool DeleteSQLUsers(string UserName)
        {
            string text1 = "";
            if(!this.IsSqlUserExists(UserName, false))
            {
                text1 = ""+text1+" EXECUTE sp_droplogin '" + UserName + "'";
                return Convert.ToBoolean(ExecuteNonQuery(text1));
            }
            return true;
        }
        /// <summary>
        /// Function to delete user from database role
        /// </summary>
        /// <param name="DBName">your database name</param>
        /// <param name="UserName">your username</param>
        /// <param name="RoleName">rolename</param>
        /// <returns>true/false</returns>
        public bool DeleteUserFromDatabaseRole(string DBName, string UserName, string RoleName)
        {
            bool flag2 = false;
            string text1 = "USE " + DBName + ";" + (char)13 + "" + (char)10 + "";
            try
            {
                if (this.IsDatabaseUserExists(DBName, UserName, RoleName))
                {
                    string[] textArray1 ={ text1, "EXECUTE sp_droprolemember'", RoleName, "','", UserName, "'" };
                    text1 = String.Concat(textArray1);
                    ExecuteNonQuery(text1);
                    return true;
                }
                flag2 = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Function to check login to database valid or not
        /// </summary>
        /// <param name="DBName">your database name</param>
        /// <param name="UserName">your username</param>
        /// <returns>true/false</returns>
        public bool IsLoginValid(string DBName, string UserName)
        {
            string text1 = "USE " + DBName + ";" + (char)13 + "" + (char)10 + "";
            string[] textArray1 ={ text1, "SELECT * from sysusers a WHERE a.name ='", UserName, "' AND a.sid = (SELECT top 1 b.sid from master.dbo.syslogins b WHERE b.loginname='", UserName, "')" };
            text1 = String.Concat(textArray1);
            return this.IsRowExists(text1);
        }
        #endregion
        #region SWITCH Handling
        /// <summary>
        /// Function to switch DB to user config
        /// </summary>
        public void SwitchDBToUserConfig()
        {
            string SetConfig = cSec.DecryptTripleDes(System.Configuration.ConfigurationManager.AppSettings.Get("UserConfig"),false);
            if (GetParameterValue("cfgmode", SetConfig).ToLower() == "userconfig")
            {
                _UserID = GetParameterValue("User", SetConfig);      //User
                _Server = GetParameterValue("Server", SetConfig);    //Server
                _Database = GetParameterValue("Database", SetConfig); //Database
                _DataSource = GetParameterValue("Source", SetConfig); //Source
                if (_DataSource.Trim().Length == 0)
                    _DataSource = _Server;
                _Password = GetParameterValue("Password", SetConfig);                //Password
                _TimeOut = Convert.ToInt16(GetParameterValue("TimeOut", SetConfig)); //Timeout'
                _Transaction = GetParameterValue("globaltransact", SetConfig);
                UpdateConnection();
            }
            else
                Console.WriteLine("Error setting configuration");
        }
        /// <summary>
        /// Function to switch DB to extended user config
        /// </summary>
        /// <param name="_ConfigName">user config name</param>
        public void SwitchDBToExtendedConfig(string _ConfigName)
        {
            string SetConfig = cSec.DecryptTripleDes(System.Configuration.ConfigurationManager.AppSettings.Get(_ConfigName),false);
            if (GetParameterValue("cfgmode", SetConfig).ToLower() == ""+_ConfigName.ToLower()+"_")
            {
                _UserID = GetParameterValue("User", SetConfig);      //User
                _Server = GetParameterValue("Server", SetConfig);    //Server
                _Database = GetParameterValue("Database", SetConfig); //Database
                _DataSource = GetParameterValue("Source", SetConfig); //Source
                if (_DataSource.Trim().Length == 0)
                    _DataSource = _Server;
                _Password = GetParameterValue("Password", SetConfig);                //Password
                _TimeOut = Convert.ToInt16(GetParameterValue("TimeOut", SetConfig)); //Timeout'
                _Transaction = GetParameterValue("globaltransaction", SetConfig);
                UpdateConnection();
            }
            else
                Console.WriteLine("Error setting configuration");
        }*/
        /*/// <summary>
        /// Function to switch DB to application
        /// </summary>
        public void SwitchDBToAplication()
        {
            InitDatabase();
            UpdateConnection();
        }

        /// <summary>
        /// Function to switch DB to application
        /// </summary>

        /// <summary>
        /// Function that call SwitchDBToAplication() function
        /// </summary>
        public void Create()
        {
            SwitchDBToAplication();
        }
        #endregion*/
        
        /*public void New()
        {
            _Transaction="";
        }
        */
        public void Dispose()
        {
            CloseConnection();
            GC.SuppressFinalize(this);
        }
    }

    /*public class 

    public class Join
    {
        public 
    }*/

    public enum Connector
    {
        [StringValue("AND")] And,
        [StringValue("OR")] Or
    }

    public enum Operator
    {
        [StringValue("=")] Equal,
        [StringValue("<>")] NotEqual,
        [StringValue("<")] LessThan,
        [StringValue(">")] GreaterThan,
        [StringValue("<=")] LessThanEqual,
        [StringValue(">=")] GreaterThanEqual,
        [StringValue("IN")] In,
        [StringValue("NOT IN")] NotIn,
        [StringValue("IS")] Is,
        [StringValue("LIKE")] Like,
        [StringValue("BETWEEN")] Between
    }

    public class MSSQL<T>
    {
        private readonly MSSQL sequel = new MSSQL(new MSSQL.DefaultMSSQLConfiguration());

        private String[] columnList = null;
        private String tableName = String.Empty;
        private Boolean isDistinct = false;
        private List<Condition> conditionList = new List<Condition>();

        //private Join[] joinList = null;

        public MSSQL<T> Select(params String[] columnList)
        {
            this.columnList = columnList;
            return this;
        }

        public MSSQL<T> From(String tableName)
        {
            this.tableName = tableName;
            return this;
        }

        public MSSQL<T> Where(String column, Operator op, Object value, Connector conditionConnector)
        {
            conditionList.Add(new Condition(conditionConnector, column, op, value.ToString()));
            return this;
        }

        public MSSQL<T> Where(String column, Operator op, Object value)
        {
            Where(column, op, value, Connector.And);
            return this;
        }

        public MSSQL<T> And(String column, Operator op, Object value)
        {
            Where(column, op, value);
            return this;
        }

        public MSSQL<T> Or(String column, Operator op, Object value)
        {
            Where(column, op, value, Connector.Or);
            return this;
        }

        public MSSQL<T> WhereIn(String column, Object[] valueList)
        {
            Int32 valueListLength = valueList.Length;
            String[] valueArray = new String[valueListLength];
            for (int idx = 0; idx < valueListLength; idx++)
                valueArray[idx] = valueList[idx].ToString();


            conditionList.Add(new Condition(Connector.And, column, Operator.In, valueArray));
            return this;
        }

        //public MSSQL<T> OrWhereIn()

        public Boolean IsExist()
        {
            return sequel.IsExist<T>(conditionList);
        }

        public IEnumerable<T> Get()
        {
            return sequel.GetDataList<T>(conditionList);
        }

        public T GetSingle()
        {
            return CollectionExtension.FirstOrDefault(Get());
        }
    }
}
