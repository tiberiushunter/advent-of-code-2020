using System;
using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    class Day2 : DayBase
    {
        private readonly string _input;
        private string[] _inputArr;
        private readonly Regex _regEx = new Regex(@"(\d+)-(\d+) (.): (\w+)");

        /// <summary>
        /// --- Day 2: Password Philosophy ---
        /// </summary>
        public Day2()
        {
            _input = Program.GetInput(2020, 2);
            _inputArr = _input.Split('\n');
        }

        /// <summary>
        /// --- Day 2: Password Philosophy; Part A ---
        /// </summary>
        private protected override string PartA()
        {
            int numOfValidPasswords = 0;

            foreach (string str in _inputArr)
            {
                Match m = _regEx.Match(str);

                if (m.Success)
                {
                    int count = 0;
                    for (int i = 0; i < m.Groups[4].Length; i++)
                    {
                        if (m.Groups[4].Value[i] == m.Groups[3].Value[0])
                        {
                            count++;
                        }
                    }
                    if (count >= Int32.Parse(m.Groups[1].Value) && count <= Int32.Parse(m.Groups[2].Value))
                    {
                        numOfValidPasswords++;
                    }
                }
            }
            return numOfValidPasswords.ToString();
        }

        /// <summary>
        /// --- Day 2: Password Philosophy; Part B ---
        /// </summary>
        private protected override string PartB()
        {
            int numOfValidPasswords = 0;

            foreach (string str in _inputArr)
            {
                Match m = _regEx.Match(str);

                if (m.Success)
                {
                    if (m.Groups[4].Value[Int32.Parse(m.Groups[1].Value) - 1] == m.Groups[3].Value[0] ^ m.Groups[4].Value[Int32.Parse(m.Groups[2].Value) - 1] == m.Groups[3].Value[0])
                    {
                        numOfValidPasswords++;
                    }
                }
            }
            return numOfValidPasswords.ToString();
        }
    }
}