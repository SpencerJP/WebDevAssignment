using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace WebDevAssignment
{
    class SQLDriver
    {
        private SqlConnection cnn;
        private string ServerAddress = "wdt2018.australiaeast.cloudapp.azure.com";
        private string Database = "s3539519";
        private string Username = "s3539519";
        private string Password = "abc123";

         private void startConnection()
        {
            cnn = new SqlConnection("user id=" + Username + ";" +
                    "password=" + Password + ";server=" + ServerAddress + ";" +
                    "Trusted_Connection=yes;" +
                    "database=" + Username + "; " +
                    "connection timeout=30");
            cnn.Open();
        }
    }
}
