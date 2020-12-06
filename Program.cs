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
╚═╝  ╚═╝╚═════╝   ╚═══╝  ╚══════╝╚═╝  ╚═══╝   ╚═╝        ╚═════╝ ╚═╝          ╚═════╝ ╚═════╝ ╚═════╝ ╚══════╝    ╚══════╝ ╚═════╝ ╚══════╝ ╚═════╝
";

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(welcomeText);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\nChoose a day to solve (1-25) or type 'all' to solve all days\t");

            string input = Console.ReadLine();
            int daySelected;

            if (Int32.TryParse(input, out daySelected)) //TODO: Sanitise this properly...
            {
                SolveDay(daySelected);
            }
            else
            {
                switch (input.ToLower())
                {
                    case "all":
                        SolveAll();
                        break;
                    default:
                        SolveDay(5);
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

        public static void SolveDay(int d)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n================");
            Console.WriteLine(" Day {0} Selected", d);
            Console.WriteLine("================\n");
            Console.ForegroundColor = ConsoleColor.White;

            Stopwatch timer = new Stopwatch();
            Type t = Type.GetType("advent_of_code_2020.Day" + d);

            timer.Start();

            try
            {
                BaseDay day = (BaseDay)Activator.CreateInstance(t);
                day.Solve();

                timer.Stop();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Day {0} Solved in {1}ms", d, timer.ElapsedMilliseconds);
            }
            catch
            {
                if (d < 1 || d > 26)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Pick a number between 1-25");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("We've not had that day yet!");
                }
            }
        }

        public static void SolveAll()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n===================");
            Console.WriteLine(" All Days Selected");
            Console.WriteLine("===================\n");
            Console.ForegroundColor = ConsoleColor.White;

            List<Type> listOfDays = Assembly.GetExecutingAssembly().GetTypes()
                      .Where(t => t.Namespace == "advent_of_code_2020")
                      .Where(t => t.Name.StartsWith("Day"))
                      .ToList();

            long totalTime = 0L;

            for (int i = 1; i <= listOfDays.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("=========");
                Console.WriteLine(" Day {0}", i);
                Console.WriteLine("=========\n");
                Console.ForegroundColor = ConsoleColor.White;

                Stopwatch timer = new Stopwatch();

                timer.Start();

                BaseDay day = (BaseDay)Activator.CreateInstance(listOfDays.ElementAt(i - 1));
                day.Solve();

                timer.Stop();

                Console.WriteLine("Day {0} Solved in {1}ms\n", i, timer.ElapsedMilliseconds);
                totalTime += timer.ElapsedMilliseconds;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("==============================");
            Console.WriteLine(" Total Execution Time: {0}ms", totalTime);
            Console.WriteLine("==============================");
        }
    }
}
