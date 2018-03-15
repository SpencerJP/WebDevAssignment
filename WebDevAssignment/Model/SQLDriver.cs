using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace WebDevAssignment.Model
{
    class SQLDriver
    {
        private SqlConnection cnn;
        private string ServerAddress = "wdt2018.australiaeast.cloudapp.azure.com";
        private string Database = "s3539519";
        private string Username = "s3539519";
        private string Password = "abc123";
        private string connectionString;

        private Controller.Controller c;

        public SQLDriver(Controller.Controller controller)
        {
            connectionString = "Data Source=" + ServerAddress +
                ";Initial Catalog=" + Database + ";User ID=" + Username + 
                ";Password=" + Password;
            c = controller;
        }
       

        public object GetStockRequests()
        {
            using (cnn = new SqlConnection(connectionString))
            {
                cnn.Open();

                var command = cnn.CreateCommand();
                command.CommandText = @"SELECT StockRequest.StockRequestID, Store.Name, Product.Name, StockRequest.Quantity, StoreInventory.StockLevel
                                        FROM StockRequest
                                        JOIN Store ON StockRequest.StoreID = Store.StoreID
                                        JOIN Product ON StockRequest.ProductID = Product.ProductID
                                        JOIN StoreInventory ON(StoreInventory.StoreID = StockRequest.StoreID AND StockRequest.ProductID = StoreInventory.ProductID); ";

                var table = new DataTable();
                new SqlDataAdapter(command).Fill(table);
                
            }
            
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

        public void 
    }
}
