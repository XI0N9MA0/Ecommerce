using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommercetestttt
{
    public static class EcommerceDB
    {
        public static SqlConnection GetConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-4FMPOU4\SQLEXPRESS;Initial Catalog=dbECommerce;Integrated Security=True");
            return sqlConnection;
        }
        
    }
}
