using System.Collections.Generic;
using System.Linq;
using Ayumi.Component;
using Ayumi.Data.Db;

namespace Ayumi.Extension {
    static class ColumnInfoExt {
        public static IList<ColumnInfo> GetColumnInfos<T>(this T data) where T : class => data
            .GetType()
            .GetProperties()
            .Select(prop => new ColumnInfo {
                Column = prop.GetCustomAttributes(typeof(ColumnAttribute), false).Single() as ColumnAttribute,
                Property = new DataType(prop.Name, prop.GetValue(data, null), prop.PropertyType)
            })
            .ToList();
    }
}
