using System;
using System.Collections.Generic;
using System.Text;

namespace Ayumi.Data.Query {
    public class Order {
        public String ColumnName { get; }
        public OrderDirection Direction { get; }

        public Order(String columnName, OrderDirection direction) {
            ColumnName = columnName;
            Direction = direction;
        }

        public override String ToString() => orderMap[Direction](ColumnName);

        readonly IDictionary<OrderDirection, Func<String, String>> orderMap = new Dictionary<OrderDirection, Func<String, String>> {
            [OrderDirection.Asc] = columnName => $"{columnName} ASC",
            [OrderDirection.Desc] = columnName => $"{columnName} DESC"
        };
    }
}
