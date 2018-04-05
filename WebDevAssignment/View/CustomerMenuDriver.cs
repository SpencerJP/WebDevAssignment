using System;
using System.Collections.Generic;
using System.Text;

namespace WebDevAssignment.View
{
    class CustomerMenuDriver
    {

        private Controller.Controller c;
        private int currentStoreID;

        public CustomerMenuDriver(Controller.Controller controller)
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

                    Console.Write($"Welcome to Marvelous Magic (Retail    - { StoreName })\n" +
                    "+++++++++++++++++++++++++++++++++++\n" +
                    "1. Display Products\n" +
                    "2. Return to Main Menu\n" +
                    "Please enter an option: ");

                    string input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":
                            DisplayProducts();
                            break;
                        case "2":
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

        private void DisplayProducts()
        {
            Boolean success = false;
            int currentpage = 0;
            
            var data = c.GetStoreInventory(currentStoreID);
            int length = data.Count;
            int endOfPage;

            while (!success)
            {
                endOfPage = (currentpage*3) + 3;
                Console.WriteLine(@"Inventory
ID    Product                   Current Stock");
                for (int i = 0 + (currentpage * 3); (i < endOfPage) && (i != length); i++)
                {
                    if (i > length)
                    {
                        break;

                    }
                    else
                    {
                        Console.WriteLine(String.Format("{0,-7} | {1,-26} | {2,-13}", data[i][0], data[i][1], data[i][2]));
                    }
                }
                Console.Write(@"[Legend: 'N' Next Page | 'R' Return To Menu]

Enter product ID to purchase or function: ");
                var s = Console.ReadLine();
                switch(s)
                {
                    case "N":
                        if (currentpage != (length / 3))
                        {
                            currentpage++;
                        }
                        else
                        {
                            currentpage = 0;
                        }
                        break;
                    case "R":
                        success = true;
                        break;
                    default:
                        Purchase(s);
                        break;
                }


                    
            }

        }


        private void Purchase(String s)
        {
            if (Int32.TryParse(s, out int id))
            {
                Console.Write(@"
Enter quantity to purchase: ");
                s = Console.ReadLine();
                if (Int32.TryParse(s, out int quantity))
                {
                    try
                    { 
                        c.BuyProduct(currentStoreID, id, quantity);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else if (s == "" || s == "\n")
                {
                    return;
                }
            }
            else if (s == "" || s == "\n")
            {
                return;
            }
        }
    }
}

