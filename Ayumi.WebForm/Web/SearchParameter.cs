using System;
using WebLib.Data;

namespace WebLib.Web
{
    public class SearchParameter
    {
        public String columnName;
        public Operator searchOperator;
        public String keyword;

        public SearchParameter() : this("1", Operator.Equal, "1") { }

        public SearchParameter(String columnName, Operator searchOperator, String keyword)
        {
            this.columnName = columnName;
            this.searchOperator = searchOperator;
            this.keyword = keyword;
        }
    }
}