﻿using System;
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
                        ExecuteNonQueryRequest($@"UPDATE OwnerInventory
                                                       SET OwnerInventory.StockLevel = OwnerInventory.StockLevel - {quantity}
                                                       WHERE OwnerInventory.ProductID = {productID}
                                                       ");
                        
                        ExecuteNonQueryRequest($@"IF EXISTS (SELECT * FROM StoreInventory 
                                                            WHERE (StoreInventory.ProductID = {productID} AND StoreInventory.StoreID = {storeID}) )
                                                     UPDATE StoreInventory 
                                                     SET StoreInventory.StockLevel = StoreInventory.StockLevel + {quantity}
                                                     WHERE (StoreInventory.ProductID = {productID} AND StoreInventory.StoreID = {storeID})
                                                 ELSE
                                                     INSERT INTO StoreInventory VALUES({storeID}, {productID}, {quantity})");
                        ExecuteNonQueryRequest($@"DELETE FROM StockRequest
                                                 WHERE StockRequest.StockRequestID = {stockrequestID}");

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
