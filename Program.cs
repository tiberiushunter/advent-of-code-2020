using System;
using System.Drawing;
using Console = Colorful.Console;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;

namespace AdventOfCode
{
    class Program
    {
        static string aocSessionKey;
        static void Main(string[] args)
        {
            // Adds the User Secrets Config
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

            // Loads the Advent of Code session key.
            var secretProvider = config.Providers.First();
            if (!secretProvider.TryGet("AdventOfCode:Session", out aocSessionKey))
            {
                Console.WriteLine("Advent of Code session secret not found!", Color.Red);
                Console.WriteLine("\nHave you run this?");
                Console.WriteLine("\tdotnet user-secrets set \"AdventOfCode:Session\" \"y0ur_s3ss10n_k3y*\"", Color.Yellow);
                return;
            }

            string welcomeText = @"

 █████╗ ██████╗ ██╗   ██╗███████╗███╗   ██╗████████╗     ██████╗ ███████╗     ██████╗ ██████╗ ██████╗ ███████╗
██╔══██╗██╔══██╗██║   ██║██╔════╝████╗  ██║╚══██╔══╝    ██╔═══██╗██╔════╝    ██╔════╝██╔═══██╗██╔══██╗██╔════╝
███████║██║  ██║██║   ██║█████╗  ██╔██╗ ██║   ██║       ██║   ██║█████╗      ██║     ██║   ██║██║  ██║█████╗  
██╔══██║██║  ██║╚██╗ ██╔╝██╔══╝  ██║╚██╗██║   ██║       ██║   ██║██╔══╝      ██║     ██║   ██║██║  ██║██╔══╝  
██║  ██║██████╔╝ ╚████╔╝ ███████╗██║ ╚████║   ██║       ╚██████╔╝██║         ╚██████╗╚██████╔╝██████╔╝███████╗
╚═╝  ╚═╝╚═════╝   ╚═══╝  ╚══════╝╚═╝  ╚═══╝   ╚═╝        ╚═════╝ ╚═╝          ╚═════╝ ╚═════╝ ╚═════╝ ╚══════╝
                                                                                                              
";

            Console.WriteLine(welcomeText, Color.Green);
            Console.Write("Choose a year to solve (2015-2020) or press Enter to solve all years\t");

            string yearInput = Console.ReadLine();
            int yearSelected;

            if (Int32.TryParse(yearInput, out yearSelected))
            {
                Console.Write("\nChoose a day to solve (1-25) or press Enter to solve all days\t");

                string dayInput = Console.ReadLine();
                int daySelected;

                if (Int32.TryParse(dayInput, out daySelected))
                {
                    SolveDay(daySelected, yearSelected);
                }
                else
                {
                    SolveYear(yearSelected);
                }
            }
            else
            {
                SolveAll();
            }
        }

        /// <summary>
        /// Returns the input for a given year and day.
        /// </summary>
        /// <remarks>
        /// If the input file doesn't exist locally it'll fetch the input from AoC and store it
        /// in the <c>/input</c> directory to prevent frequent calls to AoC servers.
        /// </remarks>
        /// <param name="year">Year of AoC input to fetch</param>
        /// <param name="day">Day of AoC input to fetch</param>
        /// <returns>String challenge input</returns>
        public static string GetInput(int year, int day)
        {
            string path = @".\input\" + year + " - Day " + day + ".txt";
            DirectoryInfo di = Directory.CreateDirectory(@".\input\");

            if (!File.Exists(path))
            {
                using (FileStream fs = File.Create(path))
                {
                    using (var client = new WebClient())
                    {
                        client.Headers.Add(HttpRequestHeader.Cookie, "session=" + Program.aocSessionKey);

                        Byte[] input = new UTF8Encoding(true).GetBytes(client.DownloadString("https://adventofcode.com/" + year + "/day/" + day + "/input").Trim());
                        fs.Write(input, 0, input.Length);
                    }
                }
            }
            return File.ReadAllText(path);
        }

        public static void SolveDay(int d, int y)
        {
            Console.WriteLine("=============", Color.Green);
            Console.WriteLine(" {0} - Day {1}", y, d, Color.Green);
            Console.WriteLine("=============\n", Color.Green);

            Stopwatch timer = new Stopwatch();
            Type t = Type.GetType("AdventOfCode._" + y + ".Day" + d);

            timer.Start();

            try
            {
                var day = (DayBase)Activator.CreateInstance(t);
                day.Solve();

                timer.Stop();

                Console.WriteLine("Solved in {0}ms\n", timer.ElapsedMilliseconds);
            }
            catch
            {
                if (d < 1 || d > 25)
                {
                    Console.WriteLine("Pick a day between 1 and 25", Color.Red);
                }
                else if (y < 2015 || y > 2020)
                {
                    Console.WriteLine("Pick a year between 2015 and 2020", Color.Red);
                }
                else
                {
                    Console.WriteLine("A solution for that day hasn't been completed yet!", Color.Yellow);
                }
            }
        }
        public static void SolveYear(int y)
        {
            Console.WriteLine("\n============================", Color.Green);
            Console.WriteLine(" All Days for {0} Selected", y, Color.Green);
            Console.WriteLine("============================\n", Color.Green);

            List<Type> listOfDays = Assembly.GetExecutingAssembly().GetTypes()
                      .Where(t => t.Namespace == "AdventOfCode._" + y)
                      .Where(t => t.Name.StartsWith("Day"))
                      .ToList();

            Stopwatch timer = new Stopwatch();

            timer.Start();

            for (int i = 1; i <= listOfDays.Count; i++)
            {
                SolveDay(i, y);
            }

            timer.Stop();

            Console.WriteLine("==============================", Color.Yellow);
            Console.WriteLine(" Total Execution Time: {0}ms", timer.ElapsedMilliseconds, Color.Yellow);
            Console.WriteLine("==============================", Color.Yellow);
        }

        public static void SolveAll()
        {
            Console.WriteLine("\n=================================", Color.Green);
            Console.WriteLine(" All Days for All Years Selected", Color.Green);
            Console.WriteLine("=================================\n", Color.Green);

            List<Type> listOfDays = Assembly.GetExecutingAssembly().GetTypes()
                      .Where(t => t.Namespace.StartsWith("AdventOfCode._"))
                      .Where(t => t.Name.StartsWith("Day"))
                      .ToList();

            long totalTime = 0L;

            for (int i = 1; i <= listOfDays.Count; i++)
            {
                string year = listOfDays.ElementAt(i - 1).Namespace.Substring(listOfDays.ElementAt(i - 1).Namespace.Length - 4);
                Console.WriteLine("=============", Color.Green);
                Console.WriteLine(" {0} Day {1}", year, i, Color.Green);
                Console.WriteLine("=============\n", Color.Green);

                Stopwatch timer = new Stopwatch();

                timer.Start();

                var day = (DayBase)Activator.CreateInstance(listOfDays.ElementAt(i - 1));
                day.Solve();

                timer.Stop();

                Console.WriteLine("Solved in {0}ms\n", timer.ElapsedMilliseconds);
                totalTime += timer.ElapsedMilliseconds;
            }
            Console.WriteLine("==============================", Color.Yellow);
            Console.WriteLine(" Total Execution Time: {0}ms", totalTime, Color.Yellow);
            Console.WriteLine("==============================", Color.Yellow);
        }
    }
}
