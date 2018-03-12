using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace WebDevAssignment.View
{
    class OwnerMenuDriver
    {
        public void OpenMenu()
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

        private void DisplayStockRequests()
        {
            Console.Write("Stock Requests\n\n");
            try
            {

            }
            catch(SqlException e)
            {
                Console.WriteLine("Something went wrong while fetching data from the SQL Server.");
                Console.WriteLine(e.ErrorCode);
            }
        }
    }
}
