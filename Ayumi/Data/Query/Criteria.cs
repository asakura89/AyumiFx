using System;

namespace Ayumi.Data.Query {
    public class Criteria {
        public String FieldName{ get; set; }
        public CriteriaOperator Op{ get; set; }
        public Object Value{ get; set; }
        public CriteriaValueType Type{ get; set; }
        public CriteriaJunction Junction{ get; set; }
    }
}