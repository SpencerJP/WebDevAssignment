using System;
using System.Collections.Generic;
using System.Text;

namespace WebDevAssignment.View
{
    class CustomerMenuDriver
    {
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
                            //DisplayProducts();
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
