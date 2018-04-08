using System;
using System.Collections.Generic;
using WebDevAssignment.View;
using WebDevAssignment.Model;
using System.Data.SqlClient;
using System.Data;

namespace WebDevAssignment.Controller
{

    /*
     *  Controller
     *  Main methods for connecting the view and the model
     */

    class Controller
    {

        public Controller()
        {
            MainMenu MainMenu = new MainMenu(this);
        }


        /*
         * @param id productID in OwnerInventory to be reset to 20 
         */
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

        /*
         * @param id StoreID to get name from
         * @returns name of Store with the same id        
         */
        public String SelectStore(int id)
        {
            var sql = new SQLController();
            var data = sql.ExecuteDataRequest($"SELECT Name FROM Store WHERE StoreID={id}");
            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];
                return row["Name"].ToString();
            }
            throw new Exception("Not a valid ID.");
        }


        /*
         * @returns a table of storeIds and store names
         */
        public List<List<String>> GetStores()
        {
            var sql = new SQLController();
            var content = new List<List<string>>();
            var data = sql.ExecuteDataRequest(@"SELECT * FROM Store");
            foreach (var x in data.Select())
            {
                content.Add(new List<string> { x["StoreID"].ToString(), x["Name"].ToString() });
            }
            return content;
        }

        /*
         * @returns a table of ProductIDs, Product Names, and StockLevels from OwnerInventory
         */
        public List<List<string>> GetOwnerInventory()
        {
            var sql = new SQLController();
            var content = new List<List<string>>();
            var data = sql.ExecuteDataRequest(@"SELECT OwnerInventory.ProductID as ID, Product.Name as Product, OwnerInventory.StockLevel
                                        FROM OwnerInventory
                                        JOIN Product ON Product.ProductID = OwnerInventory.ProductID");
            foreach (var x in data.Select())
            {
                content.Add(new List<string> { x["ID"].ToString(), x["Product"].ToString(), x["StockLevel"].ToString()});
            }
            return content;

        }

        /*
         * @param id StoreID to retrieve from
         * @return Table of ProductID, Product Name, and Stocklevel from a specific store
         */
        public List<List<String>> GetStoreInventory(int id)
        {
            var sql = new SQLController();
            var content = new List<List<string>>();
            var data = sql.ExecuteDataRequest($@"SELECT StoreInventory.StoreID, StoreInventory.ProductID as ID, Product.Name as Product, StoreInventory.StockLevel as StockLevel
                                        FROM StoreInventory
                                        JOIN Product ON Product.ProductID = StoreInventory.ProductID
                                        WHERE StoreInventory.StoreID = {id}");
            foreach (var x in data.Select())
            {
                content.Add(new List<string> { x["ID"].ToString(), x["Product"].ToString(), x["StockLevel"].ToString() });
            }
            return content;
        }

        /*
         * @param productID to get name from
         * @returns string of product name 
         */
        public string GetProductNameByID(int id)
        {
            var sql = new SQLController();
            var data = sql.ExecuteDataRequest($@"SELECT * FROM Product
                                                           WHERE ProductID = {id}");
            foreach(var x in data.Select())
            {
                return x["Name"].ToString();
            }
            throw new Exception($"A product of the ID {id} does not exist!");
        }


       /*
        * @param storeID to check 
        * @param quantity threshold to get items below
        * @returns table of productID, product name, and stocklevel below that threshold in that store
        */
        public List<List<String>> GetStockBelowThreshold(int id, int threshold)
        {
            if (threshold < 1)
            {
                throw new Exception("Threshold can not be 0 or lower!");
            }
            var sql = new SQLController();
            var content = new List<List<string>>();
            var data = sql.ExecuteDataRequest($@"SELECT StoreInventory.StoreID, StoreInventory.ProductID as ID, Product.Name as Product, StoreInventory.StockLevel as StockLevel
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
        /*
         * @returns table of StockRequestID, Store Name, Product Name, Quantity and StockLevel 
         */ 
        public List<List<string>> GetStockRequests()
        {
            var sql = new SQLController();
            var content = new List<List<string>>();
            var data = sql.ExecuteDataRequest(@"SELECT StockRequest.StockRequestID as ID, Store.Name as Store, Product.Name as Product, StockRequest.Quantity as Quantity, OwnerInventory.StockLevel as StockLevel
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

        /*
         * @param storeID to create it for
         * @param productId to create it for
         * @param quantity to request
         */
        public void CreateRequest(int storeID, int productID, int quantity)
        {
            if (quantity < 1)
            {
                throw new Exception("Quantity can not be be 0 or lower!");
            }
            var sql = new SQLController();
            var rowsAffected = sql.ExecuteNonQueryRequest($@"INSERT INTO StockRequest VALUES({storeID},
                                                            {productID},
                                                            {quantity})");
            if (rowsAffected != 1)
            {
                throw new Exception("Invalid product ID.");
            }
        }

        /*
         * @param storeID
         * @param productID
         * Creates a request of 1 quantity of the product with id productID, the request is from storeid. This method checks first that this store does not have that item.
         */
        public void AddNewInventoryItem(int storeID, int productID)
        {
            try
            {
                // this will throw an exception if productID is not valid
                GetProductNameByID(productID);
            }
            catch(Exception)
            {
                throw new Exception($"There is no product with an id of {productID}");
            }

            var sql = new SQLController();
            var data = sql.ExecuteDataRequest($@" SELECT OwnerInventory.ProductID
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

        /*
         * @param id Store to retrieve items not in stock from
         * @returns table of products that are not stocked by the store
         */
        public List<List<String>> GetItemsNotInStock(int id)
        {
            var sql = new SQLController();
            var content = new List<List<string>>();

            var data = sql.ExecuteDataRequest($@" SELECT OwnerInventory.ProductID, Product.Name, OwnerInventory.StockLevel
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

        /*
         * @param storeid to buy from
         * @param productID that is being bought
         * @param quantity to buy of that productid
         * 
         */
        public void BuyProduct(int store, int productID, int quantity)
        {
            if (quantity < 1)
            {
                throw new Exception("Quantity can not be 0 or lower. That wouldn't make sense!");
            }
            var sql = new SQLController();
            var content = new List<List<string>>();
            var rowsAffected = sql.ExecuteNonQueryRequest($@"UPDATE StoreInventory
                                                            SET StockLevel = StockLevel - {quantity}
                                                            WHERE ProductID = {productID} AND StoreID = {store} AND StockLevel >= {quantity}");
            if (rowsAffected != 1)
            {
                throw new Exception($"There is not enough stock for this quantity of {GetProductNameByID(productID)}! Sorry about that!");
            }

        }

        public bool ProcessStockRequest(int id)
        {
            var sql = new SQLController();
            int quantity = -1, stocklevel = -1, productID = -1, storeID = -1, stockrequestID = -1;

            using (var data = sql.ExecuteDataRequest($@"SELECT StockRequest.StockRequestID as ID, Store.StoreID as StoreID, Product.ProductID as ProductID, StockRequest.Quantity as Quantity, OwnerInventory.StockLevel as StockLevel
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
                        // checks if everything was found
                        throw new Exception("Something went wrong when reading the data from SQL.");
                    }
                    if (quantity > stocklevel) // not enough stock
                    {
                        return false;
                    }
                    var newOwnerStock = (int)stocklevel - (int)quantity;

                   // change stock level in ownerinventory
                    sql.ExecuteNonQueryRequest($@"UPDATE OwnerInventory
                                                       SET OwnerInventory.StockLevel = OwnerInventory.StockLevel - {quantity}
                                                       WHERE OwnerInventory.ProductID = {productID}
                                                       ");

                    // change stock level in store inventory (if theres 0 stock it creates a new record instead)
                    sql.ExecuteNonQueryRequest($@"IF EXISTS (SELECT * FROM StoreInventory 
                                                            WHERE (StoreInventory.ProductID = {productID} AND StoreInventory.StoreID = {storeID}) )
                                                     UPDATE StoreInventory 
                                                     SET StoreInventory.StockLevel = StoreInventory.StockLevel + {quantity}
                                                     WHERE (StoreInventory.ProductID = {productID} AND StoreInventory.StoreID = {storeID})
                                                 ELSE
                                                     INSERT INTO StoreInventory VALUES({storeID}, {productID}, {quantity})");

                    // removes stockrequest once complete
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
