using Guna.UI2.WinForms;
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
using static Guna.UI2.Native.WinApi;

namespace ecommercetestttt
{
    public partial class frmElecManagement : Form
    {
        ElectronicsDB electronicsDB = new ElectronicsDB();
        Electronic electronic = new Electronic();

        public frmElecManagement()
        {
            InitializeComponent();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            ElecImageBx.Image = Info.Get_Uploaded_Picture();
        }

        private bool IsValidData()
        {
            return Validator.IsTextPresent(txtID) && Validator.IsTextPresent(txtName) && Validator.IsDecimal(txtPrice) &&
                   Validator.IsDecimal(txtQty) && Validator.IsTextPresent(txtColor) && Validator.IsDecimal(txtSizeOrLength) &&
                   Validator.IsTextPresent(txtCompany) && Validator.IsImagePresent(ElecImageBx) && Validator.IsSelected(cmbType) &&
                   Validator.IsTextPresent(txtDescription);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidData())
                {
                    Get_Elec_InputInfo();
                    electronicsDB.Insert_IntoElectronic(electronic);
                    MessageBox.Show("Saved Successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Get_Elec_InputInfo()
        {
            electronic.Id = "E"+txtID.Text;
            electronic.Name = txtName.Text;
            electronic.Size = decimal.Parse(txtSizeOrLength.Text);
            electronic.Price = decimal.Parse(txtPrice.Text);
            electronic.Qty = int.Parse(txtQty.Text);
            electronic.Company = txtCompany.Text;
            electronic.Color = txtColor.Text;
            electronic.ReleaseDate = releaseDateTPKR.Value.ToString("yyyy-MM-dd");
            electronic.Type = cmbType.Text;
            electronic.Description = txtDescription.Text;
            electronic.Picture = ElecImageBx.Image;
            electronic.ElectronicTypeID = electronicsDB.Get_ElectronicTypeID(electronic.Type);
            if (electronic.Qty > 0)
            {
                electronic.InStockStatus = true;
            }
            else
            {
                electronic.InStockStatus = false;
            }
        }

        private void Clear_Input_ElecInfo()
        {
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnAdd.Enabled = true;

            txtName.Text = null;
            txtSizeOrLength.Text = null;
            txtCompany.Text = null;
            txtQty.Text = null;
            txtPrice.Text = null;
            txtColor.Text = null;
            releaseDateTPKR.Value = DateTime.Now;
            ElecImageBx.Image = null;
            txtDescription.Text = null;
            cmbType.Text = null;
        }

        private void Display_ElecInfo()
        {
            txtName.Text = electronic.Name;
            txtPrice.Text = electronic.Price.ToString();
            txtColor.Text = electronic.Color;
            txtQty.Text = electronic.Qty.ToString();
            txtCompany.Text = electronic.Company;
            txtSizeOrLength.Text = electronic.Size.ToString();
            releaseDateTPKR.Value = DateTime.Parse(electronic.ReleaseDate);
            txtDescription.Text = electronic.Description;
            cmbType.Text = electronic.Type;
            ElecImageBx.Image = electronic.Picture;
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {
            Clear_Input_ElecInfo();
            electronic.Id = "E" + txtID.Text;
            electronicsDB.Search_ElecForManagement(ref electronic, this);
            Display_ElecInfo();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to update?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (IsValidData())
                {
                    Get_Elec_InputInfo();
                    electronicsDB.UpdateElectronics(electronic);
                    MessageBox.Show("Updated!");
                }
            }
        }

        private void btnDiscard_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Confirm Discard", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Clear_Input_ElecInfo();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to delete?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Get_Elec_InputInfo();// get everything info from the form
                electronicsDB.Delete_Data_From_Electronics(electronic);
                MessageBox.Show("Deleted Successfully!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Clear_Input_ElecInfo();
            }
        }

        private void frmElecManagement_Load(object sender, EventArgs e)
        {
            Display_ElecInfo();
        }
    }
}
