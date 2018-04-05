using System;
using System.Data.SqlClient;
using System.Data;

namespace WebDevAssignment.Model
{

    class SQLController
    {
        public const string ServerAddress = "wdt2018.australiaeast.cloudapp.azure.com";
        public const string Database = "s3539519";
        public const string Username = "s3539519";
        public const string Password = "abc123";

        private string connectionString;
        private SqlConnection cnn;

        public SQLController()
        {
                connectionString = "Data Source=" + SQLController.ServerAddress +
                ";Initial Catalog=" + SQLController.Database + ";User ID=" + SQLController.Username +
                ";Password=" + SQLController.Password;
        }

        public DataTable ExecuteDataRequestReturnTable(string commandString)
        {
            using (cnn = new SqlConnection(connectionString))
            {
                cnn.Open();

                var command = cnn.CreateCommand();
                command.CommandText = commandString;

                var table = new DataTable();
                new SqlDataAdapter(command).Fill(table);
                return table;
            }
            throw new Exception("Error while fetching data.");
        }

        public SqlDataReader ExecuteDataRequestReturnReader(string commandString)
        {
            using (cnn = new SqlConnection(connectionString))
            {
                cnn.Open();

                var command = cnn.CreateCommand();
                command.CommandText = commandString;
                return command.ExecuteReader();
            }
            throw new Exception("Error while fetching data.");
        }

        public int ExecuteNonQueryRequest(string commandString)
        {
            using (cnn = new SqlConnection(connectionString))
            {
                cnn.Open();
                var command = cnn.CreateCommand();
                command.CommandText = commandString;

                return command.ExecuteNonQuery();
            }
            throw new Exception("Error while fetching data.");
        }

    }
}
