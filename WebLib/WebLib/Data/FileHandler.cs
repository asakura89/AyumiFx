using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace WebLib.Data
{
    public static class FileHandler
    {
        #region ReadFile

        /// <summary>
        /// Return string: Read all Value in File
        /// </summary>
        /// <param name="pathFile">TextFile's Path</param>
        /// <returns>string</returns>
        public static string ReadFile(string pathFile)
        {
            return System.IO.File.ReadAllText(pathFile);
        }
        /// <summary>
        /// Return string[]: Read all Value Per Lines in File.
        /// </summary>
        /// <param name="pathFile">TextFile's Path</param>
        /// <returns>string[]</returns>
        public static string[] ReadAllLines(string pathFile)
        {
            return System.IO.File.ReadAllLines(pathFile);
        }
        /// <summary>
        /// Return List<string[]>: Read all Value Per Lines in File with every line value has delimiter.
        /// </summary>
        /// <param name="pathFile">TextFile's Path</param>
        /// <param name="delimiter">char splitter per column</param>
        /// <returns>List<string[]></returns>
        public static List<string[]> ReadAllLinesToListString(string pathFile, char delimiter)
        {
            string[] lines = ReadAllLines(pathFile);
            List<string[]> listValue = new List<string[]>();
            foreach (string line in lines)
            {
                string[] val = line.Split(delimiter);
                listValue.Add(val);
            }
            return listValue;
        }
        /// <summary>
        /// Return List<string[]>: Read all Value Per Lines in File.Type : Fixed Length. Fixed Length defined at first row
        /// </summary>
        /// <param name="pathFile">TextFile's path</param>
        /// <param name="delimiter">char splitter for first row(fixed length)</param>
        /// <param name="space">character for remain length that not use by the value.</param>
        /// <returns>List<string[]></returns>
        public static List<string[]> ReadAllLinesToListString(string pathFile, char delimiter,char space)
        {
            string[] lines = ReadAllLines(pathFile);
            List<int> listLength = GetListOfLength(lines[0].Split(delimiter));
            List<string[]> listValue = new List<string[]>();
            for (int i = 1; i < lines.Length - 1; i++)
            {
                List<string> value1Lines = GetStringOfOneLineReadLength(space, lines[i], listLength);
                listValue.Add(value1Lines.ToArray());
            }
            return listValue;
        }
        /// <summary>
        /// Return List<string[]>: Read all Value Per Lines in File.Type : Fixed Length. Fixed Length defined from user 
        /// </summary>
        /// <param name="pathFile">TextFile's Path</param>
        /// <param name="space">character for remain length that not use by the value.</param>
        /// <param name="listLength">Fixed Length per column</param>
        /// <returns>List<string[]></returns>
        public static List<string[]> ReadAllLinesToListString(string pathFile, char space,List<int> listLength)
        {
            string[] lines = ReadAllLines(pathFile);
            List<string[]> listValue = new List<string[]>();
            for (int i = 1; i < lines.Length - 1; i++)
            {
                List<string> value1Lines = GetStringOfOneLineReadLength(space, lines[i], listLength);
                listValue.Add(value1Lines.ToArray());
            }
            return listValue;
        }
        /// <summary>
        /// Return List<T>: Read all Value Per Lines in File.Type: Fixed Length. Retun List Of Object. 
        /// 2 Type column Defined : 1. defined column order in first line of file and take Length value from Attribut ColumnLength of property, 2. Column order By Attribut Index and read the attribut ColumnLength of property    
        /// </summary>
        /// <typeparam name="TSource">Type of Object</typeparam>
        /// <param name="pathFile">TextFile's Path</param>
        /// <param name="delimiter">char splitter per column</param>
        /// <param name="space">character for remain length that not use by the value.</param>
        /// <param name="isColumnDefined">Type Column Defined</param>
        /// <returns>List<T></returns>
        public static List<TSource> ReadAllLinesToListObject<TSource>(string pathFile, char delimiter, char space, bool isColumnDefined)
        {
            if (isColumnDefined)
                return ReadAllLinesToListObjectByColumn<TSource>(pathFile, delimiter, space);
            else
                return ReadAllLinesToListObjectByIndex<TSource>(pathFile, delimiter, space);
        }
        /// <summary>
        /// Return List<T>: Read all Value Per Lines in File.Type: delimiter. Retun List Of Object.
        /// Column split by delimiter
    /// </summary>
    /// <typeparam name="TSource">Type of Object</typeparam>
    /// <param name="pathFile">TextFile's Path</param>
    /// <param name="delimiter">char splitter per column</param>
        /// <param name="isColumnDefined">Type Column Defined </param>
        /// <returns>List<T></returns>
        public static List<TSource> ReadAllLinesToListObject<TSource>(string pathFile, char delimiter, bool isColumnDefined)
        {
            if (isColumnDefined)
                return ReadAllLinesToListObjectByColumn<TSource>(pathFile, delimiter);
            else
                return ReadAllLinesToListObjectByIndex<TSource>(pathFile, delimiter);
        }

        private static List<TSource> ReadAllLinesToListObjectByIndex<TSource>(string pathFile, char delimiter)
        {
            List<string[]> listVal = ReadAllLinesToListString(pathFile, delimiter);
            List<TSource> listObject = Activator.CreateInstance<List<TSource>>();
            List<ObjectRepoData> listProp = ObjectHandler.GetListObjectRepoFromObject<TSource>();
            foreach (string[] valArray in listVal)
            {
                TSource objData = SetObjectValue<TSource>(listProp, valArray, true);
                listObject.Add(objData);
            }
            return listObject;
        }
        
        private static List<TSource> ReadAllLinesToListObjectByColumn<TSource>(string pathFile, char delimiter, char space)
        {
            string[] listVal = ReadAllLines(pathFile);
            List<TSource> listObject = Activator.CreateInstance<List<TSource>>();
            List<ObjectRepoData> listProp = GetListObjectFromString(listVal[0].Split(delimiter));
            List<int> listLength = GetListLengthFromListProperty<TSource>(listProp);
            for(int i=1;i<listVal.Length;i++)
            {
                List<string> listValObject = GetStringOfOneLineReadLength(space, listVal[i], listLength);
                TSource objData = SetObjectValue<TSource>(listProp, listValObject.ToArray(), false);
                listObject.Add(objData);
            }
            return listObject;
        }

        private static List<TSource> ReadAllLinesToListObjectByIndex<TSource>(string pathFile, char delimiter,char space)
        {
           string[] listVal = ReadAllLines(pathFile);
            List<TSource> listObject = Activator.CreateInstance<List<TSource>>();
            List<ObjectRepoData> listProp = ObjectHandler.GetListObjectRepoFromObject<TSource>();
            List<int> listLength = GetListLengthFromListProperty<TSource>(listProp);
            foreach (string valArray in listVal)
            {
                List<string> listValObject = GetStringOfOneLineReadLength(space, valArray, listLength);
                TSource objData = SetObjectValue<TSource>(listProp, listValObject.ToArray(), true);
                listObject.Add(objData);
            }
            return listObject;
        }

        private static List<int> GetListLengthFromListProperty<TSource>(List<ObjectRepoData> listProp)
        {
            List<int> listLength = new List<int>();
            TSource objData = Activator.CreateInstance<TSource>();
            foreach (ObjectRepoData objRepo in listProp)
            {
                if (objRepo.Name.Equals(ObjectRepoConstant.TABLE_NAME))
                    continue;
                Type type = objData.GetType();
                PropertyInfo propInfo = type.GetProperty(objRepo.Name);
                Type returnType = propInfo.PropertyType;
                if ((returnType.IsGenericType &&
            returnType.GetGenericTypeDefinition() == typeof(List<>)))
                    continue;
                object[] attr = propInfo.GetCustomAttributes(typeof(ColumnAttribute), false);
                int columnLength = ((ColumnAttribute)attr[0]).ColumnLengthValue;
                listLength.Add(columnLength);
            }
            return listLength;
        }

        private static List<TSource> ReadAllLinesToListObjectByColumn<TSource> (string pathFile, char delimiter)
        {
            List<string[]> listVal = ReadAllLinesToListString(pathFile, delimiter);
            List<TSource> listObject = Activator.CreateInstance<List<TSource>>();
            List<ObjectRepoData> listAttribut = GetListObjectFromString(listVal[0]);
            for(int i =1; i< listVal.Count;i++)
            {
                TSource objData = SetObjectValue<TSource>(listAttribut, listVal[i],false);
                listObject.Add(objData);
            }
            return listObject;
        }

        private static List<ObjectRepoData> GetListObjectFromString(string[] arrObjectName)
        {
            List<ObjectRepoData> listObject = new List<ObjectRepoData>();
            foreach (string objName in arrObjectName)
            {
                ObjectRepoData objRepo = new ObjectRepoData();
                objRepo.Name = objName;
                objRepo.Type = null;
                listObject.Add(objRepo);
            }
            return listObject;
        }

        private static TSource SetObjectValue<TSource>(List<ObjectRepoData>listAttribut,string[] valArray, bool IsUseIndex)
        {
            if (IsUseIndex)
                return SetObjectValueByIndex<TSource>(listAttribut, valArray);
            else
                return SetObjectValueByColumnName<TSource>(listAttribut, valArray);
            
        }
 
        private static TSource SetObjectValueByColumnName<TSource>(List<ObjectRepoData> listAttribut, string[] valArray)
        {
            TSource objData = Activator.CreateInstance<TSource>();
            int i = 0;
            foreach (ObjectRepoData objRepo in listAttribut)
            {

                if (objRepo.Name.Equals(ObjectRepoConstant.TABLE_NAME))
                        continue;
                    Type type = objData.GetType();
                    PropertyInfo propInfo = type.GetProperty(objRepo.Name);
                    Type returnType = propInfo.PropertyType;
                    if ((returnType.IsGenericType &&
                returnType.GetGenericTypeDefinition() == typeof(List<>)))
                        continue;
                    propInfo.SetValue(objData, Convert.ChangeType(valArray[i], propInfo.PropertyType), null);
                    i++;
            }
            return objData;
        }

        private static TSource SetObjectValueByIndex<TSource>(List<ObjectRepoData> listAttribut, string[] valArray)
        {
            TSource objData = Activator.CreateInstance<TSource>();
            foreach (ObjectRepoData objRepo in listAttribut)
            {
                if (objRepo.Name.Equals(ObjectRepoConstant.TABLE_NAME))
                    continue;
                Type type = objData.GetType();
                PropertyInfo propInfo = type.GetProperty(objRepo.Name);
                Type returnType = propInfo.PropertyType;
                if ((returnType.IsGenericType &&
            returnType.GetGenericTypeDefinition() == typeof(List<>)))
                    continue;
                object[] attr = propInfo.GetCustomAttributes(typeof(ColumnAttribute), false);
                int colIndex = ((ColumnAttribute)attr[0]).ColumnIndex;
                propInfo.SetValue(objData, Convert.ChangeType(valArray[colIndex], propInfo.PropertyType), null);
            }
            return objData;
        }

        private static List<string> GetStringOfOneLineReadLength(char space, string lines, List<int> listLength)
        {
            int totalLength = GetTotalLength(listLength);
            List<string> value1Lines = new List<string>();
            int x = 0;
            foreach (int length in listLength)
            {
                string val = lines.Substring(x, length);
                val = val.Remove(val.IndexOf(space));
                value1Lines.Add(val);
                x += length;
            }
            return value1Lines;
        }

        private static List<int> GetListOfLength(string[] lines)
        {
            List<int> listLength = new List<int>();
            foreach (string line in lines)
            {
                int res;
                bool trys = int.TryParse(line, out res);
                if (!trys)
                    throw new Exception("Invalid Format");
                listLength.Add(Convert.ToInt32(line));
            }
            return listLength;
        }

        private static int GetTotalLength(List<int> listLength)
        {
            int totalLength = 0;
            foreach (int length in listLength)
            {
                totalLength+=Convert.ToInt32(length);
            }
            return totalLength;
        }
        #endregion

        #region WriteFile
        /// <summary>
        /// Write String[] To Text File 
        /// </summary>
        /// <param name="pathFile">TextFile's path</param>
        /// <param name="valueFile">Value to be written</param>
        public static void WriteFile(string pathFile, string[] valueFile)
        {

            System.IO.File.WriteAllLines(pathFile, valueFile);
        }
        /// <summary>
        /// Write string to TextFile
        /// </summary>
        /// <param name="pathFile">TextFile's path</param>
        /// <param name="valueFile">Value to be written</param>
        public static void WriteFile(string pathFile, string valueFile)
        {
            System.IO.File.WriteAllText(pathFile, valueFile);
        }
        /// <summary>
        /// Write List of Object to TextFile 
        /// </summary>
        /// <typeparam name="TSource">type Of Object</typeparam>
        /// <param name="listAttr">Value List of Object</param>
        /// <param name="pathFile">TextFile's Path</param>
        /// <param name="delimiter">if type= withLength then delimeter act as space. else delimiter act as char Splitter per Column</param>
        /// <param name="withLength">Type Write File </param>
        public static void WriteFile<TSource>(List<TSource> listAttr, string pathFile, char delimiter,bool withLength)
        {
            if(withLength)
                WriteFileWithLength<TSource>(listAttr,pathFile, delimiter);
            else
                WriteFileFromListObject<TSource>(listAttr,pathFile, delimiter);
        }
        /// <summary>
        /// Write File From DataTable To TextFile
        /// </summary>
        /// <param name="dt">Source DataTable</param>
        /// <param name="pathFile">Textfile's Path</param>
        /// <param name="delimiter">Char splitter per column</param>
        public static void WriteFile(DataTable dt, string pathFile, char delimiter)
        {
            List<string> listRow = new List<string>();
            for (int x=0; x< dt.Rows.Count ;x++)
            {
                string rows = string.Empty;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    rows += dt.Rows[x][i].ToString();
                    if (i != dt.Columns.Count - 1)
                        rows += delimiter;
                }
                listRow.Add(rows);
            }
            WriteFile(pathFile, listRow.ToArray());
        }

        private static void WriteFileWithLength<TSource>(List<TSource> listAttr, string pathFile, char emptyChar)
        {
            TSource objectData = Activator.CreateInstance<TSource>();
            List<string> listWriteVal = new List<string>();
            foreach (TSource obj in listAttr)
            {
                string valuFile = GetStringOfOneLine<TSource>(emptyChar, obj);
                listWriteVal.Add(valuFile);
            }
            WriteFile(pathFile, listWriteVal.ToArray());
        }

        private static void WriteFileFromListObject<TSource>(List<TSource> listAttr, string pathFile, char delimiter )
        {
            TSource objectData = Activator.CreateInstance<TSource>();
            List<string> listWriteVal = new List<string>();
            foreach (TSource obj in listAttr)
            {
                string valuFile = GetStringOfOneLineWithDelimiter<TSource>(delimiter, obj);
                listWriteVal.Add(valuFile);
            }
            WriteFile(pathFile, listWriteVal.ToArray());
        }

        private static string GetStringOfOneLine<TSource>(char emptyChar, TSource obj)
        {
            List<ObjectRepoData>listObject = ObjectHandler.GetListObjectRepoFromObject<TSource>();
            string valuFile = string.Empty;
            for (int i = 0; i < listObject.Count; i++)
            {
              
                string val = string.Empty;
                if (listObject[i].Name.Equals(ObjectRepoConstant.TABLE_NAME))
                    continue;
                PropertyInfo propInfo = obj.GetType().GetProperty(listObject[i].Name);
                Type returnType = propInfo.PropertyType;
                if ((returnType.IsGenericType &&
            returnType.GetGenericTypeDefinition() == typeof(List<>)))
                    continue;
                val += (string)propInfo.GetValue(obj, null);
                object[] attr = propInfo.GetCustomAttributes(typeof(ColumnAttribute), false);
                if (attr == null)
                    throw new Exception("Class Data must use attribute !");
                int attrLength = ((ColumnAttribute)attr[0]).ColumnLengthValue;
                if (val.Length > attrLength)
                    throw new Exception("Object Length is too long than Column Length");

                valuFile += FillEmptyString(val, (attrLength - val.Length), emptyChar);
            }
            return valuFile;
        }

        private static string FillEmptyString(string val, int length ,char empty)
        {
            for (int i = 0; i < length; i++)
            {
                val += empty;
            }
            return val;
        }

        private static string GetStringOfOneLineWithDelimiter<TSource>(char delimiter, TSource obj)
        {
            List<ObjectRepoData> listObject = ObjectHandler.GetListObjectRepoFromObject<TSource>();
            string valuFile = string.Empty;
            for (int i = 0; i < listObject.Count; i++)
            {
                if (listObject[i].Name.Equals(ObjectRepoConstant.TABLE_NAME))
                    continue;
                PropertyInfo propInfo = obj.GetType().GetProperty(listObject[i].Name);
                Type returnType = propInfo.PropertyType;
                if ((returnType.IsGenericType &&
            returnType.GetGenericTypeDefinition() == typeof(List<>)))
                    continue;
                valuFile += (string)propInfo.GetValue(obj, null);
                if (i != listObject.Count - 1)
                    valuFile += delimiter;

            }
            return valuFile;
        }

        #endregion
    }
}
