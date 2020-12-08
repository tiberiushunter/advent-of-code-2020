using System;
using System.Linq;

namespace AdventOfCode._2020
{
    class Day1 : DayBase
    {
        private readonly string _input;
        private int[] _arr;

        /// <summary>
        /// --- Day 1: Report Repair ---
        /// </summary>
        public Day1()
        {
            _input = Program.GetInput(2020, 1);
            _arr = _input.Split('\n').Select(n => Convert.ToInt32(n)).ToArray();
        }

        /// <summary>
        /// --- Day 1: Report Repair; Part A ---
        /// </summary>
        private protected override string PartA()
        {
            for (int i = 0; i < _arr.Length; i++)
            {
                for (int j = 0; j < _arr.Length; j++)
                {
                    if (_arr[i] + _arr[j] == 2020)
                    {
                        return (_arr[i] * _arr[j]).ToString();
                    }
                }
            }
            // Nothing is found
            return string.Empty;
        }

        /// <summary>
        /// --- Day 1: Report Repair; Part B ---
        /// </summary>
        private protected override string PartB()
        {
            for (int i = 0; i < _arr.Length; i++)
            {
                for (int j = 0; j < _arr.Length; j++)
                {
                    for (int k = 0; k < _arr.Length; k++)
                    {
                        if (_arr[i] + _arr[j] + _arr[k] == 2020)
                        {
                            return (_arr[i] * _arr[j] * _arr[k]).ToString();
                        }
                    }
                }
            }
            // Nothing is found
            return string.Empty;
        }
    }
}