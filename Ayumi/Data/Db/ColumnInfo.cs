using System;
using Ayumi.Component;

namespace Ayumi.Data.Db {
    sealed class ColumnInfo {
        public ColumnAttribute Column { get; set; }
        public DataType Property { get; set; }
    }
}
