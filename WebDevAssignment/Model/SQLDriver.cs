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

        private string connectionString;
        private SqlConnection cnn;

        public SQLDriver()
        {
                connectionString = "Data Source=" + SQLDriver.ServerAddress +
                ";Initial Catalog=" + SQLDriver.Database + ";User ID=" + SQLDriver.Username +
                ";Password=" + SQLDriver.Password;
        }

        public DataTable GetStockRequests()
        {
            using (cnn = new SqlConnection(connectionString))
            {
                cnn.Open();

                var command = cnn.CreateCommand();
                command.CommandText = @"SELECT StockRequest.StockRequestID as ID, Store.Name as Store, Product.Name as Product, StockRequest.Quantity as Quantity, OwnerInventory.StockLevel as StockLevel
                                        FROM StockRequest
                                        JOIN Store ON StockRequest.StoreID = Store.StoreID
                                        JOIN Product ON StockRequest.ProductID = Product.ProductID
                                        JOIN OwnerInventory ON StockRequest.ProductID = OwnerInventory.ProductID; ";

                var table = new DataTable();
                new SqlDataAdapter(command).Fill(table);
                return table;
            }
            throw new Exception("Error while fetching data.");
        }

        public DataTable GetOwnerInventory()
        {
            using (cnn = new SqlConnection(connectionString))
            {
                cnn.Open();

                var command = cnn.CreateCommand();
                command.CommandText = @"SELECT OwnerInventory.ProductID as ID, Product.Name as Product, OwnerInventory.StockLevel
                                        FROM OwnerInventory
                                        JOIN Product ON Product.ProductID = OwnerInventory.ProductID";

                var table = new DataTable();
                new SqlDataAdapter(command).Fill(table);
                return table;
            }
            throw new Exception("Error while fetching data.");
        }

        public bool GetAndUpdateSpecificStockRequest(int id)
        {
            using (cnn = new SqlConnection(connectionString))
            {
                cnn.Open();

                var command = new SqlCommand($@"SELECT StockRequest.StockRequestID as ID, Store.StoreID as StoreID, Product.ProductID as ProductID, StockRequest.Quantity as Quantity, OwnerInventory.StockLevel as StockLevel
                                        FROM StockRequest
                                        JOIN Store ON StockRequest.StoreID = Store.StoreID
                                        JOIN Product ON StockRequest.ProductID = Product.ProductID
                                        JOIN OwnerInventory ON StockRequest.ProductID = OwnerInventory.ProductID
                                        WHERE StockRequestID = {id}; ", cnn);
                using (var reader = command.ExecuteReader())
                {
                    
                    try
                    {
                        reader.Read();
                        var quantity = reader["Quantity"];
                        var stocklevel = reader["StockLevel"];
                        var productID = reader["ProductID"];
                        var stockrequestID = reader["ID"];
                        var storeID = reader["StoreID"];
                        reader.Close();
                        if ((int)quantity > (int)stocklevel) // not enough stock
                        {
                            return false;
                        }
                        var newOwnerStock = (int)stocklevel - (int)quantity;

                        // Updates the "Description" field in the "Status" table.
                        command.CommandText = $@"UPDATE OwnerInventory
                                                       SET OwnerInventory.StockLevel = OwnerInventory.StockLevel - {quantity}
                                                       WHERE OwnerInventory.ProductID = {productID}
                                                       ";

                        var updates = command.ExecuteNonQuery();
                        command.CommandText = $@"IF EXISTS (SELECT * FROM StoreInventory 
                                                            WHERE (StoreInventory.ProductID = {productID} AND StoreInventory.StoreID = {storeID}) )
                                                     UPDATE StoreInventory 
                                                     SET StoreInventory.StockLevel = StoreInventory.StockLevel + {quantity}
                                                     WHERE (StoreInventory.ProductID = {productID} AND StoreInventory.StoreID = {storeID})
                                                 ELSE
                                                     INSERT INTO StoreInventory VALUES({storeID}, {productID}, {quantity})
                                                       ";
                        updates = command.ExecuteNonQuery();

                        command.CommandText = $@"DELETE FROM StockRequest
                                                 WHERE StockRequest.StockRequestID = {stockrequestID}";
                        updates = command.ExecuteNonQuery();

                        return true;
                    }
                    catch(Exception e)
                    {
                        return false;
                    }
                }
            }
            throw new Exception("Error while fetching data.");
        }
    }
}
