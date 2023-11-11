using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WK.DBUtility;

namespace TestInterface
{
    [Serializable]
    [WkTable("WK_StockMutation")]
    public class StockMutationData
    {
        [WkField(DbFieldType.PRIMARY_KEY)]
        public string MutationId { get; set; }

        [WkField]
        public DateTime Date { get; set; }

        [WkField]
        public string Info { get; set; }

        [WkItemField]
        public List<Detail> Items { get; set; }

        public StockMutationData()
        {
            MutationId = "";
            Date = DateTime.Now;
            Info = "";
            Items = new List<Detail>();
        }

        public StockMutationData(string mutationId)
            : this()
        {
            MutationId = mutationId;
        }

        [Serializable]
        [WkTable("WK_StockMutationDetail")]
        public class Detail
        {
            [WkField(DbFieldType.PRIMARY_KEY)]
            public int StockId { get; set; }

            [WkField]
            public int MutationQty { get; set; }

            public int ProductId_J { get; set; }
            public string ProductName_J { get; set; }
        }
    }
}
