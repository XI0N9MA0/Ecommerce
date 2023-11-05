using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Guna.UI2.Native.WinApi;

namespace ecommercetestttt
{
    public partial class FormElecViewmore : Form
    {
        DBWorkService data = new DBWorkService();
        ElectronicsDB electronicsDB = new ElectronicsDB();

        private Electronic electronic = new Electronic();
        private string cusID;
        public FormElecViewmore(string id, string cusID)
        {
            InitializeComponent();
            electronic.Id = id;
            this.cusID = cusID;
        }

        private void display_Info()
        {
            txtProductID.Text = electronic.Id;
            lblTitle.Text = electronic.Name;
            elec_picture.Image = electronic.Picture;
            lblManufacture.Text = electronic.Company;
            lblType.Text = electronic.Type;
            lblPrice.Text = "$" + String.Format("{0:F2}", electronic.Price);
            txtDescription.Text = electronic.Description;
            lblColor.Text = electronic.Color;
            lblSize.Text = electronic.Size+" inche(s)";
            lblReleaseDate.Text = DateTime.Parse(electronic.ReleaseDate).ToString("dd/MM/yyyy");
            if (electronic.InStockStatus == true)
            {
                lblInStock.ForeColor = Color.FromArgb(83, 145, 126);
                lblInStock.Text = "In Stock";
            }
            else
            {
                Qty_UPDown.Enabled = false;
                btnAddtoCart.Enabled = false;
                btnBuyNow.Enabled = false;
                lblInStock.ForeColor = Color.FromArgb(235, 156, 92);
                lblInStock.Text = "Unavailable";
            }
        }

        private void FormElecViewmore_Load(object sender, EventArgs e)
        {
            electronic = electronicsDB.Get_Electronic(electronic.Id);
            display_Info();
        }

        private void btnAddtoCart_Click(object sender, EventArgs e)
        {
            if(Validator.Check_CusID_ProductQty(cusID, Qty_UPDown, txtProductID))
            {
                Electronic electoCart = new Electronic();
                get_displayInfo(ref electoCart);
                electronicsDB.Send_ElecToCart(electoCart, cusID);
            }
            else
            {
                Qty_UPDown.Value = data.Get_ProductQty(electronic.Id);
            }
            
        }

        private void get_displayInfo(ref Electronic electoCart)
        {
            electoCart.Id = txtProductID.Text;
            electoCart.Name = lblTitle.Text;
            electoCart.Picture = elec_picture.Image;
            electoCart.Price = electronic.Price;
            electoCart.Qty = int.Parse(Qty_UPDown.Value.ToString());
        }
    }
}
