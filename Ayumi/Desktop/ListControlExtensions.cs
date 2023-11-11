using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Ayumi.Desktop
{
    public static class ListControlExtensions
    {
        /*private static void ClearItemList(this ListControl listControl)
        {
            if (listControl is ComboBox)
                ((ComboBox)listControl).Items.Clear();
            if (listControl is ListBox)
                ((ListBox)listControl).Items.Clear();
            if (listControl is CheckedListBox)
                ((CheckedListBox)listControl).Items.Clear();

            listControl.DataSource = null;
        }*/

        private static void SetItemList(this ListControl listControl, IEnumerable dataList, String display, String value)
        {
            listControl.DisplayMember = display;
            listControl.ValueMember = value;
            listControl.DataSource = dataList;
        }

        private static void SetItemList(this ListControl listControl, DataTable dataTable, String display, String value)
        {
            listControl.DisplayMember = display;
            listControl.ValueMember = value;
            listControl.DataSource = dataTable;
        }

        public static void PopulateWithRawDataList<T>(this ListControl listControl, IList<T> dataList, String display, String value)
        {
            //ClearItemList(listControl);
            listControl.DataSource = null;
            SetItemList(listControl, dataList, display, value);
        }

        public static void PopulateWithRawDataList(this ListControl listControl, DataTable dataTable, String display, String value)
        {
            //ClearItemList(listControl);
            listControl.DataSource = null;
            SetItemList(listControl, dataTable, display, value);
        }

        public static void PopulateWithDataList<T>(this ListControl listControl, IList<T> dataList, String display, String value)
        {
            var newT = Activator.CreateInstance<T>();
            dataList.Insert(0, newT);
            //ClearItemList(listControl);
            listControl.DataSource = null;
            SetItemList(listControl, dataList, display, value);
        }

        public static void PopulateWithDataList(this ListControl listControl, DataTable dataTable, String display, String value)
        {
            DataRow emptyRow = dataTable.NewRow();
            Int32 colCount = dataTable.Columns.Count;
            for (int i = 0; i < colCount; i++)
            {
                String currentColumnType = dataTable.Columns[i].DataType.Name;
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
            dataTable.Rows.InsertAt(emptyRow, 0);

            //ClearItemList(listControl);
            listControl.DataSource = null;
            SetItemList(listControl, dataTable, display, value);
        } 
    }
}