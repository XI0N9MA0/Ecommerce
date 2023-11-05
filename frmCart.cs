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
    public partial class frmCart : Form
    {
        DBWorkService data = new DBWorkService();
        FormUser frmUser;
        private User user = new User();

        public frmCart(User user, Image pf_picture, FormUser frmUser)
        {
            InitializeComponent();
            this.user = user;
            profile_picture.Image = pf_picture;
            this.frmUser = frmUser;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            frmUser.btnRefresh_Click(sender, e);
            this.Close();
        }

        private void frmCart_Load(object sender, EventArgs e)
        {
            lblUsername.Text = user.Username;
            lblPoint.Text = user.UserPoint.ToString();
            btnRefresh_Click(sender, e);
        }

        public void btnRefresh_Click(object sender, EventArgs e)
        {
            cartFLP.Controls.Clear();
            data.FromCarts_To_EachCartItem_AndDisplayCartItems(this, user.CusID, user.UserPoint);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            if(cartFLP.Controls.Count > 0)
            {
                Invoice invoice = new Invoice();
                List<INVOICEITEM> products = new List<INVOICEITEM>();

                products = data.Get_AllProduct_FromCarts(products, user.CusID);
                invoice = data.Get_InvoiceInfo(invoice, user.CusID, user.UserPoint);

                data.Insert_IntoInvoice(invoice, products);
                data.Decrease_ProductQty_Increase_ProductSoldQty(products);
                
                data.Delete_FromCarts();
                cartFLP.Controls.Clear();
                new frmInvoice(user.CusID, invoice.InvoiceID.ToString()).Show();
            }
            else
            {
                MessageBox.Show("Cart is Empty...");
            }
            
        }

        private void btnInvoices_Click(object sender, EventArgs e)
        {
            new frmInvoiceList(user, profile_picture.Image, frmUser).ShowDialog();
            this.Close();
        }
    }
}
