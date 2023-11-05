using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommercetestttt
{
    public class Account
    {
        private string accountID;
        private string username;
        private string password;
        private string cpassword;
        private string userType;

        public Account()
        {
            AccountID = null;
            Username = null;
            Password = null;
            Cpassword = null;
            UserType = null;
        }

        public string AccountID { get => accountID; set => accountID = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Cpassword { get => cpassword; set => cpassword = value; }
        public string UserType { get => userType; set => userType = value; }
    }
}
