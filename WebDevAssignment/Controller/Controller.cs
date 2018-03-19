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
            throw new NotImplementedException();
        }

        public object DisplayStock()
        {
            throw new NotImplementedException();
        }

        public List<List<string>> GetOwnerInventory()
        {
            var sql = new SQLDriver();
            var content = new List<List<string>>();
            var data = sql.GetOwnerInventory();
            foreach (var x in data.Select())
            {
                content.Add(new List<string> { x["ID"].ToString(), x["Product"].ToString(), x["StockLevel"].ToString()});
            }
            return content;

        }

        public List<List<string>> GetStockRequests()
        {

            var sql = new SQLDriver();
            var content = new List<List<string>>();
            var data = sql.GetStockRequests();

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
            var sql = new SQLDriver();

            var success = sql.GetAndUpdateSpecificStockRequest(id);
            return success;
        }
    }
}
