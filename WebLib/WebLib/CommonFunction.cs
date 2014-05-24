using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using WebLib.Data;

namespace WebLib
{
    public static class CommonFunction
    {
        /// <summary>
        /// Convert DateTime to string compatible for SQL
        /// </summary>
        /// <param name="dateTimeFormat">DateTime Value</param>
        /// <returns>string</returns>
        public static string ToSQLCompatibleFormat(DateTime dateTimeFormat)
        {
            return dateTimeFormat.ToString("yyyyMMdd HH:mm:ss");
        }

        public static string ConvertDateToString(DateTime date)
        {
            return date.ToString("dd-MMM-YYYY");
        }

        public static DateTime GetMinSQLDatetime()
        {
            return new DateTime(1753, 1, 1);
        }

        /// <summary>
        /// Convert DataTable To List of Object
        /// </summary>
        /// <typeparam name="TSource">type of Object</typeparam>
        /// <param name="dt">Data table to be converted</param>
        /// <returns>List<T></returns>
        public static List<TSource> ConvertDataTableToListObject<TSource>(DataTable dt)
        {
            List<TSource> listObject = new List<TSource>();
            TSource objectData = Activator.CreateInstance<TSource>();
            List<ObjectRepoData> listAttribut = ObjectHandler.GetListObjectRepoFromObject<TSource>();
            foreach (DataRow dr in dt.Rows)
            {
                TSource objData = Activator.CreateInstance<TSource>();
                foreach (ObjectRepoData objRepo in listAttribut)
                {
                    if (objRepo.Name.Equals(ObjectRepoConstant.TABLE_NAME))
                        continue;
                    Type type = objData.GetType();
                    PropertyInfo prop = type.GetProperty(objRepo.Name);
                    Type returnType = prop.PropertyType;
                    if ((returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(List<>)))
                        continue;

                    if (dr[prop.Name] != DBNull.Value)
                        prop.SetValue(objData, Convert.ChangeType(dr[objRepo.Name], prop.PropertyType), null);
                }
                listObject.Add(objData);
            }
            return listObject;
        }

        ///// <summary>
        ///// Get half UOBI key to decrypt string from config file
        ///// </summary>
        ///// <param name="appName">App name from config file</param>
        ///// <returns>half key</returns>
        //private static String GetHalfKeyFromRegistry(String appName)
        //{
        //    ApplicationRegistryHandler appReg = new ApplicationRegistryHandler();
        //    return appReg.ReadFromRegistry("Software\\" + appName, "Key");
        //}

        ///// <summary>
        ///// Get decrypted web config value by config name
        ///// </summary>
        ///// <param name="configName">config name</param>
        ///// <returns>decrypted config value</returns>
        //public static String GetWebConfigValue(String configName)
        //{
        //    Encryptor encryptor = new Encryptor();

        //    String encryptedConfig = ConfigurationManager.AppSettings[configName].ToString();
        //    String appName = ConfigurationManager.AppSettings[AppConstant.APP_NAME].ToString().Trim();
        //    String uobiKey = ConfigurationManager.AppSettings[AppConstant.UOBI_KEY].ToString();

        //    String regKey = CommonFunction.GetHalfKeyFromRegistry(appName);
        //    String decryptedConfig = encryptor.Decrypt(encryptedConfig, regKey, uobiKey);

        //    return decryptedConfig;
        //}

        public static void PopulateRawListToListBox<T>(ref ListBox listbox, List<T> dataList, String display, String value)
        {
            listbox.Items.Clear();
            listbox.DataSource = null;

            listbox.DataTextField = display;
            listbox.DataValueField = value;
            listbox.DataSource = dataList;
            listbox.DataBind();
        }

        public static void PopulateListToListBox<T>(ref ListBox listbox, List<T> dataList, String display, String value)
        {
            T newT = Activator.CreateInstance<T>();
            dataList.Insert(0, newT);

            PopulateRawListToListBox(ref listbox, dataList, display, value);
        }

        public static void PopulateRawListToDropdown<T>(ref DropDownList dropdown, List<T> dataList, String display, String value)
        {
            dropdown.Items.Clear();
            dropdown.DataSource = null;

            dropdown.DataTextField = display;
            dropdown.DataValueField = value;
            dropdown.DataSource = dataList;
            dropdown.DataBind();
        }

        public static void PopulateListToDropdown<T>(ref DropDownList dropdown, List<T> dataList, String display, String value)
        {
            T newT = Activator.CreateInstance<T>();
            dataList.Insert(0, newT);

            PopulateRawListToDropdown(ref dropdown, dataList, display, value);
        }

        public static void PopulateRawDataTableToDropdown(ref DropDownList dropdown, DataTable dt, String display, String value)
        {
            dropdown.Items.Clear();
            dropdown.DataSource = null;

            dropdown.DataTextField = display;
            dropdown.DataValueField = value;
            dropdown.DataSource = dt;
            dropdown.DataBind();
        }

        public static void PopulateDataTableToComboBox(ref DropDownList dropdown, DataTable dt, String display, String value)
        {
            DataRow emptyRow = dt.NewRow();
            Int32 colCount = dt.Columns.Count;
            for (int i = 0; i < colCount; i++)
            {
                String currentColumnType = dt.Columns[i].DataType.Name;
                switch (currentColumnType)
                {
                    case "Boolean":
                        emptyRow[i] = false;
                        break;
                    case "Char":
                        emptyRow[i] = Char.MinValue;
                        break;
                    case "DateTime":
                        emptyRow[i] = DateTime.MinValue;
                        break;
                    case "String":
                        emptyRow[i] = String.Empty;
                        break;
                    case "TimeSpan":
                        emptyRow[i] = new TimeSpan(0);
                        break;
                    case "Byte":
                    case "SByte":
                    case "Int16":
                    case "Int32":
                    case "Int64":
                    case "UInt16":
                    case "UInt32":
                    case "UInt64":
                        emptyRow[i] = 0;
                        break;
                    case "Decimal":
                    case "Single":
                    case "Double":
                        emptyRow[i] = 0.0;
                        break;
                }
            }
            dt.Rows.InsertAt(emptyRow, 0);

            PopulateRawDataTableToDropdown(ref dropdown, dt, display, value);
        }

        public static void PopulateListToRepeater<T>(ref Repeater repeater, List<T> dataList)
        {
            repeater.DataSource = null;

            repeater.DataSource = dataList;
            repeater.DataBind();
        }
    }
}
