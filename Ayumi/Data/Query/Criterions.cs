using System;
using System.Collections.Generic;

namespace Ayumi.Data.Query {
    public class Criterions : List<Criteria> {
        Criterions(String columnName, CriteriaOp critOp, params Object[] values) {
            Add(new Criteria(columnName, critOp, values));
        }

        public Criterions And(String columnName, CriteriaOp critOp, params Object[] values) {
            Add(new Criteria(CriteriaJunction.And, columnName, critOp, values));
            return this;
        }

        public Criterions Or(String columnName, CriteriaOp critOp, params Object[] values) {
            Add(new Criteria(CriteriaJunction.Or, columnName, critOp, values));
            return this;
        }

        public static Criterions Where(String columnName, CriteriaOp critOp, params Object[] values) =>
            new Criterions(columnName, critOp, values);
    }
}
