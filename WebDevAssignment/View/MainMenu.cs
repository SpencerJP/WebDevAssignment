using System;
using System.Collections.Generic;
using System.Text;

namespace WebDevAssignment.View
{
    /*
     *  MainMenu
     *  Launches a view of the main menu of this app.
     */
    class MainMenu
    {
        private Controller.Controller c;
        public MainMenu(Controller.Controller controller)
        {
            c = controller;
            this.OpenMenu();
        }

        public void OpenMenu()
        {
            Boolean quit = false;
            while (!quit)
            {
                while (true)
                {
                    Console.Write("Welcome to Marvelous Magic\n" +
                    "+++++++++++++++++++++++++++++++++++\n" +
                    "1. Owner\n" +
                    "2. Franchise Holder\n" +
                    "3. Customer\n" +
                    "4. Quit\n" +
                    "Please enter an option: ");
                    string input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":

                            OwnerMenuDriver OwnerMenu = new OwnerMenuDriver(this.c);
                            OwnerMenu.OpenMenu();
                            break;

                        case "2":

                            FranchiseHolderDriver FranchiseOwnerMenu = new FranchiseHolderDriver(this.c);
                            FranchiseOwnerMenu.SelectStore();
                            break;

                        case "3":

                            CustomerMenuDriver CustomerMenu = new CustomerMenuDriver(this.c);
                            CustomerMenu.SelectStore();
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