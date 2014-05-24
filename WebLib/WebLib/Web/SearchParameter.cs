using System;

namespace WebLib.Web
{
    public class SearchParameter
    {
        public String columnName;
        public String searchOperator;
        public String keyword;

        public SearchParameter() : this(String.Empty, String.Empty, String.Empty) { }

        public SearchParameter(String columnName, String searchOperator, String keyword)
        {
            this.columnName = columnName;
            this.searchOperator = searchOperator;
            this.keyword = keyword;
        }
    }
}