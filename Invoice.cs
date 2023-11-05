using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommercetestttt
{
    public class Invoice
    {
        private int invoiceID;
        private string invoiceDate;
        private string cusID;
        private decimal total;
        private decimal discount;
        private decimal subtotal;
        private decimal paidAmount;
        private string address;

        public int InvoiceID { get => invoiceID; set => invoiceID = value; }
        public string InvoiceDate { get => invoiceDate; set => invoiceDate = value; }
        public string CusID { get => cusID; set => cusID = value; }
        public decimal Total { get => total; set => total = value; }
        public decimal Discount { get => discount; set => discount = value; }
        public decimal Subtotal { get => subtotal; set => subtotal = value; }
        public decimal PaidAmount { get => paidAmount; set => paidAmount = value; }
        public string Address { get => address; set => address = value; }
    }
}
