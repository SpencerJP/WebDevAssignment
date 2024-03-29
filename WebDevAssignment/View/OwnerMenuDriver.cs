﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace WebDevAssignment.View
{
    /* 
     * OwnerMenuDriver
     * Opens a view for the owner menu
     */
    class OwnerMenuDriver
    {
        private Controller.Controller c;
        public OwnerMenuDriver(Controller.Controller controller)
        {
            c = controller;
        }

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
                            DisplayStockRequests();
                            break;
                        case "2":
                            DisplayOwnerInventory();
                            break;
                        case "3":
                            ResetInventoryStock();
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

            var data = c.GetStockRequests();
            Boolean success = false;
            Console.WriteLine(@"                 
                                            Stock Requests

ID       Store                       Product                  Quantity      Current Stock        Stock Availability");
            foreach(var x in data)
            {
                // get a printable boolean of whether quantity is less than current stock
                var stockavailability = Convert.ToString(Int32.Parse(x[3]) < Int32.Parse(x[4]));
                Console.WriteLine(String.Format("{0,-6} | {1,-25} | {2,-22} | {3,-11} | {4,-18} | {5,-18} ", x[0], x[1], x[2], x[3], x[4], stockavailability));
            }
            Console.Write(@"
                            Enter request to process:");
            while (!success)
            {
                var s = Console.ReadLine();
                if (Int32.TryParse(s, out int id))
                {
                    success = c.ProcessStockRequest(id);
                    Console.WriteLine(success ? "The operation was successful, the stock request has been processed and removed." : "The operation was a failure, you don't have enough stock, or You inserted an invalid ID.");
                    success = true;
                }
                else 
                {
                    if (!(s == "" || s == "\n"))
                    {
                        Console.WriteLine("You inserted an invalid ID.");
                    }
                    success = true;
                }
            }


        }

        private void DisplayOwnerInventory()
        {
            var data = c.GetOwnerInventory();
            Console.WriteLine(@"
                              Owner Inventory
ID    Product                   Current Stock");
            foreach (var x in data)
            {
                Console.WriteLine(String.Format("{0,-7} | {1,-26} | {2,-13}", x[0], x[1], x[2]));
            }
            Console.WriteLine();

        }

        private void ResetInventoryStock()
        {
            DisplayOwnerInventory();
            Console.Write("Insert an ID to reset: ");
            while(true)
            {
                var s = Console.ReadLine();
                if (Int32.TryParse(s, out int id))
                {   
                    var success = c.ResetInventoryStock(id);
                    if (success)
                    {
                        try
                        {
                            Console.WriteLine(c.GetProductNameByID(id) + " has been set to 20.");
                        }
                        catch (Exception)
                        {
                            // This shouldn't occur
                            Console.WriteLine("Unknown error, check that the DB is connected");
                        }
                                                   
                    }
                    else
                    {   
                        try
                        {
                            Console.WriteLine(c.GetProductNameByID(id) + " has a stock higher than 20, so it has not been reset.");
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    break;
                } else
                {
                    if (!(s == "" || s == "\n"))
                    {
                        Console.WriteLine("You inserted an invalid ID.");
                    }
                    break;
                }
            }
        }
    }
}
