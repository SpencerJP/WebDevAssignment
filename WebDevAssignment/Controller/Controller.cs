using System;
using System.Collections.Generic;
using System.Text;

namespace WebDevAssignment.Controller
{
    class Controller
    {
        private Model.SQLDriver sqldriver;
        private View.MainMenu mainmenu;

        public Controller()
        {

            Model.SQLDriver sqldriver = new Model.SQLDriver(this);
            View.MainMenu mainmenu = new View.MainMenu(this);

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

        public object GetStockRequests()
        {

            /* */
            var data = sqldriver.GetStockRequests();

            throw new NotImplementedException();

        }

        public object GetProducts()
        {
            throw new NotImplementedException();
        }

        internal bool BuyProduct(int id)
        {
            throw new NotImplementedException();
        }
    }
}
