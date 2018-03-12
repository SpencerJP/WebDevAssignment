using System;

namespace WebDevAssignment.View
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
        
        private static void OpenNextMenu(string input)
        {
            if (input == "1") // Owner
            {
                OwnerMenuDriver OwnerMenu = new OwnerMenuDriver();
                OwnerMenu.OpenMenu();
            }
            else if (input == "2") // Franchise Holder
            {
                FranchiseHolderDriver FranchiseOwnerMenu = new FranchiseHolderDriver();
                FranchiseOwnerMenu.OpenMenu();
            }
            else //( input must be "3" ) // Customer
            {
                CustomerMenuDriver CustomerMenu = new CustomerMenuDriver();
                CustomerMenu.OpenMenu();
            }
        }

    }
}
