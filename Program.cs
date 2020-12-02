using System;

namespace advent_of_code_2020
{
    class Program
    {
        static void Main(string[] args)
        {
            string welcomeText = @"
 █████╗ ██████╗ ██╗   ██╗███████╗███╗   ██╗████████╗     ██████╗ ███████╗     ██████╗ ██████╗ ██████╗ ███████╗    ██████╗  ██████╗ ██████╗  ██████╗     
██╔══██╗██╔══██╗██║   ██║██╔════╝████╗  ██║╚══██╔══╝    ██╔═══██╗██╔════╝    ██╔════╝██╔═══██╗██╔══██╗██╔════╝    ╚════██╗██╔═████╗╚════██╗██╔═████╗    
███████║██║  ██║██║   ██║█████╗  ██╔██╗ ██║   ██║       ██║   ██║█████╗      ██║     ██║   ██║██║  ██║█████╗       █████╔╝██║██╔██║ █████╔╝██║██╔██║    
██╔══██║██║  ██║╚██╗ ██╔╝██╔══╝  ██║╚██╗██║   ██║       ██║   ██║██╔══╝      ██║     ██║   ██║██║  ██║██╔══╝      ██╔═══╝ ████╔╝██║██╔═══╝ ████╔╝██║    
██║  ██║██████╔╝ ╚████╔╝ ███████╗██║ ╚████║   ██║       ╚██████╔╝██║         ╚██████╗╚██████╔╝██████╔╝███████╗    ███████╗╚██████╔╝███████╗╚██████╔╝    
╚═╝  ╚═╝╚═════╝   ╚═══╝  ╚══════╝╚═╝  ╚═══╝   ╚═╝        ╚═════╝ ╚═╝          ╚═════╝ ╚═════╝ ╚═════╝ ╚══════╝    ╚══════╝ ╚═════╝ ╚══════╝ ╚═════╝";

            Console.WriteLine(welcomeText);
            Console.Write("\nChoose an Advent of Code Day number to run [1-25]");
            string input = Console.ReadLine();

            if (input != string.Empty) //TODO: Sanitise this...
            {
                Console.WriteLine("Day selected: {0}\n", input);
            }
            else
            {
                Console.WriteLine("Latest Day Selected\n", input);
            }

            switch (input)
            {
                case "1":
                    Day1 a = new Day1();
                    break;
                case "2":
                    Day2 b = new Day2();
                    break;
                default:
                    Day2 z = new Day2();
                    break;
            }
        }
    }
}
