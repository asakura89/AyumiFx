using System;
using System.Collections.Generic;

namespace WebApp {
    public sealed class JQueryDataTableDataFormat {
        public IList<JQueryDataTableColumn> ColumnNames { get; set; }
        public IList<String[]> DataRows { get; set; }
    }

    public sealed class JQueryDataTableColumn {
        public String title { get; set; }
    }
}