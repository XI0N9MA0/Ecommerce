using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ecommercetestttt
{
    public partial class frmInvoice : Form
    {
        private string cusID;
        private string invoiceID;

        DBWorkService data = new DBWorkService();

        public frmInvoice(string cusID, string invoiceID)
        {
            InitializeComponent();
            this.cusID = cusID;
            this.invoiceID = invoiceID;
        }

        private void Invoice_Load(object sender, EventArgs e)
        {
            //data.CartToInvoice(this, cusID);
            data.display_invoice(this, cusID, invoiceID);
            
        }

        
    }
}
