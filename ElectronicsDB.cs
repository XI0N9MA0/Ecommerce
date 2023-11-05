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
    public class ElectronicsDB
    {
        private SqlConnection sqlConnection = EcommerceDB.GetConnection();
        private SqlCommand cmd;
        private SqlDataReader reader;
        private string sql;

        public void Delete_Data_From_Electronics(Electronic electronic)
        {
            sqlConnection.Open();
            sql = @"DELETE FROM Electronics WHERE electronicID ='" + electronic.Id + "'";
            cmd = new SqlCommand(sql, sqlConnection);
            cmd.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public void UpdateElectronics(Electronic electronic)
        {
            try
            {
                sqlConnection.Open();
                sql = @"UPDATE Electronics 
                        SET    [electronicName] = @electronicName
                              ,[color] = @color
                              ,[releaseDate] = @releaseDate
                              ,[price]  = @price
                              ,[qty] = @qty
                              ,[picture] = @picture
                              ,[description] = @description
                              ,[inStock_Status] = @inStock_Status
                              ,[electronicTypeID] = @electronicTypeID
                              ,[size] = @size
                              ,[company] = @company
                        WHERE electronicID = '" + electronic.Id + "'";
                cmd = new SqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@electronicName", electronic.Name);
                cmd.Parameters.AddWithValue("@color", electronic.Color);
                cmd.Parameters.AddWithValue("@releaseDate", electronic.ReleaseDate);
                cmd.Parameters.AddWithValue("@price", electronic.Price);
                cmd.Parameters.AddWithValue("@qty", electronic.Qty);
                cmd.Parameters.AddWithValue("@size", electronic.Size);
                cmd.Parameters.AddWithValue("@company", electronic.Company);
                cmd.Parameters.AddWithValue("@description", electronic.Description);
                cmd.Parameters.AddWithValue("@inStock_Status", electronic.InStockStatus);
                cmd.Parameters.AddWithValue("@electronicTypeID", electronic.ElectronicTypeID);
                cmd.Parameters.AddWithValue("@picture", Info.Get_ImgToBinary(electronic.Picture));
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public int Get_ElectronicTypeID(string electronicType)
        {
            int electronicID = 0;
            sqlConnection.Open();
            sql = @"SELECT electronicTypeID FROM ElectronicTypes WHERE type = '" + electronicType + "'";
            cmd = new SqlCommand(sql, sqlConnection);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                electronicID = int.Parse(reader["electronicTypeID"].ToString());
            }
            sqlConnection.Close();
            return electronicID;
        }

        public void Insert_IntoElectronic(Electronic electronic)
        {
            sqlConnection.Open();
            sql = @"INSERT INTO Electronics(
                               [electronicID]
	                          ,[electronicName]
                              ,[color]
                              ,[releaseDate]
                              ,[price]
                              ,[qty]
                              ,[picture]
                              ,[description]
                              ,[inStock_Status]
                              ,[rate]
                              ,[totalRate]
                              ,[soldQty]
                              ,[electronicTypeID]
                              ,[size]
                              ,[company])
                    VALUES(
                               @electronicID
	                          ,@electronicName
                              ,@color
                              ,@releaseDate
                              ,@price
                              ,@qty
                              ,@picture
                              ,@description
                              ,@inStock_Status
                              ,@rate
                              ,@totalRate
                              ,@soldQty
                              ,@electronicTypeID
                              ,@size
                              ,@company)"
            ;

            cmd = new SqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@electronicID", electronic.Id);
            cmd.Parameters.AddWithValue("@electronicName", electronic.Name);
            cmd.Parameters.AddWithValue("@color", electronic.Color);
            cmd.Parameters.AddWithValue("@releaseDate", electronic.ReleaseDate);
            cmd.Parameters.AddWithValue("@price", electronic.Price);
            cmd.Parameters.AddWithValue("@qty", electronic.Qty);
            cmd.Parameters.AddWithValue("@picture", Info.Get_ImgToBinary(electronic.Picture));
            cmd.Parameters.AddWithValue("@description", electronic.Description);
            cmd.Parameters.AddWithValue("@inStock_Status", electronic.InStockStatus);
            cmd.Parameters.AddWithValue("@rate", electronic.Rate);
            cmd.Parameters.AddWithValue("@totalRate", electronic.TotalRate);
            cmd.Parameters.AddWithValue("@soldQty", electronic.SoldQty);
            cmd.Parameters.AddWithValue("@electronicTypeID", electronic.ElectronicTypeID);
            cmd.Parameters.AddWithValue("@size", electronic.Size);
            cmd.Parameters.AddWithValue("@company", electronic.Company);
            cmd.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public void Search_ElecForManagement(ref Electronic electronic, frmElecManagement frmElecM)
        {
            sqlConnection.Open();
            sql = @"SELECT *
                    FROM Electronics E  
                    JOIN ElectronicTypes T
                    ON E.electronicTypeID = T.electronicTypeID
                    WHERE E.electronicID = '" + electronic.Id + "'";
            cmd = new SqlCommand(sql, sqlConnection);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                if (electronic.Id == reader["electronicID"].ToString())
                {

                    frmElecM.btnUpdate.Enabled = true;
                    frmElecM.btnDelete.Enabled = true;
                    frmElecM.btnAdd.Enabled = false;

                    electronic.Name = reader["electronicName"].ToString();
                    electronic.Price = decimal.Parse(reader["price"].ToString());
                    electronic.Color = reader["color"].ToString();
                    electronic.Qty = int.Parse(reader["qty"].ToString());
                    electronic.Company = reader["company"].ToString();
                    electronic.Size = decimal.Parse(reader["size"].ToString());
                    electronic.ReleaseDate = reader["releaseDate"].ToString();
                    electronic.Description = reader["description"].ToString();
                    electronic.Type = reader["type"].ToString();
                    //ElecImageBx.Image is assigned by new Bitmap to avoid generic error occurred in gdi+
                    electronic.Picture = new Bitmap(Info.Get_BinaryToImg((byte[])reader["picture"]));
                }
            }

            sqlConnection.Close();
        }

        public Electronic Get_Electronic(string elecID)
        {
            Electronic electronic = new Electronic();
            try
            {
                sqlConnection.Open();
                sql = @"SELECT *
                        FROM Electronics E
                        JOIN ElectronicTypes T
                        ON E.electronicTypeID = T.electronicTypeID
                        WHERE electronicID ='" + elecID + "'";
                cmd = new SqlCommand(sql, sqlConnection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    electronic.Id = reader["electronicID"].ToString();
                    electronic.Name = reader["electronicName"].ToString();
                    electronic.Price = decimal.Parse(reader["price"].ToString());
                    electronic.Description = reader["description"].ToString();
                    electronic.Qty = int.Parse(reader["qty"].ToString());
                    electronic.Rate = decimal.Parse(reader["rate"].ToString());
                    electronic.TotalRate = int.Parse(reader["totalRate"].ToString());
                    electronic.Company = reader["company"].ToString();
                    electronic.InStockStatus = bool.Parse(reader["inStock_Status"].ToString());
                    electronic.Color = reader["color"].ToString();
                    electronic.Size = decimal.Parse(reader["size"].ToString());
                    electronic.ReleaseDate = reader["releaseDate"].ToString();
                    electronic.Type = reader["type"].ToString();
                    electronic.Picture = Info.Get_BinaryToImg((byte[])reader["picture"]);

                }
                reader.Close();
                sqlConnection.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return electronic;
        }

        public void Send_ElecToCart(Electronic electoCart, string cusID)
        {
            string existID = null;
            sqlConnection.Open();
            sql = @"SELECT productID FROM Carts WHERE productID = '" + electoCart.Id + "' AND cusID = '" + cusID + "' ";
            cmd = new SqlCommand(sql, sqlConnection);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                existID = reader.GetString(0);
            }

            reader.Close();
            sqlConnection.Close();

            if (electoCart.Id == existID)
            {
                sqlConnection.Open();
                sql = @"UPDATE Carts
                        SET unit_price = @unit_price,
                        qty = @qty,
                        total_amount = @total_amount
                        WHERE productID = '" + existID + "' AND cusID = '" + cusID + "' ";
                cmd = new SqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@unit_price", electoCart.Price);
                cmd.Parameters.AddWithValue("@qty", electoCart.Qty);
                cmd.Parameters.AddWithValue("@total_amount", electoCart.Qty * electoCart.Price);
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
            else
            {
                sqlConnection.Open();
                sql = @"INSERT INTO Carts (productID, productName, unit_price, qty, total_amount, picture, cusID)
                    VALUES(@productID, @productName, @unit_price, @qty, @total_amount, @picture, @cusID)";
                cmd = new SqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@productID", electoCart.Id);
                cmd.Parameters.AddWithValue("@productName", electoCart.Name);
                cmd.Parameters.AddWithValue("@unit_price", electoCart.Price);
                cmd.Parameters.AddWithValue("@qty", electoCart.Qty);
                cmd.Parameters.AddWithValue("@total_amount", electoCart.Qty * electoCart.Price);
                cmd.Parameters.AddWithValue("@picture", Info.Get_ImgToBinary(new Bitmap(electoCart.Picture)));
                cmd.Parameters.AddWithValue("@cusID", cusID);

                cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
            MessageBox.Show("Added to Cart!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
