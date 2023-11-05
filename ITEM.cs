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
    public partial class ITEM : UserControl
    {
        private bool IsBook = false;
        private string cusID = null;
        public ITEM(string item, string cusID)
        {
            InitializeComponent();
            this.cusID = cusID;
            if (item == "Books")
                IsBook = true;
            else
                IsBook = false;
            
        }
        private string id;
        private string title;
        private string author;
        private decimal price;
        private decimal rate;
        private Image picture;
        private string company;

        public string Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Author { get => author; set => author = value; }
        public decimal Price { get => price; set => price = value; }
        public decimal Rate { get => rate; set => rate = value; }
        public Image Picture { get => picture; set => picture = value; }
        public string Company { get => company; set => company = value; }

        private void btnViewMore_Click(object sender, EventArgs e)
        {
            if (IsBook)
                new FormBookViewmore(Id, cusID).ShowDialog();
            else
                new FormElecViewmore(Id, cusID).ShowDialog();
        }

        private void ITEM_Load(object sender, EventArgs e)
        {
            
            if (IsBook)
            {
                lblTitle.Text = Title;
                picturebx.Image = Picture;
                picturebx.SizeMode = PictureBoxSizeMode.StretchImage;
                lblChangeable.Text = "By";
                lblAuthorOrCompany.Text = Author;
                lblPrice.Text = "$" + String.Format("{0:F2}",Price); //get just two decimal places ex: 123.1234 to 123.12
                lblRate.Text = "("+Rate.ToString()+")";
            }
            else
            {
                lblChangeable.Text = "Company:";
                lblTitle.Text = Title;
                picturebx.SizeMode = PictureBoxSizeMode.Zoom;
                picturebx.Image = Picture;
                lblAuthorOrCompany.Text = Company;
                lblPrice.Text = "$" + String.Format("{0:F2}", Price); //get just two decimal places ex: 123.1234 to 123.12
                lblRate.Text = "(" + Rate.ToString() + ")";
            }
        }
    }
}
