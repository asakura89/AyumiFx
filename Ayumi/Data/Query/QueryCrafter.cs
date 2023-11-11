using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ayumi.Extension;

namespace Ayumi.Data.Query {
    public interface IQueryComponent {
        IEnumerable<String> Columns { get; }
        IEnumerable<String> Tables { get; }
    }

    public interface IQueryCrafter {
        IColumnSelector Select(params String[] columns);
        IColumnSelector SelectAll();
        IInsert Insert();
        IInsert InsertInto();
        IUpdate Update();
        IDelete Delete();
        String Craft();
        String RawSql { get; }
        String ToString();
    }

    public interface IDelete {
    }

    public interface IUpdate {
    }

    public interface IInsert {
    }

    public interface IColumnSelector {
        IEnumerable<String> Columns { get; }
        ITableSelector From(params String[] tables);
    }

    public class ColumnSelector : IColumnSelector {
        public ColumnSelector(String[] columns) {
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));
            if (!columns.Any())
                throw new ArgumentOutOfRangeException(nameof(columns));
            if (columns.Any(column => column.Trim() == String.Empty))
                throw new InvalidOperationException("Invalid columns.");

            foreach (String column in columns)
                internalColumns.Add(column);
        }

        readonly IList<String> internalColumns = new List<String>();

        public IEnumerable<String> Columns => internalColumns;

        public ITableSelector From(params String[] tables) => new TableSelector(tables);
    }


    public interface ITableSelector {
        IEnumerable<String> Tables { get; }
        IPredicate Where(String predicate);
    }

    public class TableSelector : ITableSelector {
        public TableSelector(String[] tables) {
            if (tables == null)
                throw new ArgumentNullException(nameof(tables));
            if (!tables.Any())
                throw new ArgumentOutOfRangeException(nameof(tables));
            if (tables.Any(table => table.Trim() == String.Empty))
                throw new InvalidOperationException("Invalid tables.");

            foreach (String table in tables)
                internalTables.Add(table);
        }

        readonly IList<String> internalTables = new List<String>();

        public IEnumerable<String> Tables => internalTables;

        public IPredicate Where(String predicate) => new Predicate(predicate);
    }

    public interface IPredicate {
        IEnumerable<String> Predicates { get; }
        IPredicate AndWhere(String predicate);
        IPredicate OrWhere(String predicate);
    }

    public class Predicate : IPredicate {
        public Predicate(String predicate) {
            if (String.IsNullOrEmpty(predicate))
                throw new ArgumentNullException(nameof(predicate));

            internalPredicates.Add(predicate);
        }

        readonly IList<String> internalPredicates = new List<String>();

        public IEnumerable<String> Predicates => internalPredicates;

        public IPredicate AndWhere(String predicate) {
            internalPredicates.Add($"AND {predicate}");

            return this;
        }

        public IPredicate OrWhere(String predicate) {
            internalPredicates.Add($"OR {predicate}");

            return this;
        }
    }

    public class QueryCrafter : IQueryCrafter {
        

        //public SqlBuilder InsertInto(String tableName) {
        //    sql
        //        .Append("INSERT INTO ")
        //        .Append(tableName)
        //        .Append(" ");

        //    return this;
        //}

        //public SqlBuilder Update(String tableName) {
        //    sql
        //        .Append("UPDATE ")
        //        .Append(tableName)
        //        .Append(" ");

        //    return this;
        //}

        //public SqlBuilder Set(String innerString) {
        //    sql
        //        .Append("SET ")
        //        .Append(innerString)
        //        .Append(" ");

        //    return this;
        //}

        //public SqlBuilder Delete(String tableName) {
        //    sql
        //        .Append("DELETE ")
        //        .Append(tableName)
        //        .Append(" ");

        //    return this;
        //}

        //readonly IList<String> selectedColumns = new List<String>();
        //public QueryCrafter Select(params String[] columns) {
        //    if (columns == null)
        //        throw new ArgumentNullException(nameof(columns));
        //    if (!columns.Any())
        //        throw new ArgumentOutOfRangeException(nameof(columns));
        //    if (columns.Any(column => column.Trim() == String.Empty))
        //        throw new InvalidOperationException("Invalid columns.");

        //    foreach (String column in columns)
        //        selectedColumns.Add(column);

        //    return this;
        //}

        //public QueryCrafter SelectAll() {
        //    selectedColumns.Add("*");

        //    return this;
        //}

        //readonly IList<String> fromTables = new List<String>();
        //public QueryCrafter From(params String[] tables) {
        //    if (tables == null)
        //        throw new ArgumentNullException(nameof(tables));
        //    if (!tables.Any())
        //        throw new ArgumentOutOfRangeException(nameof(tables));
        //    if (tables.Any(table => table.Trim() == String.Empty))
        //        throw new InvalidOperationException("Invalid tables.");

        //    foreach (String table in tables)
        //        fromTables.Add(table);

        //    return this;
        //}

        //readonly IList<String> whereClauses = new List<String>();
        //public QueryCrafter Where(String whereClause) {
        //    if (String.IsNullOrEmpty(whereClause))
        //        throw new ArgumentNullException(nameof(whereClause));

        //    whereClauses.Add(whereClause);

        //    return this;
        //}

        //public QueryCrafter AndWhere(String whereClause) => Where($"AND {whereClause}");

        //public QueryCrafter OrWhere(String whereClause) => Where($"OR {whereClause}");

        //public SqlBuilder Bracket(String innerString) {
        //    sql
        //        .Append(" (")
        //        .Append(innerString)
        //        .Append(") ");

        //    return this;
        //}

        //public SqlBuilder Values(String innerString) {
        //    sql.Append("VALUES ");
        //    Bracket(innerString);
        //    return this;
        //}

        //public SqlBuilder Comma() {
        //    sql.Append(", ");
        //    return this;
        //}

        //public SqlBuilder And() {
        //    sql.Append("AND ");
        //    return this;
        //}

        //public SqlBuilder BeginTrans() {
        //    sql.Append("BEGIN TRAN ");
        //    return this;
        //}

        //public SqlBuilder CommitTrans() {
        //    sql.Append("COMMIT TRAN ");
        //    return this;
        //}

        //public SqlBuilder RollbackTrans() {
        //    sql.Append("ROLLBACK TRAN ");
        //    return this;
        //}

        public IColumnSelector Select(params String[] columns) => new ColumnSelector(columns);

        public IColumnSelector SelectAll() => new ColumnSelector(new[] { "*" });

        public IInsert Insert() => throw new NotImplementedException();

        public IInsert InsertInto() => throw new NotImplementedException();

        public IUpdate Update() => throw new NotImplementedException();

        public IDelete Delete() => throw new NotImplementedException();

        public String Craft() {
            var sql = new StringBuilder()
                .AppendLine("SELECT");

            //if (!selectedColumns.Any())
            //    throw new InvalidOperationException($"Query can't be crafted. Cause '{nameof(selectedColumns)}'.");

            //String columns = String.Join($",{Environment.NewLine}", selectedColumns);
            //sql.AppendLine(columns);

            //if (!fromTables.Any())
            //    throw new InvalidOperationException($"Query can't be crafted. Cause '{nameof(fromTables)}'.");

            //String tables = String.Join(Environment.NewLine, fromTables.Select(table => $"FROM {table}"));
            //sql.AppendLine(tables);

            //if (whereClauses.Any()) {
            //    String wheres = String.Join(Environment.NewLine, whereClauses).TrimStart("AND ").TrimStart("OR ");
            //    sql.AppendLine(wheres);
            //}

            

            return sql.ToString();
        }

        public String RawSql => Craft();

        public override String ToString() => Craft();
    }
}