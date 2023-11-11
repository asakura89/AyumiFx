using System.Collections.Generic;
using System.Text;

namespace WK.DBUtility
{
    public class SqlBuilder
    {
        private readonly StringBuilder sql = new StringBuilder();

        public override string ToString()
        {
            return sql.ToString();
        }

        public SqlBuilder InsertInto(string tableName)
        {
            sql.Append("INSERT INTO ");
            sql.Append(tableName);
            sql.Append(" ");
            return this;
        }

        public SqlBuilder Update(string tableName)
        {
            sql.Append("UPDATE ");
            sql.Append(tableName);
            sql.Append(" ");
            return this;
        }

        public SqlBuilder Set(string innerString)
        {
            sql.Append("SET ");
            sql.Append(innerString);
            sql.Append(" ");
            return this;
        }

        public SqlBuilder Delete(string tableName)
        {
            sql.Append("DELETE ");
            sql.Append(tableName);
            sql.Append(" ");
            return this;
        }

        public SqlBuilder Select(string selectClause)
        {
            sql.Append("SELECT ");
            sql.Append(selectClause);
            sql.Append(" ");
            return this;
        }

        public SqlBuilder SelectAllFields()
        {
            sql.Append("SELECT * ");
            return this;
        }

        public SqlBuilder From(string tableName)
        {
            sql.Append("FROM ");
            sql.Append(tableName);
            sql.Append(" ");
            return this;
        }

        public SqlBuilder Where(string whereClause)
        {
            sql.Append("WHERE ");
            sql.Append(whereClause);
            sql.Append(" ");
            return this;
        }

        public SqlBuilder Bracket(string innerString)
        {
            sql.Append(" (");
            sql.Append(innerString);
            sql.Append(") ");
            return this;
        }

        public SqlBuilder Values(string innerString)
        {
            sql.Append("VALUES ");
            Bracket(innerString);
            return this;
        }

        public SqlBuilder Comma()
        {
            sql.Append(", ");
            return this;
        }

        public SqlBuilder And()
        {
            sql.Append("AND ");
            return this;
        }

        public static string CombineWithComma(List<string> values)
        {
            return CombineString(values, ",").ToString();
        }

        public static string CombineWithAnd(List<string> values)
        {
            return CombineString(values, "AND").ToString();
        }

        private static StringBuilder CombineString(IList<string> stringList, string separator)
        {
            var combinedString = new StringBuilder();
            for (int index = 0; index < stringList.Count; index++)
            {
                combinedString.Append(stringList[index]);
                if (index < stringList.Count - 1) 
                    combinedString.Append(" " + separator + " ");
            }
            return combinedString;
        }
    }
}
