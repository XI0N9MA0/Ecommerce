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
    public partial class FormLogin : Form
    {
        private Login login = new Login();
        private AccountDB accountDB = new AccountDB();
        public FormLogin()
        {
            InitializeComponent();
        }

        private bool IsEmpty_Username_Password()
        {
            login.Username = txtusername.Text;
            login.Password = txtpassword.Text;

            if(login.Username != "" && login.Password != "")
                return false;
            return true;
        }

        private void btnLog_in_Click(object sender, EventArgs e)
        {

            if (!IsEmpty_Username_Password())
            {
                try
                {
                    //check to see if input data is valid
                    login = accountDB.Get_LoginInfo(login);

                    //if accountId exist let user log in
                    if (login.AccountID != null)
                    {
                        new FormUser(login.UserType, login.AccountID, login.Username, this).Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Fail! Please enter username and password correctly");
                        txtusername.Text = "";
                        txtusername.PlaceholderText = "";
                        txtpassword.Text = "";
                        txtpassword.PlaceholderText = "";
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please fill in your username and password!");
            }

        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            new FormSignup().ShowDialog();
        }

        private void btnForgetPW_Click(object sender, EventArgs e)
        {
            new FormForgotPW().ShowDialog();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            login.AccountID = null;
            login.Username = null;
            login.UserType = null;
        }
    }
}
