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

namespace ecommercetestttt
{
    public partial class frmBookManagement : Form
    {
        private BooksDB booksDB = new BooksDB();
        private List<Guna2CheckBox> chkbxs = new List<Guna2CheckBox>();
        private List<int> genreID = new List<int>();
        private Book book = new Book();
        private List<string> genres = new List<string>();

        public frmBookManagement()
        {
            InitializeComponent();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            BookImageBx.Image = Info.Get_Uploaded_Picture();
        }

        private void frmBookManagement_Load(object sender, EventArgs e)
        {
            chkbxs.Add(chkbComic);
            chkbxs.Add(chkbHumor);
            chkbxs.Add(chkbHorror);
            chkbxs.Add(chkbRomance);
            chkbxs.Add(chkbNovel);
            chkbxs.Add(chkbSelfHelp);
            chkbxs.Add(chkbSciFi);
            chkbxs.Add(chkbFairyTale);
            Display_BookInfotoControls(genres);
        }

        private void Get_Book_InputInfo()
        {
            book.Id = "B"+txtID.Text;
            book.Title = txtTitle.Text;
            book.PrintLength = int.Parse(txtPrintLength.Text);
            book.Price = decimal.Parse(txtPrice.Text);
            book.Qty = int.Parse(txtQty.Text);
            book.Author = txtAuthor.Text;
            book.Publisher = txtPublisher.Text;
            book.PublicationDate = PublicationDPKR.Value.ToString("yyyy-MM-dd");
            book.Language = cmbLanguage.Text;
            book.Description = txtDescription.Text;
            book.Picture = BookImageBx.Image;
            if (book.Qty > 0)
            {
                book.InStockStatus = true;
            }
        }

        private bool IsValidData()
        {
            return Validator.IsTextPresent(txtID) && Validator.IsTextPresent(txtTitle) && Validator.IsDecimal(txtPrintLength) &&
                   Validator.IsDecimal(txtPrice) && Validator.IsTextPresent(txtAuthor) && Validator.IsDecimal(txtQty) &&
                   Validator.IsSelected(cmbLanguage) && Validator.IsChecked(chkbxs) && Validator.IsImagePresent(BookImageBx) &&
                   Validator.IsTextPresent(txtDescription);
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            booksDB.Get_GenreID(genreID, chkbxs);
            try
            {
                if (IsValidData())
                {
                    Get_Book_InputInfo();

                    booksDB.Insert_IntoBook(book);
                    //input into BookGenre Table
                    booksDB.Set_NewData_Into_BookGenre(genreID, book);
                    MessageBox.Show("Saved!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnDiscard_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Confirm Discard", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Clear_Input_BookInfo();
            }
        }

        private void txtID_TextChange(object sender, EventArgs e)
        {
            //set everything to default before read from database 
            Clear_Input_BookInfo();
            book.Id = "B"+txtID.Text;
            booksDB.Search_BookForManagement(ref book, ref genres, chkbxs, this);
            Display_BookInfotoControls(genres);
        }

        public void Display_BookInfotoControls(List<string> genres)
        {
            
            txtTitle.Text = book.Title;
            txtPrintLength.Text = book.PrintLength.ToString();
            txtAuthor.Text = book.Author;
            txtQty.Text = book.Qty.ToString();
            txtPrice.Text = book.Price.ToString();
            txtPublisher.Text = book.Publisher;

            
            PublicationDPKR.Value = DateTime.Parse(book.PublicationDate);
            BookImageBx.Image = book.Picture;
            txtDescription.Text = book.Description;
            cmbLanguage.Text = book.Language;
            foreach (Guna2CheckBox chkbx in chkbxs)
            {
                foreach(string genre in genres)
                {
                    if(genre == chkbx.Text)
                        chkbx.Checked = true;
                }    
            }
        }

        private void Clear_Input_BookInfo()
        {
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnAdd.Enabled = true;
            txtTitle.Text = null;
            txtPrintLength.Text = null;
            txtAuthor.Text = null;
            txtQty.Text = null;
            txtPrice.Text = null;
            txtPublisher.Text = null;
            PublicationDPKR.Value = DateTime.Now;
            BookImageBx.Image = null;
            txtDescription.Text = null;
            cmbLanguage.Text = null;
            for (int i = 0; i < chkbxs.Count; i++)
            {
                chkbxs[i].Checked = false;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            booksDB.Get_GenreID(genreID, chkbxs);
            try
            {
                if (IsValidData())
                {
                    Get_Book_InputInfo();

                    booksDB.UpdateBook(book);

                    //Remove bookID and genreID
                    booksDB.Delete_Data_From_BookGenre(book);

                    //Set New bookID and genreID
                    booksDB.Set_NewData_Into_BookGenre(genreID, book);

                    MessageBox.Show("Updated!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to delete?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Get_Book_InputInfo();// get everything info from the form
                booksDB.Delete_Data_From_BookGenre(book);
                booksDB.Delete_Data_From_Book(book);
                MessageBox.Show("Deleted Successfully!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Clear_Input_BookInfo();
            }

        }

        
    }
}
