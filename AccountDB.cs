using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommercetestttt
{
    public class AccountDB
    {
        private SqlConnection sqlConnection = EcommerceDB.GetConnection();
        private SqlCommand cmd;
        private SqlDataReader reader;
        private string sql;
        public Login Get_LoginInfo(Login login)
        {
            sqlConnection.Open();
            sql = "SELECT * FROM Accounts WHERE username= @username AND password= @password";
            cmd = new SqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@username", login.Username);
            cmd.Parameters.AddWithValue("@password", login.Password);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                login.AccountID = reader["accountID"].ToString();
                login.UserType = reader["userType"].ToString();
                login.Username = reader["username"].ToString();
            }
            reader.Close();
            sqlConnection.Close();
            return login;
        }

        public ForgetPassword retreiveFGCustomer_AccID(ForgetPassword FGCustomer)
        {
            sqlConnection.Open();
            sql = @"SELECT A.accountID, username 
                        FROM Accounts A JOIN Customers C ON A.accountID = C.accountID
                        WHERE phoneNo = " + "'" + Info.GetPhone_whitespace(FGCustomer.PhoneNo) + "' ";
            cmd = new SqlCommand(sql, sqlConnection);
            reader = cmd.ExecuteReader();


            if (reader.Read())
            {
                FGCustomer.AccID = reader["accountID"].ToString();
                FGCustomer.Username = reader["username"].ToString();
            }
            reader.Close();
            sqlConnection.Close();
            return FGCustomer;
        }

        public void ResetPassword(string password, string accID)
        {
            sqlConnection.Open();
            sql = @"UPDATE Accounts SET password = @password WHERE accountID = " + accID;
            cmd = new SqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.ExecuteNonQuery();
            sqlConnection.Close();
        }
    }
}
