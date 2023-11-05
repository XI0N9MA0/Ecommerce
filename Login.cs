using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommercetestttt
{
    public class Login
    {
        private string username;
        private string password;
        private string userType;
        private string accountID;

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string UserType { get => userType; set => userType = value; }
        public string AccountID { get => accountID; set => accountID = value; }
    }
}
