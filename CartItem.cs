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
    public partial class CartItem : System.Windows.Forms.UserControl
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-4FMPOU4\SQLEXPRESS;Initial Catalog=dbECommerce;Integrated Security=True");
        SqlCommand cmd;
        string sql;

        DBWorkService data = new DBWorkService();

        private frmCart frmcart;
        private string productId;
        private Image picture;
        private string product_Name;
        private decimal unitPrice;
        private int qty;
        private decimal totalAmount;
        private string cusID;

        public CartItem(string cusID, frmCart frmcart)
        {
            InitializeComponent();
            this.cusID = cusID;
            this.frmcart = frmcart;
        }

        public string ProductId { get => productId; set => productId = value; }
        public Image Picture { get => picture; set => picture = value; }
        public string Product_Name { get => product_Name; set => product_Name = value; }
        public decimal UnitPrice { get => unitPrice; set => unitPrice = value; }
        public int Qty { get => qty; set => qty = value; }
        public decimal TotalAmount { get => totalAmount; set => totalAmount = value; }

        private void cartItem_Load(object sender, EventArgs e)
        {
            picturebx.Image = picture;
            lblProductName.Text = product_Name;
            lblUnitPrice.Text = "$"+string.Format("{0:F2}", UnitPrice);
            txtQty.Text = Qty.ToString();
            lblTotalAmount.Text = "$"+string.Format("{0:F2}", TotalAmount);

            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            data.Delete_SpecificItemFromCart(cusID, ProductId);
            frmcart.btnRefresh_Click(sender, e);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            Qty++;
            if (Qty <= data.Get_ProductQty(ProductId))
            {
                sqlConnection.Open();
                sql = @"UPDATE Carts 
                    SET qty = @qty,
                        total_amount = @total_amount 
                    WHERE cusID ='" + cusID + "' AND productID = '" + ProductId + "'";
                cmd = new SqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@qty", Qty);
                cmd.Parameters.AddWithValue("@total_amount", UnitPrice * Qty);
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                frmcart.btnRefresh_Click(sender, e);
            }
            else
            {
                MessageBox.Show("Out of Stock!");
                Qty = data.Get_ProductQty(ProductId);
            }
            
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            Qty--;
            if(Qty == 0)
            {
                btnDelete_Click(sender, e);
            }
            else
            {
                sqlConnection.Open();
                sql = @"UPDATE Carts 
                    SET qty = @qty,
                        total_amount = @total_amount 
                    WHERE cusID ='" + cusID + "' AND productID = '" + ProductId + "'";
                cmd = new SqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@qty", Qty);
                cmd.Parameters.AddWithValue("@total_amount", UnitPrice * Qty);
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                frmcart.btnRefresh_Click(sender, e);
            }
            
        }
    }
}
