using System;

namespace WebDevAssignment
{
    class Driver
    {
        static void Main(string[] args)
        {
            SQLDriver s = new SQLDriver();
            s.testRead();

            Boolean quit = false;
            Console.Write("Welcome to Marvelous Magic\n" +
                "+++++++++++++++++++++++++++++++++++\n" +
                "1. Owner\n" +
                "2. Franchise Holder\n" +
                "3. Customer\n" +
                "4. Quit\n" +
                "Please enter an option: ");
            while(!quit)
            {
                while (true)
                {
                    string input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":

                        case "2":

                        case "3":
                            // go to helper function that opens next menu
                            OpenNextMenu(input);
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

        // TODO make non-static
        private static void OpenNextMenu(string input)
        {
            if (input == "1") // Owner
            {
                Console.WriteLine("Owner");
            }
            else if (input == "2") // Franchise Holder
            {
                Console.WriteLine("Franchise Holder");
            }
            else //( input must be "3" ) // Customer
            {
                Console.WriteLine("Customer");
            }
        }

        private static void OwnerMenu()
        {

            Boolean quit = false;
            while (!quit)
            {
                while (true)
                {

                    Console.Write("Welcome to Marvelous Magic (Owner)\n" +
                    "+++++++++++++++++++++++++++++++++++\n" +
                    "1. Display All Stock Requests\n" +
                    "2. Display Owner Inventory\n" +
                    "3. Reset Inventory Item Stock\n" +
                    "4. Return to Main Menu\n" +
                    "Please enter an option: ");

                    string input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":
                            //DisplayStockRequests();
                            break;
                        case "2":
                            //DisplayOwnerInventory();
                            break;
                        case "3":
                            //ResetInventoryItemStock();
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

        private static void FranchiseHolderMenu()
        {

            Boolean quit = false;
            while (!quit)
            {
                while (true)
                {

                    Console.Write("Welcome to Marvelous Magic (Franchise Holder - {{STORE NAME}})\n" +
                    "+++++++++++++++++++++++++++++++++++\n" +
                    "1. Display Inventory\n" +
                    "2. Stock Request\n" +
                    "3. Add New Inventory Item\n" +
                    "4. Return to Main Menu\n" +
                    "Please enter an option: ");

                    string input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":
                            //DisplayStockRequests();
                            break;
                        case "2":
                            //DisplayOwnerInventory();
                            break;
                        case "3":
                            //ResetInventoryItemStock();
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

        private static void CustomerMenu()
        {

            Boolean quit = false;
            while (!quit)
            {
                while (true)
                {

                    Console.Write("Welcome to Marvelous Magic (Retail    - {{STORE NAME}})\n" +
                    "+++++++++++++++++++++++++++++++++++\n" +
                    "1. Display Products\n" +
                    "2. Return to Main Menu\n" +
                    "Please enter an option: ");

                    string input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":
                            //DisplayStockRequests();
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
    }
}
