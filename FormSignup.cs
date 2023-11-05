using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ecommercetestttt
{
    public partial class FormSignup : Form
    {
        CustomerDB customerDB = new CustomerDB();
        public FormSignup()
        {
            InitializeComponent();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            profile_Picture.Image = Info.Get_Uploaded_Picture();
        }

        private void Get_Acc_InputInfo(ref Account acc)
        {
            acc.Username = txtUsername.Text;
            acc.Password = txtPassword.Text;
            acc.Cpassword = txtCPassword.Text;
            acc.UserType = "Customer";
        }

        private void Get_Cus_InputInfo(ref Customer cus)
        {
            cus.FirstName = txtFirstName.Text;
            cus.LastName = txtLastName.Text;
            cus.BirthDate = birthdatePicker.Value.ToString("yyyy-MM-dd");
            cus.Email = txtEmail.Text;
            cus.PhoneNo = Info.GetPhone_whitespace(txtPhone.Text);
            cus.Address = txtAddress.Text;
            if (rdbMale.Checked)
            {
                cus.Gender = 'M';
            }
            else if (rdbFemale.Checked)
            {
                cus.Gender = 'F';
            }
            else
            {
                cus.Gender = 'O';
            }
            cus.ProfilePicture = profile_Picture.Image;
        }

        private bool IsValidData(List<string> Exist_Username, List<string> Exist_PhoneNo)
        {
            return Validator.IsUnique(txtUsername, Exist_Username) && Validator.IsValidPassword(txtPassword, txtCPassword) &&
                   Validator.IsTextPresent(txtFirstName) && Validator.IsTextPresent(txtLastName) && Validator.IsGenderPresent(rdbMale, rdbFemale, rdbOther) &&
                   Validator.IsValidBirthDate(birthdatePicker) && Validator.IsValidEmail(txtEmail) && Validator.IsUniquePhone(txtPhone, Exist_PhoneNo) &&
                   Validator.IsTextPresent(txtAddress) && Validator.IsImagePresent(profile_Picture);
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {

            List<string> Exist_Username = new List<string>();
            List<string> Exist_PhoneNo = new List<string>();

            Exist_Username = customerDB.Get_AllUsername(Exist_Username);
            Exist_PhoneNo = customerDB.Get_AllPhoneNo(Exist_PhoneNo);

            Account acc = new Account();
            Customer cus = new Customer();

            Get_Acc_InputInfo(ref acc);
            Get_Cus_InputInfo(ref cus);

            try
            {
                //check to see if input data is correct or not
                if (IsValidData(Exist_Username, Exist_PhoneNo))
                {
                    customerDB.Insert_NewCustomer(acc, cus);
                    MessageBox.Show("Saved");
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
