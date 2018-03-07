using System;

namespace WebDevAssignment
{
    class Driver
    {
        static void Main(string[] args)
        {
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
    }
}
