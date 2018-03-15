using System;
using System.Collections.Generic;
using System.Text;

namespace WebDevAssignment.View
{
    class CustomerMenuDriver
    {

        private Controller.Controller c;
        public CustomerMenuDriver(Controller.Controller controller)
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

                    Console.Write("Welcome to Marvelous Magic (Retail    - {{STORE NAME}})\n" +
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
            var data = c.GetProducts();
            while (!success)
            {
                var s = Console.ReadLine();
                if (Int32.TryParse(s, out int id))
                {
                    success = c.BuyProduct(id);
                }
                else if (s == "" || s == "\n")
                {
                    success = true;
                }
            }

        }
    }
}

