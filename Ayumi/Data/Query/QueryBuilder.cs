using System;
using System.Collections.Generic;

namespace Ayumi.Data.Query {
    public class QueryBuilder {
        public String defaultQuery = String.Empty;
        public String tableName = String.Empty;
        public List<String> fieldList = new List<String>();
        public List<Criteria> criteriaList = new List<Criteria>();
        public List<String> groupCriteriaList = new List<String>();
        public List<String> sortCriteriaList = new List<String>();
        public SortCriteriaType SortCriteriaType = SortCriteriaType.Asc;

        String GetOperator(CriteriaOperator op) {
            String opString = String.Empty;
            switch (op) {
                case CriteriaOperator.Eq:
                    opString = "=";
                    break;

                case CriteriaOperator.NE:
                    opString = "<>";
                    break;

                case CriteriaOperator.LT:
                    opString = "<";
                    break;

                case CriteriaOperator.LE:
                    opString = "<=";
                    break;

                case CriteriaOperator.GT:
                    opString = ">";
                    break;

                case CriteriaOperator.GE:
                    opString = ">=";
                    break;

                case CriteriaOperator.In:
                    opString = "IN";
                    break;

                case CriteriaOperator.Like:
                    opString = "LIKE";
                    break;
            }

            return opString;
        }

        String GetJunction(CriteriaJunction junction) {
            String junctionString = String.Empty;
            switch (junction) {
                case CriteriaJunction.And:
                    junctionString = "AND";
                    break;

                case CriteriaJunction.Or:
                    junctionString = "OR";
                    break;
            }

            return junctionString;
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

        String BuildSelectClause() {
            String builtQueryString = String.Empty;

            if (defaultQuery != String.Empty) {
                builtQueryString = defaultQuery;
            }
            else {
                if (fieldList.Count != 0) {
                    String selectString = String.Join(", ", fieldList.ToArray());

                    builtQueryString = String.Format("SELECT {0} FROM {1}", selectString, tableName);
                }
                else {
                    builtQueryString = String.Format("SELECT * FROM {0}", tableName);
                }
            }

            return builtQueryString;
        }

        String BuildWhereClause(String selectClause) {
            String criteriaString = String.Empty;
            foreach (Criteria criteria in criteriaList) {
                String opString = GetOperator(criteria.Op);
                String valueString = GetValue(criteria.Value, criteria.Type);
                String junctionString = GetJunction(criteria.Junction);
                criteriaString += String.Format("{0} {1} {2} {3} ", junctionString, criteria.FieldName, opString, valueString);
            }

            String builtQueryString = String.Format("{0} WHERE {1}", selectClause, criteriaString.Trim());

            return builtQueryString;
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

        public String BuildQueryString() {
            if (tableName == String.Empty && defaultQuery == String.Empty)
                throw new ArgumentException("tableName");

            String finalBuiltQueryString = BuildSelectClause();

            if (criteriaList.Count != 0)
                finalBuiltQueryString = BuildWhereClause(finalBuiltQueryString);
            if (groupCriteriaList.Count != 0)
                finalBuiltQueryString = BuildGroupByClause(finalBuiltQueryString);
            if (sortCriteriaList.Count != 0)
                finalBuiltQueryString = BuildOrderByClause(finalBuiltQueryString);

            return finalBuiltQueryString;
        }
    }
}