using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace WebDevAssignment.Model
{
    class SQLCustomerFunctions
    {
        private string connectionString;
        private SqlConnection cnn;

        public SQLCustomerFunctions()
        {
            connectionString = "Data Source=" + SQLDriver.ServerAddress +
                ";Initial Catalog=" + SQLDriver.Database + ";User ID=" + SQLDriver.Username +
                ";Password=" + SQLDriver.Password;
        }
    }
}
