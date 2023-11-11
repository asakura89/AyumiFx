using System;
using System.Collections.Generic;

namespace Ayumi.Data.Query {
    public class Orders : List<Order> {
        Orders(String columnName, OrderDirection direction) {
            Add(new Order(columnName, direction));
        }

        public Orders ThenBy(String columnName) {
            Add(new Order(columnName, OrderDirection.Asc));
            return this;
        }

        public Orders ThenByDescending(String columnName) {
            Add(new Order(columnName, OrderDirection.Desc));
            return this;
        }

        public static Orders OrderBy(String columnName) =>
            new Orders(columnName, OrderDirection.Asc);

        public static Orders OrderByDescending(String columnName) =>
            new Orders(columnName, OrderDirection.Desc);
    }
}
