using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ecommercetestttt
{
    public partial class InvoiceListItem : UserControl
    {
        private string invoiceID;
        private string invoiceDate;
        private string cusID;

        public InvoiceListItem(string cusID)
        {
            InitializeComponent();
            this.cusID = cusID;
        }

        public string InvoiceID { get => invoiceID; set => invoiceID = value; }
        public string InvoiceDate { get => invoiceDate; set => invoiceDate = value; }

        private void btnView_Click(object sender, EventArgs e)
        {
            new frmInvoice(cusID, InvoiceID).ShowDialog();
        }

        private void InvoiceListItem_Load(object sender, EventArgs e)
        {
            lblInvoiceID.Text = invoiceID;
            lblInvoiceDate.Text = DateTime.Parse(invoiceDate).ToString("dd/MM/yyyy");
        }
    }
}
