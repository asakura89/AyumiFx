using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WK.DBUtility;

namespace TestInterface
{
    [Serializable]
    [WkTable("WK_Invoice")]
    public class InvoiceData
    {
        [WkField(DbFieldType.PRIMARY_KEY)]
        public string InvoiceNo { get; set; }

        [WkField]
        public DateTime Date { get; set; }

        [WkField]
        public string CustomerId { get; set; }

        [WkItemField]
        public List<InvoiceItemData> Items { get; set; }

        public InvoiceData(string vInvoiceNo)
            :this()
        {
            InvoiceNo = vInvoiceNo;
        }

        public InvoiceData()
        {
            InvoiceNo = string.Empty;
            Date = DateTime.Now;
            CustomerId = string.Empty;
            Items = new List<InvoiceItemData>();
        }
    }

    [Serializable]
    [WkTable("WK_InvoiceItem")]
    public class InvoiceItemData
    {
        [WkField(DbFieldType.PRIMARY_KEY)]
        public string ProductId { get; set; }

        [WkField]
        public int Qty { get; set; }

        [WkField]
        public decimal Amount { get; set; }

        public InvoiceItemData()
        {
            ProductId = string.Empty;
            Qty = 0;
            Amount = 0;
        }
    }
}
