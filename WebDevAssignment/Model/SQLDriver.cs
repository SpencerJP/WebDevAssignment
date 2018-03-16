using System;
using System.Data.SqlClient;
using System.Data;

namespace WebDevAssignment.Model
{

    
    
    

    class SQLDriver
    {
        public const string ServerAddress = "wdt2018.australiaeast.cloudapp.azure.com";
        public const string Database = "s3539519";
        public const string Username = "s3539519";
        public const string Password = "abc123";

        private SqlConnection cnn;
        private string connectionString;

        public SQLDriver()
        {

         
            connectionString = "Data Source=" + ServerAddress +
                ";Initial Catalog=" + Database + ";User ID=" + Username + 
                ";Password=" + Password;

        }
       

        public DataTable GetStockRequests()
        {
            using (cnn = new SqlConnection(connectionString))
            {
                cnn.Open();

                var command = cnn.CreateCommand();
                command.CommandText = @"SELECT StockRequest.StockRequestID as ID, Store.Name as Store, Product.Name as Product, StockRequest.Quantity as Quantity, StoreInventory.StockLevel as StockLevel
                                        FROM StockRequest
                                        JOIN Store ON StockRequest.StoreID = Store.StoreID
                                        JOIN Product ON StockRequest.ProductID = Product.ProductID
                                        JOIN StoreInventory ON(StoreInventory.StoreID = StockRequest.StoreID AND StockRequest.ProductID = StoreInventory.ProductID); ";

                var table = new DataTable();
                new SqlDataAdapter(command).Fill(table);
                return table;
            }
            throw new Exception("Error while fetching data.");
            
        }
    }
}
