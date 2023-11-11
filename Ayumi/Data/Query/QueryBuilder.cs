using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ayumi.Data.Query {
    public class QueryBuilder {
        String tableName = String.Empty;
        readonly IList<String> columns = new List<String>();
        readonly IList<Criteria> criterions = new List<Criteria>();
        readonly IList<Order> orders = new List<Order>();
        readonly IList<String> groups = new List<String>();

        public QueryBuilder Select() {
            columns.Add("*");
            return this;
        }

        public QueryBuilder Select(params String[] columns) {
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));
            if (!columns.Any())
                throw new ArgumentException(nameof(columns));

            foreach (String column in columns)
                this.columns.Add(column);

            return this;
        }

        public QueryBuilder From(String tableName) {
            this.tableName = tableName;
            return this;
        }

        public QueryBuilder Where(String columnName, CriteriaOp critOp, params Object[] values) =>
            Where(Criterions.Where(columnName, critOp, values));

        public QueryBuilder Where(IEnumerable<Criteria> criterions) {
            if (criterions == null)
                throw new ArgumentNullException(nameof(criterions));
            if (!criterions.Any())
                throw new ArgumentException(nameof(criterions));

            foreach (Criteria criteria in criterions)
                this.criterions.Add(criteria);

            return this;
        }

        public QueryBuilder OrderBy(String columnName, OrderDirection direction) {
            if (direction == OrderDirection.Asc)
                return OrderBy(Orders.OrderBy(columnName));

            return OrderBy(Orders.OrderByDescending(columnName));
        }

        public QueryBuilder OrderBy(IEnumerable<Order> orders) {
            if (orders == null)
                throw new ArgumentNullException(nameof(orders));
            if (!orders.Any())
                throw new ArgumentException(nameof(orders));

            foreach (Order order in orders)
                this.orders.Add(order);

            return this;
        }
        
        String GetValue(Object value, CriteriaValueType type) {
            String valueString = String.Empty;
            switch (type) {
                case CriteriaValueType.String:
                    valueString = String.Format("'{0}'", value);
                    break;

                case CriteriaValueType.Integer:
                    valueString = value.ToString();
                    break;

                case CriteriaValueType.DateTime:
                    throw new NotImplementedException();
                    break;
            }

            return valueString;
        }

        public String BuildQueryString() {
            if (tableName == String.Empty)
                throw new ArgumentException("tableName");

            var queryString = new StringBuilder(BuildSelectClause());

            if (criterions.Any())
                queryString.Append(BuildWhereClause());
            if (groups.Any())
                queryString.Append(BuildGroupByClause());
            if (orders.Any())
                queryString.Append(BuildOrderByClause());

            return queryString.ToString();
        }

        String BuildSelectClause() {
            var built = new StringBuilder()
                .Append("SELECT ");

            if (columns.Any())
                built
                    .Append(String.Join(", ", columns))
                    .Append(" ");
            else
                built.Append("* ");

            built.AppendFormat("FROM {0}", tableName);
            return built.ToString();
        }

        String BuildWhereClause() {
            var built = new StringBuilder()
                .Append("WHERE ");
            foreach (Criteria criteria in criterions)
                built.Append(criteria);

            return built.ToString();
        }

        String BuildGroupByClause(String selectClause) {
            String groupCriteriaString = String.Join(", ", groupCriteriaList.ToArray());
            String builtQueryString = String.Format("{0} GROUP BY {1}", selectClause, groupCriteriaString);

            return builtQueryString;
        }

        String BuildOrderByClause(String selectClause) {
            String sortCriteriaString = String.Join(", ", sortCriteriaList.ToArray());
            String builtQueryString = String.Format("{0} ORDER BY {1}", selectClause, sortCriteriaString);

            return builtQueryString;
        }
    }
}