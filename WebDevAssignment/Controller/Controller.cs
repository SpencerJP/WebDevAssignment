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
            var content = new List<List<string>>();
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

        public string GetProductNameByID(int id)
        {
            var sql = new SQLController();
            var data = sql.ExecuteDataRequestReturnTable($@"SELECT * FROM Product
                                                           WHERE ProductID = {id}");
            foreach(var x in data.Select())
            {
                Console.WriteLine(x["Name"].ToString());
                return x["Name"].ToString();
            }
            throw new Exception($"A product of the ID {id} does not exist!");
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



        public object GetProducts()
        {
            throw new NotImplementedException();
        }

        public bool BuyProduct(int id)
        {
            throw new NotImplementedException();
        }

        public object GetInventory()
        {
            throw new NotImplementedException();
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
