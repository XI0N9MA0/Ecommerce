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
    public partial class INVOICEITEM : UserControl
    {
        private string id;
        private string itemName;
        private int qty;
        private decimal unitPrice;
        private decimal totalAmount;

        public INVOICEITEM()
        {
            InitializeComponent();
        }

        public string Id { get => id; set => id = value; }
        public string ItemName { get => itemName; set => itemName = value; }
        public int Qty { get => qty; set => qty = value; }
        public decimal UnitPrice { get => unitPrice; set => unitPrice = value; }
        public decimal TotalAmount { get => totalAmount; set => totalAmount = value; }

        private void INVOICEITEM_Load(object sender, EventArgs e)
        {
            lblID.Text = Id;
            lblName.Text = ItemName;
            lblQuantity.Text = Qty.ToString();
            lblUnitPrice.Text = "$" + string.Format("{0:F2}", UnitPrice);
            lblTotalAmount.Text = "$" + string.Format("{0:F2}", Qty*UnitPrice);
        }
    }
}
