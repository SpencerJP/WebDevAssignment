﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WebDevAssignment.View
{
    class FranchiseHolderDriver
    {
        private Controller.Controller c;
        public FranchiseHolderDriver(Controller.Controller controller)
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
                            //DisplayInventory();
                            break;
                        case "2":
                            //StockRequest();
                            break;
                        case "3":
                            //AddNewInventoryItem();
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
            var data = c.GetInventory();
        }

        private void StockRequest()
        {
            var data = c.GetStockRequests();
        }

        private void AddNewInventoryItem()
        {

        }

    }
}
