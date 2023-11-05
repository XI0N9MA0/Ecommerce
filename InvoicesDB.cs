using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommercetestttt
{
    public class InvoicesDB
    {
        private SqlConnection sqlConnection = EcommerceDB.GetConnection();
        private SqlCommand cmd;
        private SqlDataReader reader;
        private string sql;


    }
}
