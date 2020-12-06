using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
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
                Console.ForegroundColor = ConsoleColor.White;
                Solve(daySelected);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                switch (input.ToLower())
                {
                    case "all":
                        Console.WriteLine("Solving all Days \n");
                        SolveAll();
                        break;
                    default:
                        Console.WriteLine("Defaulting to Latest Day \n");
                        new Day5();
                        break;
                }
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

        public static void Solve(int day)
        {
            Stopwatch stopwatch = new Stopwatch();
            Type t = Type.GetType("advent_of_code_2020.Day" + day);

            stopwatch.Start();
            Activator.CreateInstance(t);
            stopwatch.Stop();

            Console.WriteLine("Solved in {0}ms", stopwatch.ElapsedMilliseconds);
        }

        public static void SolveAll()
        {
            List<Type> listOfDays = Assembly.GetExecutingAssembly().GetTypes()
                      .Where(t => t.Namespace == "advent_of_code_2020")
                      .Where(t => t.Name.StartsWith("Day"))
                      .ToList();

            long totalTime = 0L;

            foreach (Type day in listOfDays)
            {
                Stopwatch timer = new Stopwatch();

                timer.Start();
                Activator.CreateInstance(day);
                timer.Stop();
                Console.WriteLine("{0} Solved in {1}ms\n", day.Name, timer.ElapsedMilliseconds);
                totalTime += timer.ElapsedMilliseconds;
            }
            Console.WriteLine("Total Execution Time = {0}ms", totalTime);
        }
    }
}
