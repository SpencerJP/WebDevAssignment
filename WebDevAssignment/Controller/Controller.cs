using System;
using System.Collections.Generic;
using System.Text;
using WebDevAssignment.View;
using WebDevAssignment.Model;
using System.Data.SqlClient;
using System.Data;

namespace WebDevAssignment.Controller
{
    class Controller
    {

        public Controller()
        {
            MainMenu MainMenu = new MainMenu(this);
        }

        public bool ResetInventoryStock(int id)
        {
            var sql = new SQLController();
            var linesAffected = sql.ExecuteNonQueryRequest($@"UPDATE OwnerInventory
                                          SET StockLevel = 20
                                          WHERE ProductID = {id} AND StockLevel < 20"); // safe because it is an int
            if (linesAffected == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public String SelectStore(int id)
        {
            var sql = new SQLController();
            var data = sql.ExecuteDataRequestReturnTable($"SELECT Name FROM Store WHERE StoreID={id}");
            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];
                return row["Name"].ToString();
            }
            throw new Exception("Not a valid ID.");
        }

        public List<List<String>> GetStores()
        {
            var sql = new SQLController();
            var content = new List<List<string>>();
            var data = sql.ExecuteDataRequestReturnTable(@"SELECT * FROM Store");
            foreach (var x in data.Select())
            {
                content.Add(new List<string> { x["StoreID"].ToString(), x["Name"].ToString() });
            }
            return content;
        }

        public List<List<string>> GetOwnerInventory()
        {
            var sql = new SQLController();
            var content = new List<List<string>>();
            var data = sql.ExecuteDataRequestReturnTable(@"SELECT OwnerInventory.ProductID as ID, Product.Name as Product, OwnerInventory.StockLevel
                                        FROM OwnerInventory
                                        JOIN Product ON Product.ProductID = OwnerInventory.ProductID");
            foreach (var x in data.Select())
            {
                content.Add(new List<string> { x["ID"].ToString(), x["Product"].ToString(), x["StockLevel"].ToString()});
            }
            return content;

        }

        public List<List<String>> GetStoreInventory(int id)
        {
            var sql = new SQLController();
            var content = new List<List<string>>();
            var data = sql.ExecuteDataRequestReturnTable($@"SELECT StoreInventory.StoreID, StoreInventory.ProductID as ID, Product.Name as Product, StoreInventory.StockLevel as StockLevel
                                        FROM StoreInventory
                                        JOIN Product ON Product.ProductID = StoreInventory.ProductID
                                        WHERE StoreInventory.StoreID = {id}");
            foreach (var x in data.Select())
            {
                content.Add(new List<string> { x["ID"].ToString(), x["Product"].ToString(), x["StockLevel"].ToString() });
            }
            return content;
        }

        public string GetProductNameByID(int id)
        {
            var sql = new SQLController();
            var data = sql.ExecuteDataRequestReturnTable($@"SELECT * FROM Product
                                                           WHERE ProductID = {id}");
            foreach(var x in data.Select())
            {
                return x["Name"].ToString();
            }
            throw new Exception($"A product of the ID {id} does not exist!");
        }

        public List<List<String>> GetStockBelowThreshold(int id, int threshold)
        {
            var sql = new SQLController();
            var content = new List<List<string>>();
            var data = sql.ExecuteDataRequestReturnTable($@"SELECT StoreInventory.StoreID, StoreInventory.ProductID as ID, Product.Name as Product, StoreInventory.StockLevel as StockLevel
                                        FROM StoreInventory
                                        JOIN Product ON Product.ProductID = StoreInventory.ProductID
                                        WHERE StoreInventory.StoreID = {id} AND StockLevel < { threshold }");
            int i = 0;
            foreach (var x in data.Select())
            {
                content.Add(new List<string> { x["ID"].ToString(), x["Product"].ToString(), x["StockLevel"].ToString() });
                i++;
            }
            if (i < 1)
            {
                throw new Exception("No products are below the threshold.");
            }
            return content;
        }

        public List<List<string>> GetStockRequests()
        {
            var sql = new SQLController();
            var content = new List<List<string>>();
            var data = sql.ExecuteDataRequestReturnTable(@"SELECT StockRequest.StockRequestID as ID, Store.Name as Store, Product.Name as Product, StockRequest.Quantity as Quantity, OwnerInventory.StockLevel as StockLevel
                                        FROM StockRequest
                                        JOIN Store ON StockRequest.StoreID = Store.StoreID
                                        JOIN Product ON StockRequest.ProductID = Product.ProductID
                                        JOIN OwnerInventory ON StockRequest.ProductID = OwnerInventory.ProductID; ");

            foreach (var x in data.Select())
            {
                content.Add(new List<string> { x["ID"].ToString(), x["Store"].ToString(), x["Product"].ToString(), x["Quantity"].ToString(), x["StockLevel"].ToString() });
            }
            return content;

        }

        public void CreateRequest(int storeID, int productID, int quantity)
        {
            var sql = new SQLController();
            var content = new List<List<string>>();
            var rowsAffected = sql.ExecuteNonQueryRequest($@"INSERT INTO StockRequest VALUES({storeID},
                                                            {productID},
                                                            {quantity})");
            if (rowsAffected != 1)
            {
                throw new Exception("Invalid product ID.");
            }
        }
        
        public void AddNewInventoryItem(int storeID, int productID)
        {
            var sql = new SQLController();
            var data = sql.ExecuteDataRequestReturnTable($@" SELECT OwnerInventory.ProductID
                                                            FROM OwnerInventory
                                                            WHERE OwnerInventory.ProductID IN
                                                            (SELECT ProductID
                                                            FROM Product
                                                            EXCEPT
                                                            SELECT ProductID FROM StoreInventory WHERE StoreInventory.StoreID = {storeID})" );
            foreach (var x in data.Select())
            {
                if (productID == (int) x["ProductID"])
                {
                    CreateRequest(storeID, productID, 1);
                    return;
                }
            }
            throw new Exception("Failed to create a stock request - This store already has that item in stock.");
        }

        public List<List<String>> GetItemsNotInStock(int id)
        {
            var sql = new SQLController();
            var content = new List<List<string>>();

            var data = sql.ExecuteDataRequestReturnTable($@" SELECT OwnerInventory.ProductID, Product.Name, OwnerInventory.StockLevel
                                                            FROM OwnerInventory
                                                            INNER JOIN Product ON OwnerInventory.ProductID = Product.ProductID
                                                            WHERE OwnerInventory.ProductID IN
                                                            (SELECT ProductID
                                                            FROM Product
                                                            EXCEPT
                                                            SELECT ProductID FROM StoreInventory WHERE StoreInventory.StoreID = {id}) ");

            foreach (var x in data.Select())
            {
                content.Add(new List<string> { x["ProductID"].ToString(), x["Name"].ToString(), x["StockLevel"].ToString() });
            }
            return content;
        }

        public bool BuyProduct(int store, int id, int quantity)
        {
            
        }

        public bool ProcessStockRequest(int id)
        {
            var sql = new SQLController();
            int quantity = -1, stocklevel = -1, productID = -1, storeID = -1, stockrequestID = -1;

            using (var data = sql.ExecuteDataRequestReturnTable($@"SELECT StockRequest.StockRequestID as ID, Store.StoreID as StoreID, Product.ProductID as ProductID, StockRequest.Quantity as Quantity, OwnerInventory.StockLevel as StockLevel
                                        FROM StockRequest
                                        JOIN Store ON StockRequest.StoreID = Store.StoreID
                                        JOIN Product ON StockRequest.ProductID = Product.ProductID
                                        JOIN OwnerInventory ON StockRequest.ProductID = OwnerInventory.ProductID
                                        WHERE StockRequestID = {id}; ")) // safe because it has to be an int
            {

                try
                {
                    if (data.Rows.Count > 0)
                    {
                        DataRow row = data.Rows[0];

                        quantity = (int) row["Quantity"];
                        stocklevel = (int) row["StockLevel"];
                        productID = (int) row["ProductID"];
                        stockrequestID = (int)row["ID"];
                        storeID = (int) row["StoreID"];

                    }
                    if (quantity == -1)
                    {
                        throw new Exception("Something went wrong when reading the data from SQL.");
                    }
                    if (quantity > stocklevel) // not enough stock
                    {
                        return false;
                    }
                    var newOwnerStock = (int)stocklevel - (int)quantity;

                   
                    sql.ExecuteNonQueryRequest($@"UPDATE OwnerInventory
                                                       SET OwnerInventory.StockLevel = OwnerInventory.StockLevel - {quantity}
                                                       WHERE OwnerInventory.ProductID = {productID}
                                                       ");

                    sql.ExecuteNonQueryRequest($@"IF EXISTS (SELECT * FROM StoreInventory 
                                                            WHERE (StoreInventory.ProductID = {productID} AND StoreInventory.StoreID = {storeID}) )
                                                     UPDATE StoreInventory 
                                                     SET StoreInventory.StockLevel = StoreInventory.StockLevel + {quantity}
                                                     WHERE (StoreInventory.ProductID = {productID} AND StoreInventory.StoreID = {storeID})
                                                 ELSE
                                                     INSERT INTO StoreInventory VALUES({storeID}, {productID}, {quantity})");
                    sql.ExecuteNonQueryRequest($@"DELETE FROM StockRequest
                                                 WHERE StockRequest.StockRequestID = {stockrequestID}");

                    return true;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
        }
    }
}
