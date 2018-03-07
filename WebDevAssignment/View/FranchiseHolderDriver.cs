using System;
using System.Collections.Generic;
using System.Text;

namespace WebDevAssignment.View
{
    class FranchiseHolderDriver
    {

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



    }
}
