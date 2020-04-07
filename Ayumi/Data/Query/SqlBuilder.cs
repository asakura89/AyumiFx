using System;
using System.Collections.Generic;
using System.Text;

namespace Ayumi.Data.Query {
    public class SqlBuilder {
        readonly StringBuilder sql = new StringBuilder();

        public override String ToString() => sql.ToString();

        public SqlBuilder InsertInto(String tableName) {
            sql
                .Append("INSERT INTO ")
                .Append(tableName)
                .Append(" ");

            return this;
        }

        public SqlBuilder Update(String tableName) {
            sql
                .Append("UPDATE ")
                .Append(tableName)
                .Append(" ");

            return this;
        }

        public SqlBuilder Set(String innerString) {
            sql
                .Append("SET ")
                .Append(innerString)
                .Append(" ");

            return this;
        }

        public SqlBuilder Delete(String tableName) {
            sql
                .Append("DELETE ")
                .Append(tableName)
                .Append(" ");

            return this;
        }

        public SqlBuilder Select(String selectClause) {
            sql
                .Append("SELECT ")
                .Append(selectClause)
                .Append(" ");

            return this;
        }

        public SqlBuilder SelectAll() {
            sql.Append("SELECT * ");
            return this;
        }

        public SqlBuilder From(String tableName) {
            sql
                .Append("FROM ")
                .Append(tableName)
                .Append(" ");

            return this;
        }

        public SqlBuilder Where(String whereClause) {
            sql
                .Append("WHERE ")
                .Append(whereClause)
                .Append(" ");

            return this;
        }

        public SqlBuilder Bracket(String innerString) {
            sql
                .Append(" (")
                .Append(innerString)
                .Append(") ");

            return this;
        }

        public SqlBuilder Values(String innerString) {
            sql.Append("VALUES ");
            Bracket(innerString);
            return this;
        }

        public SqlBuilder Comma() {
            sql.Append(", ");
            return this;
        }

        public SqlBuilder And() {
            sql.Append("AND ");
            return this;
        }

        public static String CombineWithComma(IEnumerable<String> values) => String.Join(", ", values);

        public static String CombineWithAnd(IEnumerable<String> values) => String.Join(" AND ", values);
    }
}