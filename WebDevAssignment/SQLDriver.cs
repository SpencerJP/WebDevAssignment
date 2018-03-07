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
        private string connectionString;

        public SQLDriver()
        {
            connectionString = "Data Source=" + ServerAddress +
                ";Initial Catalog=" + Database + ";User ID=" + Username + 
                ";Password=" + Password;
        }

         private void startConnection()
        {
            cnn = new SqlConnection(connectionString);
            cnn.Open();
        }

        public void testRead()
        {
            try
            {
                this.startConnection();
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("select * from Product", cnn);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    Console.WriteLine(myReader["ProductID"].ToString());
                    Console.WriteLine(myReader["Name"].ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
