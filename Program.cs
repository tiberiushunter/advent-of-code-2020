using System;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace advent_of_code_2020
{
    class Program
    {
        static string aocSessionKey;
        static void Main(string[] args)
        {
            // Adds the User Secrets
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

            // Loads the Advent of Code session key.
            // This is me playing around with UserSecrets and is going to be used to fetch input straight from AoC
            var secretProvider = config.Providers.First();
            if (!secretProvider.TryGet("AdventOfCode:Session", out aocSessionKey))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Advent of Code session secret not found!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nHave you run this?");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\tdotnet user-secrets set \"AdventOfCode:Session\" \"y0ur_s3ss10n_k3y*\"");
                return;
            }

            string welcomeText = @"
 █████╗ ██████╗ ██╗   ██╗███████╗███╗   ██╗████████╗     ██████╗ ███████╗     ██████╗ ██████╗ ██████╗ ███████╗    ██████╗  ██████╗ ██████╗  ██████╗     
██╔══██╗██╔══██╗██║   ██║██╔════╝████╗  ██║╚══██╔══╝    ██╔═══██╗██╔════╝    ██╔════╝██╔═══██╗██╔══██╗██╔════╝    ╚════██╗██╔═████╗╚════██╗██╔═████╗    
███████║██║  ██║██║   ██║█████╗  ██╔██╗ ██║   ██║       ██║   ██║█████╗      ██║     ██║   ██║██║  ██║█████╗       █████╔╝██║██╔██║ █████╔╝██║██╔██║    
██╔══██║██║  ██║╚██╗ ██╔╝██╔══╝  ██║╚██╗██║   ██║       ██║   ██║██╔══╝      ██║     ██║   ██║██║  ██║██╔══╝      ██╔═══╝ ████╔╝██║██╔═══╝ ████╔╝██║    
██║  ██║██████╔╝ ╚████╔╝ ███████╗██║ ╚████║   ██║       ╚██████╔╝██║         ╚██████╗╚██████╔╝██████╔╝███████╗    ███████╗╚██████╔╝███████╗╚██████╔╝    
╚═╝  ╚═╝╚═════╝   ╚═══╝  ╚══════╝╚═╝  ╚═══╝   ╚═╝        ╚═════╝ ╚═╝          ╚═════╝ ╚═════╝ ╚═════╝ ╚══════╝    ╚══════╝ ╚═════╝ ╚══════╝ ╚═════╝";

            Console.WriteLine(welcomeText);
            Console.Write("\nChoose Day to Solve [1-25]\t");
            Console.ForegroundColor = ConsoleColor.Green;

            string input = Console.ReadLine();
            int daySelected;

            if (Int32.TryParse(input, out daySelected)) //TODO: Sanitise this properly...
            {
                Console.WriteLine("Day {0} selected.\n", daySelected);
            }

            Console.ForegroundColor = ConsoleColor.White;

            switch (input.ToLower())
            {
                case "1":
                    new Day1();
                    break;
                case "2":
                    new Day2();
                    break;
                case "3":
                    new Day3();
                    break;
                case "4":
                    new Day4();
                    break;
                case "all":
                    Console.WriteLine("Solving all Days \n");
                    new Day1();
                    new Day2();
                    new Day3();
                    new Day4();
                    break;
                default:
                    Console.WriteLine("Defaulting to Latest Day \n");
                    new Day4();
                    break;
            }
        }

        public static string GetInput(int year, int day)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.Cookie, "session=" + Program.aocSessionKey);
                return client.DownloadString("https://adventofcode.com/" + year + "/day/" + day + "/input").Trim();
            }
        }
    }
}
