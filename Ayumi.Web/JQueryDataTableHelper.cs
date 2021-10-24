using System;
using System.Collections.Generic;
using System.Data;

namespace WebApp {
    public static class JQueryDataTableHelper {
        public static JQueryDataTableDataFormat GetJqueryDataTableDataFormat(DataTable dataTable) {
            var columns = new List<JQueryDataTableColumn>();
            foreach (DataColumn column in dataTable.Columns)
                columns.Add(new JQueryDataTableColumn {title = column.ColumnName});

            var rows = new List<String[]>();
            foreach (DataRow dr in dataTable.Rows) {
                var row = new String[dataTable.Columns.Count];
                System.Int32 index = 0;
                foreach (DataColumn col in dataTable.Columns)
                    row[index++] = dr[col].ToString();

                rows.Add(row);
            }

            return new JQueryDataTableDataFormat {ColumnNames = columns, DataRows = rows};
        }
    }
}