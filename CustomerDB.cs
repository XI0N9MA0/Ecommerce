using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ecommercetestttt
{
    public class CustomerDB
    {
        private SqlConnection sqlConnection = EcommerceDB.GetConnection();
        private SqlCommand cmd;
        private SqlDataReader reader;
        private string sql;
        public List<string> Get_AllUsername(List<string> Exist_Username)
        {
            try
            {
                sqlConnection.Open();
                sql = @"SELECT username 
                    FROM Accounts A 
                    LEFT JOIN Customers C ON A.accountID = C.accountID";
                cmd = new SqlCommand(sql, sqlConnection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Exist_Username.Add(reader["username"].ToString());
                }
                reader.Close();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return Exist_Username;
        }

        public List<string> Get_AllPhoneNo(List<string> Exist_PhoneNo)
        {
            try
            {
                sqlConnection.Open();
                sql = @"SELECT phoneNo 
                    FROM Accounts A 
                    LEFT JOIN Customers C ON A.accountID = C.accountID";
                cmd = new SqlCommand(sql, sqlConnection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Exist_PhoneNo.Add(reader["phoneNo"].ToString());
                }
                reader.Close();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return Exist_PhoneNo;
        }

        public void Insert_NewCustomer(Account acc, Customer cus)
        {
            sqlConnection.Open();
            //sql = string.Empty;
            sql = @"INSERT INTO  Accounts(username, password, userType) 
                          VALUES (@username, @password, @userType);
                        
                        INSERT INTO Customers (firstName, lastName, gender, birthdate, address, phoneNo, email, point, profilePicture, accountID)
                          VALUES (@firstName, @lastName, @gender, @birthdate, @address, @phoneNo, @email, @point, @profilePicture, SCOPE_IDENTITY())"
            ;

            cmd = new SqlCommand(sql, sqlConnection);

            //insert into Accounts
            cmd.Parameters.AddWithValue("@username", acc.Username);
            cmd.Parameters.AddWithValue("@password", acc.Password);
            cmd.Parameters.AddWithValue("@userType", acc.UserType);

            //insert into Customers
            cmd.Parameters.AddWithValue("@firstName", cus.FirstName);
            cmd.Parameters.AddWithValue("@LastName", cus.LastName);
            cmd.Parameters.AddWithValue("@gender", cus.Gender);
            cmd.Parameters.AddWithValue("@birthdate", cus.BirthDate);
            cmd.Parameters.AddWithValue("@address", cus.Address);
            cmd.Parameters.AddWithValue("@phoneNo", cus.PhoneNo);
            cmd.Parameters.AddWithValue("@email", cus.Email);
            cmd.Parameters.AddWithValue("@point", cus.Point);
            cmd.Parameters.AddWithValue("@profilePicture", Info.Get_ImgToBinary(cus.ProfilePicture));
            cmd.ExecuteNonQuery();

            sqlConnection.Close();
        }
    }
}
