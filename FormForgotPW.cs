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
    public partial class FormForgotPW : Form
    {
        ForgetPassword FGCustomer = new ForgetPassword();
        AccountDB accountDB = new AccountDB();

        public FormForgotPW()
        {
            InitializeComponent();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            
            try
            {
                FGCustomer.PhoneNo = txtPhone.Text;
                FGCustomer = accountDB.retreiveFGCustomer_AccID(FGCustomer);
                if (FGCustomer.AccID != null)
                {
                    txtNPassw.Enabled = true;
                    txtCPassw.Enabled = true;
                    btnConfirm.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Please enter your username and phone number correctly!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            FGCustomer.Npassw = txtNPassw.Text;
            FGCustomer.Cpassw = txtCPassw.Text;
            try
            {
                if (Validator.IsTextPresent(txtNPassw) || Validator.IsTextPresent(txtCPassw))
                {
                    if (FGCustomer.Npassw == FGCustomer.Cpassw)
                    {
                        accountDB.ResetPassword(FGCustomer.Npassw, FGCustomer.AccID);
                        DialogResult result = MessageBox.Show("Successfully reset a password!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (result == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Password do not match!");
                    }

                }
                else
                {
                    txtNPassw.PasswordChar = '\0';
                    txtNPassw.PlaceholderForeColor = Color.Red;
                    txtNPassw.PlaceholderText = "Fill out new password";
                    txtCPassw.PasswordChar = '\0';
                    txtCPassw.PlaceholderForeColor = Color.Red;
                    txtCPassw.PlaceholderText = "Fill out confirm password";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
    }
}
