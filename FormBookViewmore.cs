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
    public partial class FormBookViewmore : Form
    {
        private DBWorkService data = new DBWorkService();
        private BooksDB booksDB = new BooksDB();
        private Book book = new Book();
        private string cusID = null;

        public FormBookViewmore(string id, string cusID)
        {
            InitializeComponent();
            book.Id = id;
            this.cusID = cusID;
        }

        private void display_BookInfo()
        {
            txtProductID.Text = book.Id;
            lblTitle.Text = book.Title;
            Book_picture.Image = book.Picture;
            lblAuthor.Text = book.Author;
            lblGenre.Text = book.Genre;
            lblPrice.Text = "$" + String.Format("{0:F2}", book.Price);
            txtDescription.Text = book.Description;
            lblPrintLength.Text = book.PrintLength.ToString();
            lblLanguage.Text = book.Language;
            lblPublisher.Text = book.Publisher;
            lblPublicationDate.Text = DateTime.Parse(book.PublicationDate).ToString("dd/MM/yyyy");
            if (book.InStockStatus == true)
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

        private void FormBookViewmore_Load(object sender, EventArgs e)
        {
            book = booksDB.Get_Book(book.Id);
            display_BookInfo();
        }

        private void lblInStock_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddtoCart_Click(object sender, EventArgs e)
        {
            if (Validator.Check_CusID_ProductQty(cusID, Qty_UPDown, txtProductID))
            {
                Book booktoCart = new Book();
                get_display_info(ref booktoCart);
                booksDB.Send_BookToCart(booktoCart, cusID);
            }
            else
            {
                Qty_UPDown.Value = data.Get_ProductQty(txtProductID.Text);
            }
        }

        private void get_display_info(ref Book booktoCart)
        {
            booktoCart.Id = txtProductID.Text;
            booktoCart.Title = lblTitle.Text;
            booktoCart.Picture = Book_picture.Image;
            booktoCart.Price = book.Price;
            booktoCart.Qty = int.Parse(Qty_UPDown.Value.ToString());
        }
    }
}
