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

            using (var reader = sql.ExecuteDataRequestReturnReader($@"SELECT StockRequest.StockRequestID as ID, Store.StoreID as StoreID, Product.ProductID as ProductID, StockRequest.Quantity as Quantity, OwnerInventory.StockLevel as StockLevel
                                        FROM StockRequest
                                        JOIN Store ON StockRequest.StoreID = Store.StoreID
                                        JOIN Product ON StockRequest.ProductID = Product.ProductID
                                        JOIN OwnerInventory ON StockRequest.ProductID = OwnerInventory.ProductID
                                        WHERE StockRequestID = {id}; ")) 
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
