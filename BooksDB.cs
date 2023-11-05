using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ecommercetestttt
{
    public class BooksDB
    {
        private SqlConnection sqlConnection = EcommerceDB.GetConnection();
        private SqlCommand cmd;
        private SqlDataReader reader;
        private string sql;
        
        public void Delete_Data_From_BookGenre(Book book)
        {
            sqlConnection.Open();
            sql = @"DELETE FROM BookGenre WHERE bookID = '" + book.Id + "'";
            cmd = new SqlCommand(sql, sqlConnection);
            cmd.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public void Delete_Data_From_Book(Book book)
        {
            sqlConnection.Open();
            sql = @"DELETE FROM Books WHERE bookID ='" + book.Id + "'";
            cmd = new SqlCommand(sql, sqlConnection);
            cmd.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public void Set_NewData_Into_BookGenre(List<int> genreID, Book book)
        {
            foreach (int genre in genreID)
            {
                sqlConnection.Open();
                sql = "INSERT INTO BookGenre(bookID, genreID) VALUES (@bookID, @genreID)";
                cmd = new SqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@bookID", book.Id);
                cmd.Parameters.AddWithValue("@genreID", genre);
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public void Get_GenreID(List<int> genreID, List<Guna2CheckBox> chkbxs)
        {
            genreID.Clear();
            List<string> genres = new List<string>();


            for (int i = 0; i < chkbxs.Count; i++)
            {
                if (chkbxs[i].Checked)
                    genres.Add(chkbxs.ElementAt(i).Text);
            }

            foreach (string genre in genres)
            {
                sqlConnection.Open();
                sql = "SELECT genreID FROM Genres WHERE genre = '" + genre + "'";
                cmd = new SqlCommand(sql, sqlConnection);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    genreID.Add(int.Parse(reader["genreID"].ToString()));
                }
                sqlConnection.Close();
            }


        }

        public void UpdateBook(Book book)
        {
            sqlConnection.Open();
            sql = @"UPDATE Books 
                        SET    [title] = @title
                              ,[language] = @language
                              ,[author] = @author
                              ,[page] = @page
                              ,[publisher] = @publisher
                              ,[publicationDate] = @publicationDate
                              ,[price] = @price
                              ,[qty] = @qty
                              ,[picture] = @picture
                              ,[description] = @description
                              ,[inStock_Status] = @inStock_Status
                      WHERE bookID = '" + book.Id + "'";
            cmd = new SqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@title", book.Title);
            cmd.Parameters.AddWithValue("@language", book.Language);
            cmd.Parameters.AddWithValue("@author", book.Author);
            cmd.Parameters.AddWithValue("@page", book.PrintLength);
            cmd.Parameters.AddWithValue("@publisher", book.Publisher);
            cmd.Parameters.AddWithValue("@publicationDate", book.PublicationDate);
            cmd.Parameters.AddWithValue("@price", book.Price);
            cmd.Parameters.AddWithValue("@qty", book.Qty);
            cmd.Parameters.AddWithValue("@description", book.Description);
            cmd.Parameters.AddWithValue("@inStock_Status", book.InStockStatus);
            cmd.Parameters.AddWithValue("@picture", Info.Get_ImgToBinary(book.Picture));

            cmd.ExecuteNonQuery();

            sqlConnection.Close();
        }

        public void Insert_IntoBook(Book book)
        {
            sqlConnection.Open();
            sql = @"INSERT INTO Books(
                               [bookID]
	                          ,[title]
                              ,[language]
                              ,[author]
                              ,[page]
                              ,[publisher]
                              ,[publicationDate]
                              ,[price]
                              ,[qty]
                              ,[picture]
                              ,[description]
                              ,[inStock_Status]
                              ,[rate]
                              ,[totalRate]
                              ,[soldQty])
                    VALUES(
                               @bookID
	                          ,@title
                              ,@language
                              ,@author
                              ,@page
                              ,@publisher
                              ,@publicationDate
                              ,@price
                              ,@qty
                              ,@picture
                              ,@description
                              ,@inStock_Status
                              ,@rate
                              ,@totalRate
                              ,@soldQty)"
            ;

            cmd = new SqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@bookID", book.Id);
            cmd.Parameters.AddWithValue("@title", book.Title);
            cmd.Parameters.AddWithValue("@language", book.Language);
            cmd.Parameters.AddWithValue("@author", book.Author);
            cmd.Parameters.AddWithValue("@page", book.PrintLength);
            cmd.Parameters.AddWithValue("@publisher", book.Publisher);
            cmd.Parameters.AddWithValue("@publicationDate", book.PublicationDate);
            cmd.Parameters.AddWithValue("@price", book.Price);
            cmd.Parameters.AddWithValue("@qty", book.Qty);
            cmd.Parameters.AddWithValue("@description", book.Description);
            cmd.Parameters.AddWithValue("@inStock_Status", book.InStockStatus);
            cmd.Parameters.AddWithValue("@rate", book.Rate);
            cmd.Parameters.AddWithValue("@totalRate", book.TotalRate);
            cmd.Parameters.AddWithValue("@soldQty", book.SoldQty);
            cmd.Parameters.AddWithValue("@picture", Info.Get_ImgToBinary(book.Picture));
            cmd.ExecuteNonQuery();

            sqlConnection.Close();
        }


        public void Search_BookForManagement(ref Book book, ref List<string> genres, List<Guna2CheckBox> chkbxs, frmBookManagement frmbkm)
        {
            genres = new List<string>();
            sqlConnection.Open();
            sql = @"SELECT *
                    FROM Books B 
                    JOIN (BookGenre R 
                    JOIN Genres G
                    ON R.genreID = G.genreID)
                    ON B.bookID = R.bookID
                    WHERE B.bookID = '" + book.Id + "'";
            cmd = new SqlCommand(sql, sqlConnection);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                if (book.Id == reader["bookID"].ToString())
                {

                    frmbkm.btnUpdate.Enabled = true;
                    frmbkm.btnDelete.Enabled = true;
                    frmbkm.btnAdd.Enabled = false;

                    book.Title = reader["title"].ToString();
                    book.PrintLength = int.Parse(reader["page"].ToString());
                    book.Author = reader["author"].ToString();
                    book.Qty = int.Parse(reader["qty"].ToString());
                    book.Price = decimal.Parse(reader["price"].ToString());
                    book.Publisher = reader["publisher"].ToString();
                    book.PublicationDate = reader["publicationDate"].ToString();
                    book.Description = reader["description"].ToString();
                    book.Language = reader["language"].ToString();

                    //BookImageBx.Image is assigned by new Bitmap to avoid generic error occurred in gdi+
                    book.Picture = new Bitmap(Info.Get_BinaryToImg((byte[])reader["picture"]));
                    genres.Add(reader["genre"].ToString());
                }
            }
            sqlConnection.Close();
        }

        public Book Get_Book(string bookid)
        {
            Book book = new Book();
            try
            {
                sqlConnection.Open();
                sql = @"SELECT * 
                    FROM Books B 
                    JOIN(BookGenre R JOIN Genres G 
                    ON R.genreID = G.genreID) 
                    ON B.bookID = R.bookID  
                    WHERE B.bookID = '" + bookid + "'";
                cmd = new SqlCommand(sql, sqlConnection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    book.Id = reader["bookID"].ToString();
                    book.Title = reader["title"].ToString();
                    book.Price = decimal.Parse(reader["price"].ToString());
                    book.Description = reader["description"].ToString();
                    book.Qty = int.Parse(reader["qty"].ToString());
                    book.Rate = decimal.Parse(reader["rate"].ToString());
                    book.TotalRate = int.Parse(reader["totalRate"].ToString());
                    book.Author = reader["author"].ToString();
                    book.InStockStatus = bool.Parse(reader["inStock_Status"].ToString());
                    book.PrintLength = int.Parse(reader["page"].ToString());
                    book.Language = reader["language"].ToString();
                    book.Publisher = reader["publisher"].ToString();
                    book.PublicationDate = reader["publicationDate"].ToString();
                    book.Picture = Info.Get_BinaryToImg((byte[])reader["picture"]);
                    book.Genre += reader["genre"].ToString() + ", ";
                }
                // remove the last 2 chars (", ")
                book.Genre = book.Genre.Remove(book.Genre.Length - 2);
                reader.Close();
                sqlConnection.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return book;
        }

        public void Send_BookToCart(Book booktoCart, string cusID)
        {
            string existID = null;
            sqlConnection.Open();
            sql = @"SELECT productID FROM Carts WHERE productID = '" + booktoCart.Id + "' AND cusID = '" + cusID + "' ";
            cmd = new SqlCommand(sql, sqlConnection);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                existID = reader.GetString(0);
            }
            reader.Close();
            sqlConnection.Close();

            if (booktoCart.Id == existID)
            {
                //for updating qty
                sqlConnection.Open();
                sql = @"UPDATE Carts
                        SET unit_price = @unit_price,
                        qty = @qty,
                        total_amount = @total_amount
                        WHERE productID = '" + existID + "' AND cusID = '" + cusID + "' ";
                cmd = new SqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@unit_price", booktoCart.Price);
                cmd.Parameters.AddWithValue("@qty", booktoCart.Qty);
                cmd.Parameters.AddWithValue("@total_amount", booktoCart.Qty * booktoCart.Price);
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
            else
            {
                //for inserting to Carts table
                sqlConnection.Open();
                sql = @"INSERT INTO Carts (productID, productName, unit_price, qty, total_amount, picture, cusID)
                    VALUES(@productID, @productName, @unit_price, @qty, @total_amount, @picture, @cusID)";
                cmd = new SqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@productID", booktoCart.Id);
                cmd.Parameters.AddWithValue("@productName", booktoCart.Title);
                cmd.Parameters.AddWithValue("@unit_price", booktoCart.Price);
                cmd.Parameters.AddWithValue("@qty", booktoCart.Qty);
                cmd.Parameters.AddWithValue("@total_amount", booktoCart.Qty * booktoCart.Price);
                cmd.Parameters.AddWithValue("@picture", Info.Get_ImgToBinary(new Bitmap(booktoCart.Picture)));
                cmd.Parameters.AddWithValue("@cusID", cusID);

                cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
            MessageBox.Show("Added to Cart!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
