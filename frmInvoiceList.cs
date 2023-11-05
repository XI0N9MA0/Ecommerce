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
    public partial class frmInvoiceList : Form
    {
        private DBWorkService data = new DBWorkService();
        private List<Invoice> invoices = new List<Invoice>();

        private User user = new User();
        private FormUser frmUser;

        public frmInvoiceList(User user, Image pfpicture, FormUser frmUser)
        {
            InitializeComponent();
            this.user = user;
            user.Pfpicture = pfpicture;
            this.frmUser = frmUser;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmInvoiceList_Load(object sender, EventArgs e)
        {
            lblUsername.Text = user.Username;
            lblPoint.Text = user.UserPoint.ToString();
            profile_picture.Image = user.Pfpicture;

            if(user.CusID != null)
            {
                invoices = data.Get_InvoiceIDNDate_AndAddtoSearchCollection(user.CusID, invoices, this);
                Display_InvoiceList();
            }
        }

        public void Display_InvoiceList()
        {
            FLPInvoicesList.Controls.Clear();
            InvoiceListItem item;
            foreach (Invoice invoice in invoices)
            {
                item = new InvoiceListItem(user.CusID);
                item.InvoiceID = invoice.InvoiceID.ToString();
                item.InvoiceDate = invoice.InvoiceDate.ToString();
                FLPInvoicesList.Controls.Add(item);
            }
        }

        private void btnCart_Click(object sender, EventArgs e)
        {
            new frmCart(user, profile_picture.Image, frmUser).ShowDialog();
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FLPInvoicesList.Controls.Clear();
            data.SearchInvoices(user.CusID, this);
        }

        private void btnInvoices_Click(object sender, EventArgs e)
        {

        }
    }
}
