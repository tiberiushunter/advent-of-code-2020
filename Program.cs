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
            Console.Write("\nChoose Day number to Solve [1-25]\t");
            string input = Console.ReadLine();

            if (input != string.Empty) //TODO: Sanitise this properly...
            {
                Console.WriteLine("Day {0} selected.\n", input);
            }
            else
            {
                Console.WriteLine("Latest Day Selected\n", input);
            }

            switch (input)
            {
                case "1":
                    var a = new Day1();
                    break;
                case "2":
                    var b = new Day2();
                    break;
                case "3":
                    var c = new Day3();
                    break;
                case "4":
                    var d = new Day4();
                    break;
                default:
                    var z = new Day4();
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
