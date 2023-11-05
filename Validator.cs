using Guna.UI2.WinForms;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ecommercetestttt
{
    public static class Validator
    {
        private static string title = "Entry Error";

        public static string Title { get => title; set => title = value; }


        public static bool IsTextPresent(Guna2TextBox txtB)
        {
            if (txtB.Text == string.Empty)
            {
                MessageBox.Show(txtB.Tag + " is a required field", Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        public static bool IsValidPassword(Guna2TextBox txtPass, Guna2TextBox txtCPass)
        {
            if (txtPass.TextLength < 8 || txtPass.Text != txtCPass.Text)
            {
                MessageBox.Show(txtPass.Tag + " must be at least 8 characters.\n" + "Password and Confirm must be the same.", Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        public static bool IsImagePresent(PictureBox picturebx)
        {
            if (picturebx.Image == null)
            {
                MessageBox.Show(picturebx.Tag + " is a required field", Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;

        }

        public static bool IsGenderPresent(Guna2RadioButton rdbM, Guna2RadioButton rdbF, Guna2RadioButton rdbO)
        {
            if (!rdbM.Checked && !rdbF.Checked && !rdbO.Checked)
            {
                MessageBox.Show("Please select your gender.", Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        public static bool IsValidBirthDate(DateTimePicker datepicker)
        {
            DateTime CurrentDate = DateTime.Today;

            if ((CurrentDate.Year - datepicker.Value.Year) <= 12)
            {
                MessageBox.Show("Sorry, you must be older than 12 years old to sign up.", Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }


            return true;
        }

        public static bool IsValidPhoneNo(Guna2TextBox txtB)
        {

            try
            {
                //try to convert input to decimal to see if it is number
                string phoneNo = txtB.Text;
                phoneNo = phoneNo.Replace(" ", string.Empty);
                txtB.Text = phoneNo;
                Convert.ToInt64(txtB.Text);
                //check to see if input number is longer than 7 digits
                if (txtB.TextLength <= 7)
                {
                    MessageBox.Show("Phone number must be longer than 7 digits.", Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                return true;
            }
            catch (FormatException)
            {

                MessageBox.Show("Phone number must be all number.", Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        public static bool IsValidEmail(Guna2TextBox txtB)
        {
            var email = new EmailAddressAttribute();
            if (email.IsValid(txtB.Text) == false)
            {
                MessageBox.Show("Invalid email address.", Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        public static bool IsUnique(Guna2TextBox txtb, List<string> Exist_item)
        {
            if (IsTextPresent(txtb))
            {
                foreach (string name in Exist_item)
                {
                    if (name == txtb.Text)
                    {
                        MessageBox.Show(txtb.Text + " is already exist!", Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsUniquePhone(Guna2TextBox txtb, List<string> Exist_item)
        {
            if (IsValidPhoneNo(txtb))
            {
                txtb.Text = Info.GetPhone_whitespace(txtb.Text);
                foreach (string name in Exist_item)
                {
                    if (name == txtb.Text)
                    {
                        MessageBox.Show(txtb.Text + " is already being used!", Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsChecked(List<Guna2CheckBox> chkbxs)
        {
            int count = 0;
            foreach (Guna2CheckBox chkbx in chkbxs)
            {
                if (!chkbx.Checked)
                {
                    count++;
                    if (count == chkbxs.Count)
                    {
                        MessageBox.Show("No checkboxs have been checked", Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                }
            }
            return true;
        }

        public static bool IsSelected(ComboBox cmb)
        {
            if (cmb.SelectedIndex == -1)
            {
                MessageBox.Show(cmb.Tag + " has not been selected!", Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }


            return true;
        }

        public static bool IsDecimal(Guna2TextBox txtB)
        {
            try
            {
                Convert.ToDecimal(txtB.Text);
                return true;
            }
            catch
            {
                MessageBox.Show("Invalid Input! Make sure " + txtB.Tag + " is number.", Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

        }

        public static bool Check_CusID_ProductQty(string cusID, Guna2NumericUpDown Qty_UPDown, Guna2TextBox txtProductID)
        {
            DBWorkService data = new DBWorkService();

            if (cusID == null || cusID == "0")
            {
                MessageBox.Show("User has no Customer ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(Qty_UPDown.Value > data.Get_ProductQty(txtProductID.Text))
            {
                MessageBox.Show("Out of Stock!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return cusID != null && Qty_UPDown.Value <= data.Get_ProductQty(txtProductID.Text);
        }

    }
}
