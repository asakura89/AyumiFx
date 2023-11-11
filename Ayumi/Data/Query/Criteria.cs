using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ayumi.Data.Query {
    public class Criteria {
        public CriteriaJunction Junction { get; }
        public String ColumnName { get; }
        public CriteriaOp CriteriaOp { get; }
        public IEnumerable<String> ParamAliases { get; }

        public Criteria(String columnName, CriteriaOp criteriaOperator, params String[] paramAliases) {
            Junction = CriteriaJunction.None;
            ColumnName = columnName;
            CriteriaOp = criteriaOperator;
            ParamAliases = paramAliases;
        }

        public Criteria(CriteriaJunction junction, String columnName, CriteriaOp criteriaOperator, params String[] paramAliases) {
            Junction = junction;
            ColumnName = columnName;
            CriteriaOp = criteriaOperator;
            ParamAliases = paramAliases;
        }

        public override String ToString() {
            var criteriaBuilder = new StringBuilder();
            criteriaBuilder.Append(junctionMap[Junction](ColumnName));
            criteriaBuilder.Append(operationMap[CriteriaOp](ParamAliases));

            return criteriaBuilder.ToString();
        }

        readonly IDictionary<CriteriaJunction, Func<String, String>> junctionMap = new Dictionary<CriteriaJunction, Func<String, String>> {
            [CriteriaJunction.None] = columnName => $" @{columnName} ",
            [CriteriaJunction.And] = columnName => $"AND @{columnName} ",
            [CriteriaJunction.Or] = columnName => $"OR @{columnName} "
        };

        readonly IDictionary<CriteriaOp, Func<IEnumerable<String>, String>> operationMap = new Dictionary<CriteriaOp, Func<IEnumerable<String>, String>> {
            [CriteriaOp.Eq] = aliases => $"= @{aliases.Single()} ",
            [CriteriaOp.NE] = aliases => $"<> @{aliases.Single()} ",
            [CriteriaOp.GT] = aliases => $"> @{aliases.Single()} ",
            [CriteriaOp.LT] = aliases => $"< @{aliases.Single()} ",
            [CriteriaOp.GE] = aliases => $">= @{aliases.Single()} ",
            [CriteriaOp.LE] = aliases => $"> @{aliases.Single()} ",
            [CriteriaOp.Like] = aliases => $"LIKE '%@{aliases.Single()}%' ",
            [CriteriaOp.In] = aliases => $"IN ({String.Join(", ", aliases)}) ", // NOTE: this is a bug
            [CriteriaOp.Between] = aliases => $"BETWEEN @{aliases.First()} AND @{aliases.Last()} "
        };
    }
}