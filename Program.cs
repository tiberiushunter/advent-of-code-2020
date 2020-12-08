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
                    SolveDay(yearSelected, daySelected);
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
        /// <param name="y">Year of AoC input to fetch</param>
        /// <param name="d">Day of AoC input to fetch</param>
        /// <returns>String challenge input</returns>
        public static string GetInput(int y, int d)
        {
            string path = @".\input\" + y + " - Day " + d + ".txt";
            DirectoryInfo di = Directory.CreateDirectory(@".\input\");

            if (!File.Exists(path))
            {
                using (FileStream fs = File.Create(path))
                {
                    using (var client = new WebClient())
                    {
                        client.Headers.Add(HttpRequestHeader.Cookie, "session=" + Program.aocSessionKey);

                        Byte[] input = new UTF8Encoding(true).GetBytes(client.DownloadString("https://adventofcode.com/" + y + "/day/" + d + "/input").Trim());
                        fs.Write(input, 0, input.Length);
                    }
                }
            }
            return File.ReadAllText(path);
        }

        /// <summary>
        /// Solves a challenge for a given year and day.
        /// </summary>
        /// <remarks>
        /// Additionally also prints execution time to complete the given day
        /// which can (will) increase on first load due to delay in fetching 
        /// input from AoC servers.
        /// </remarks>
        /// <param name="y">Year of AoC to solve.</param>
        /// <param name="d">Day of AoC to solve.</param>
        /// <returns>Console output of the solution for both parts along with execution time.</returns>
        public static void SolveDay(int y, int d)
        {
            Console.WriteLine("===============", Color.MediumPurple);
            Console.WriteLine(" {0} - Day {1}", y, d, Color.HotPink);
            Console.WriteLine("===============\n", Color.MediumPurple);

            Type t = Type.GetType("AdventOfCode._" + y + ".Day" + d);
            try
            {
                var day = (DayBase)Activator.CreateInstance(t);
                day.Solve();
            }
            catch (Exception e)
            {
                if (d < 1 || d > 25)
                {
                    Console.WriteLine("Pick a day between 1 and 25", Color.Red);
                }
                else if (y < 2015 || y > 2020)
                {
                    Console.WriteLine("Pick a year between 2015 and 2020", Color.Red);
                }
                else {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        /// <summary>
        /// Solves a challenge for a given year.
        /// </summary>
        /// <remarks>
        /// This internally calls SolveDay() recursively for each day in the given year.
        /// </remarks>
        /// <seealso cref="SolveDay(int, int)"/>
        /// <param name="y">Year of AoC to solve.</param>
        /// <returns>Console output of the solutions for both parts of each day along with a total execution time.</returns>
        public static void SolveYear(int y)
        {
            Console.WriteLine("\n============================", Color.Green);
            Console.WriteLine(" All Days for {0} Selected", y, Color.Green);
            Console.WriteLine("============================\n", Color.Green);

            List<Type> listOfDays = Assembly.GetExecutingAssembly().GetTypes()
                      .Where(t => t.Namespace == "AdventOfCode._" + y)
                      .Where(t => t.Name.StartsWith("Day"))
                      .OrderBy(t => t.Name)
                      .ToList();

            long totalTime = 0L;

            for (int d = 1; d <= listOfDays.Count; d++)
            {
                Stopwatch timer = new Stopwatch();

                timer.Start();
                SolveDay(y, d);
                timer.Stop();

                totalTime += timer.ElapsedMilliseconds;
            }

            Console.WriteLine("=======================================", Color.Yellow);
            Console.WriteLine(" Total Execution Time for {0}: {1}ms", y, totalTime, Color.Yellow);
            Console.WriteLine("=======================================", Color.Yellow);
        }

        /// <summary>
        /// Solves all challenges stored
        /// </summary>
        /// <remarks>
        /// This internally calls SolveYear() recursively for each year detected from the namespaces on file.
        /// </remarks>
        /// <seealso cref="SolveYear(int)"/>
        /// <returns>Console output of the solutions for both parts of each day of each year along with a total execution time.</returns>
        public static void SolveAll()
        {
            Console.WriteLine("\n=================================", Color.Green);
            Console.WriteLine(" All Days for All Years Selected", Color.Green);
            Console.WriteLine("=================================", Color.Green);

            List<string> years = Assembly.GetExecutingAssembly().GetTypes()
                      .Where(t => t.Namespace.StartsWith("AdventOfCode._"))
                      .OrderBy(t => t.Namespace)
                      .Select(t => t.Namespace)
                      .Distinct()
                      .ToList();

            long totalTime = 0L;

            for (int i = 1; i <= years.Count; i++)
            {
                string year = years.ElementAt(i - 1).Substring(years.ElementAt(i - 1).Length - 4);

                Stopwatch timer = new Stopwatch();

                timer.Start();
                SolveYear(Int32.Parse(year));
                timer.Stop();

                totalTime += timer.ElapsedMilliseconds;
            }
            Console.WriteLine("==============================", Color.Yellow);
            Console.WriteLine(" Total Execution Time: {0}ms", totalTime, Color.Yellow);
            Console.WriteLine("==============================", Color.Yellow);
        }
    }
}
