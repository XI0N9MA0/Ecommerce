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
    public partial class FormUser : Form
    {
        DBWorkService data = new DBWorkService();

        private FormLogin frmlogin;
        private User user = new User();
        
        public FormUser(string userType, string accID, string username, FormLogin frmlogin)
        {
            InitializeComponent();
            user.UserType = userType;
            user.AccountID = accID;
            user.Username = username;
            this.frmlogin = frmlogin;
        }


        private void btnManageElec_Click(object sender, EventArgs e)
        {
            new frmElecManagement().Show();
        }

        private void btnManageBooks_Click_1(object sender, EventArgs e)
        {
            new frmBookManagement().Show();
        }

        private void btnCart_Click(object sender, EventArgs e)
        {
            new frmCart(user, profile_picture.Image, this).ShowDialog();
            
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            frmlogin.Show();
            this.Close();
        }

        private void FormUser_Load(object sender, EventArgs e)
        {
            if(user.UserType != "Admin")
            {
                txtMessage.Hide();
                btnManageBooks.Hide();
                btnManageElec.Hide();
            }
            else
            {
                // show bot message and only visible to admin
                List<string> returnMessage = new List<string>();
                returnMessage.Add(": Welcome Back! :) ");
                returnMessage.Add(": Happy to see you again! ");
                returnMessage.Add(": Oh hey! How are you? I hope you're doing fine, ");
                returnMessage.Add(": Back to Work already? ");
                returnMessage.Add(": I knew you'd be back someday! ");
                returnMessage.Add(": Testing again, huh?? ");
                returnMessage.Add(": The other admins must be on a vacation or sth, right? ");
                Random random = new Random();
                string msg = returnMessage.ElementAt(random.Next(0, 7));
                txtMessage.Text =  msg + user.Username;
                lblUsername.Text = user.UserType+" "+ user.Username;
            }

            btnRefresh_Click(sender, e);

            
        }

        private void DisplayUserInfo()
        {
            lblPoint.Text = user.UserPoint.ToString();
            lblUsername.Text = user.Username;
            if(user.UserType != "Admin")
                profile_picture.Image = user.Pfpicture;
        }

        public void btnRefresh_Click(object sender, EventArgs e)
        {
            data.Get_UserInfo(user);
            DisplayUserInfo();
            FLPItem.Controls.Clear();
            data.Get_AllProducts_AndAddToStringCollection(user, this);
        }


        private void cmbCategory_SelectedValueChanged(object sender, EventArgs e)
        {
            btnRefresh_Click(sender, e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            FLPItem.Controls.Clear();
            data.Search_Products(user, this);
        }

        private void btnInvoices_Click(object sender, EventArgs e)
        {
            new frmInvoiceList(user, profile_picture.Image, this).ShowDialog();
        }
    }
}
