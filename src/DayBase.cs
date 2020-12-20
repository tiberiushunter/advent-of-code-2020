using System.Diagnostics;
using System.Drawing;
using Console = Colorful.Console;

namespace AdventOfCode
{
    abstract class DayBase
    {
        public void Solve()
        {
            Stopwatch timer = new Stopwatch();

            string result = string.Empty;

            timer.Start();
            result = PartA();
            timer.Stop();

            Console.WriteFormatted("Part 1:\t{0}\t", Color.LightBlue, Color.Gray, result);
            Console.WriteLineFormatted("({0})", Color.LightGreen, Color.Gray, ((float)timer.ElapsedTicks / Stopwatch.Frequency) * 1000 + "ms");

            timer.Restart();
            result = PartB();
            timer.Stop();

            Console.WriteFormatted("Part 2:\t{0}\t", Color.LightBlue, Color.Gray, result);
            Console.WriteLineFormatted("({0})\n", Color.LightGreen, Color.Gray, ((float)timer.ElapsedTicks / Stopwatch.Frequency) * 1000 + "ms");
        }
        private protected abstract string PartA();
        private protected abstract string PartB();
    }
}