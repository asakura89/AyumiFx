using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;

namespace WebLib
{
    public class MSAccess
    {
        OleDbConnection conn = new System.Data.OleDb.OleDbConnection();

        public MSAccess()
        {
            string connTemp = ConfigurationSettings.AppSettings["ConnectionString"].ToString();
            string path = Directory.GetCurrentDirectory();
            int find1 = connTemp.IndexOf("Data Source=");

            if (find1 != -1)
            {
                int find2 = connTemp.IndexOf(";", find1);
                string test = connTemp.Substring(find1, find2 - find1);
                string dbName = test.Substring(test.IndexOf("=", 0) + 1, (test.Length - (test.IndexOf("=", 0) + 1)));
                connTemp = connTemp.Replace(@connTemp.Substring(find1, find2 - find1), "Data Source=" + path + dbName);
                conn.ConnectionString = @connTemp;
            }
            else
            {
                throw new Exception("Parameter koneksi database 'Data Source' tidak ditemukan. Silahkan periksa kembali konfigurasi aplikasi.");
            }
        }

        /// <summary>
        /// Open connection to Database.
        /// </summary>
        public void OpenConnection()
        {             
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured in connecting to database : Failed to connect to data source. " + ex.Message);
            }
        }

        /// <summary>
        /// Close current connection.
        /// </summary>
        public void CloseConnection()
        {             
            try
            {
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured in connecting to database : Failed to connect to data source. " + ex.Message);
            }
        }
        
        /// <summary>
        /// Execute query.
        /// </summary>
        /// <param name="_TableOrVieworSQL">Query that can be Sql Query, Table or View name</param>
        /// <param name="_CmdType">Type of command to be excuted</param>
        /// <returns>DataTable that returned by the executed query</returns>
        public DataTable ExecuteDataTable(string _TableOrVieworSQL, CommandType _CmdType)
        {
            OleDbCommand SQLCommand = new OleDbCommand();
            OleDbDataAdapter SQLAdapter = new OleDbDataAdapter();
            DataTable _DataTable = new DataTable();
            //Preparing Connection
            OpenConnection();
            SQLCommand.Connection = conn;
            if (_CmdType == CommandType.TableDirect)
                SQLCommand.CommandText = "Select * From " + _TableOrVieworSQL + "";
            else if (_CmdType == CommandType.Text)
                SQLCommand.CommandText = _TableOrVieworSQL;

            SQLCommand.CommandType = CommandType.Text;
            //Preparing Adapter
            SQLAdapter.SelectCommand = SQLCommand;
            SQLAdapter.Fill(_DataTable);
            CloseConnection();
            return _DataTable;
        }
        
        /// <summary>
        /// Function to get DataSet from Tabel, View, T-SQL
        /// </summary>
        /// <param name="_TableOrViewOrQueryOrSP">name of table or view</param>
        /// <param name="_CmdType">cmdType</param>
        /// <returns>dataset</returns>
        public DataSet ExecuteDataSet(string _TableOrViewOrQueryOrSP, CommandType _CmdType)
        {
            OleDbCommand SQLCommand = new OleDbCommand();
            OleDbDataAdapter SQLAdapter = new OleDbDataAdapter();
            DataSet _DataSet = new DataSet();
            //Preparing Connection
            OpenConnection();
            SQLCommand.Connection = conn;
            SQLCommand.CommandText = _TableOrViewOrQueryOrSP;

            if (_CmdType == CommandType.StoredProcedure)
                SQLCommand.CommandType = CommandType.StoredProcedure;
            else
            {
                SQLCommand.CommandType = CommandType.Text;
                if (_CmdType == CommandType.TableDirect)
                    SQLCommand.CommandText = "SELECT * FROM " + _TableOrViewOrQueryOrSP + "";
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
            OleDbCommand SQLCommand = new OleDbCommand();
            OleDbDataAdapter SQLAdapter = new OleDbDataAdapter();
            DataSet _DataSet = new DataSet();
            //Preparing Connection
            OpenConnection();
            SQLCommand.Connection = conn;
            SQLCommand.CommandText = _TableOrViewOrQueryOrSP;
            SQLCommand.Parameters.Clear();

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

        /// <summary>
        /// Execute a non query 
        /// </summary>
        /// <param name="_Query"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string _Query)
        {
            OleDbCommand SQLCommand = new OleDbCommand();
            DataSet _DataSet = new DataSet();
            int _ReturnValue = 0;
            //Preparing Connection
            OpenConnection();
            SQLCommand.CommandType = CommandType.Text;
            SQLCommand.Connection = conn;
            SQLCommand.CommandText = _Query;

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
            OleDbCommand SQLCommand = new OleDbCommand();
            DataSet _DataSet = new DataSet();
            int _ReturnValue = 0;
            //Preparing Connection
            OpenConnection();
            SQLCommand.CommandType = CommandType.Text;
            SQLCommand.Connection = conn;

            for (int i = 0; i <= _Query.GetUpperBound(0); i++)
            {
                SQLCommand.CommandText = _Query[i];
                _ReturnValue = SQLCommand.ExecuteNonQuery();
            }
            //Preparing Adapter
            CloseConnection();
            return _ReturnValue;
        }
       
        /// <summary>
        /// Function to ExecuteStoredProcedure
        /// </summary>
        /// <param name="_StoredProcedure">name of storedprocedure</param>
        /// <param name="_Parameter">paramater</param>
        /// <returns>1/0(true/false)</returns>
        public int ExecuteStoredProcedure(string _StoredProcedure, SqlParameter[] _Parameter)
        {
            OleDbCommand SQLCommand = new OleDbCommand();
            DataSet _DataSet = new DataSet();
            int _ReturnValue = 0;
            //Preparing Connection           
            OpenConnection();
            SQLCommand.Connection = conn;
            SQLCommand.CommandText = _StoredProcedure;
            SQLCommand.CommandType = CommandType.StoredProcedure;

            string paramLast = "";
            if (_Parameter.GetUpperBound(0) > 0)
            {
                for (int i = 0; i <= _Parameter.GetUpperBound(0); i++)
                {
                    SQLCommand.Parameters.Add(_Parameter[i]);
                    paramLast += "'" + _Parameter[i].Value.ToString() + "',";
                }
            }
            object obj = SQLCommand.ExecuteReader();
            //_ReturnValue = SQLCommand.ExecuteReader();
            //Return Value for OUTPUT parameter
            for (int i = 0; i <= _Parameter.GetUpperBound(0); i++)
            {
                //_Parameter[i].Value = SQLCommand.Parameters.Item(_Parameter[i].ParameterName).Value;
                _Parameter[i].Value = SQLCommand.Parameters[_Parameter[i].ParameterName].Value;
            }
            //Preparing Adapter
            CloseConnection();
            return _ReturnValue;
        }
       
        /// <summary>
        /// Function to ExecuteDataScallar
        /// </summary>
        /// <param name="_TSQL">query that you want to execute</param>
        /// <returns>object</returns>
        public object ExecuteDataScallar(string _TSQL)
        {
            OleDbCommand SQLCommand = new OleDbCommand();
            OleDbDataAdapter SqlClient = new OleDbDataAdapter();
            object _DataValue = new object();
            //Preparing Connection
            OpenConnection();
            SQLCommand.Connection = conn;

            SQLCommand.CommandType = CommandType.Text;
            _DataValue = SQLCommand.ExecuteScalar();
            CloseConnection();
            return _DataValue;
        }
       
        /// <summary>
        /// Function to ExecuteDataReader
        /// </summary>
        /// <param name="_TSQL">query that you want to execute</param>
        /// <returns>SqlDataReader</returns>
        public OleDbDataReader ExecuteDataReader(string _TSQL)
        {
            OleDbCommand SQLCommand = new OleDbCommand();
            OleDbDataAdapter SqlClient = new OleDbDataAdapter();
            OleDbDataReader _DataValue;
            //Preparing Connection
            OpenConnection();
            SQLCommand.Connection = conn;
            SQLCommand.CommandText = _TSQL.Trim();

            SQLCommand.CommandType = CommandType.Text;
            _DataValue = SQLCommand.ExecuteReader();
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
        }
    }
}
