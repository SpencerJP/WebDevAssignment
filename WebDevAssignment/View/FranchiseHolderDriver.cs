using System;
using System.Collections.Generic;
using System.Text;

namespace WebDevAssignment.View
{
    class FranchiseHolderDriver
    {
        private Controller.Controller c;
        private int currentStoreID = -1;
        public FranchiseHolderDriver(Controller.Controller controller)
        {
            c = controller;
        }

        public void SelectStore()
        {
            Console.Write("\nStores\n");
            var data = c.GetStores();
            foreach (var x in data)
            {
                Console.WriteLine(String.Format("{0,-7} | {1,-26}", x[0], x[1]));
            }
            while (true)
            {
                Console.Write("Enter the store to use: ");
                var s = Console.ReadLine();
                if (Int32.TryParse(s, out int id))
                {
                    try
                    {

                        var StoreName = c.SelectStore(id);
                        currentStoreID = id;
                        OpenMenu(StoreName);
                        break;
                    }
                    catch (Exception e)
                    {
                            Console.WriteLine(e.Message);
                    }
                   
                }
                else
                {
                    if (!(s == "" || s == "\n"))
                    {
                        Console.WriteLine("You inserted an invalid ID.");
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void OpenMenu(String StoreName)
        {



        Boolean quit = false;
            while (!quit)
            {
                while (true)
                {

                    Console.Write($"Welcome to Marvelous Magic (Franchise Holder - { StoreName })\n" +
                    "+++++++++++++++++++++++++++++++++++\n" +
                    "1. Display Inventory\n" +
                    "2. Stock Request (Threshold)\n" +
                    "3. Add New Inventory Item\n" +
                    "4. Return to Main Menu\n" +
                    "Please enter an option: ");

                    string input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":
                            DisplayInventory();
                            break;
                        case "2":
                            StockRequest();
                            break;
                        case "3":
                            AddNewInventoryItem();
                            break;
                        case "4":
                            quit = true;
                            break;
                        default:
                            Console.Write("That is not a valid input." +
                                "\nPlease enter an option: ");
                            break;


                    }
                    if (quit)
                    {
                        break;
                    }
                }
            }
        }

        private void DisplayInventory()
        {
            var data = c.GetStoreInventory(this.currentStoreID);
            Console.WriteLine(@"
                              Store Inventory
ID    Product                   Current Stock");
            foreach (var x in data)
            {
                Console.WriteLine(String.Format("{0,-7} | {1,-26} | {2,-13}", x[0], x[1], x[2]));
            }
            Console.WriteLine();

        }

        private void StockRequest()
        {
            Console.Write("Enter threshold for restocking: ");
            while (true)
            {
                var s = Console.ReadLine();
                if (Int32.TryParse(s, out int threshold))
                {
                    try
                    {

                        var data = c.GetStockBelowThreshold(currentStoreID, threshold);
                        Console.WriteLine(@"
                              Inventory
ID    Product                   Current Stock");
                        foreach (var x in data)
                        {
                            Console.WriteLine(String.Format("{0,-7} | {1,-26} | {2,-13}", x[0], x[1], x[2]));
                        }
                        ProcessRequest(threshold);
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
                else
                {
                    if (!(s == "" || s == "\n"))
                    {
                        Console.WriteLine("you inserted an invalid ID.");
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void ProcessRequest(int threshold)
        {
            Console.Write("Enter the ID of the stock you would like to make a request for: ");
            while (true)
            {
                var s = Console.ReadLine();
                if (Int32.TryParse(s, out int productID))
                {
                    try
                    {

                        c.CreateRequest(currentStoreID, productID, threshold);
                        Console.WriteLine($"Successfully made a request for " + c.GetProductNameByID(productID));
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
                else
                {
                    if (!(s == "" || s == "\n"))
                    {
                        Console.WriteLine("you inserted an invalid ID.");
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void AddNewInventoryItem()
        {
            var data = c.GetItemsNotInStock(currentStoreID);
            if (data != null)
            {

                Console.WriteLine(@"
           Items Not Currently Stocked in this Store
ID    Product                   Current Stock");
                foreach (var x in data)
                {
                    Console.WriteLine(String.Format("{0,-7} | {1,-26} | {2,-13}", x[0], x[1], x[2]));
                }
                while (true)
                {
                    Console.Write("Select a product ID to add to your inventory: ");
                    var s = Console.ReadLine();
                    if (Int32.TryParse(s, out int productID))
                    {
                        try
                        {

                            c.AddNewInventoryItem(currentStoreID, productID);
                            Console.WriteLine($"A request has been made for {c.GetProductNameByID(productID) }.");
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                    }
                    else
                    {
                        if (!(s == "" || s == "\n"))
                        {
                            Console.WriteLine("you inserted an invalid ID.");
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                Console.Write(@"This store already has all available items in stock.\n");
            }
        }

    }
}
