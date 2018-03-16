using System;
using System.Collections.Generic;
using System.Text;
using WebDevAssignment.View;
using WebDevAssignment.Model;

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

        public object GetOwnerInventory()
        {
            throw new NotImplementedException();
        }

        public List<List<string>> GetStockRequests()
        {

            SQLDriver SQL = new SQLDriver();
            var content = new List<List<string>>();
            var data = SQL.GetStockRequests();

            foreach (var x in data.Select())
            {
                content.Add(new List<string> { (string)x["ID"], (string)x["Store"], (string)x["Product"], (string)x["Quantity"], (string)x["StockLevel"] });
            }
            return content;

        }

        public object GetProducts()
        {
            throw new NotImplementedException();
        }

        internal bool BuyProduct(int id)
        {
            throw new NotImplementedException();
        }

        internal object GetInventory()
        {
            throw new NotImplementedException();
        }
    }
}
