using System;
using System.Linq;

namespace AdventOfCode._2020
{
    class Day6 : DayBase
    {
        private readonly string _input;
        private string[] _inputArr;

        /// <summary>
        /// --- Day 6: Custom Customs ---
        /// </summary>
        public Day6()
        {
            _input = Program.GetInput(2020, 6);
            _inputArr = _input.Split(new string[] { "\n\n" },
                               StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// --- Day 6: Custom Customs; Part A ---
        /// </summary>
        private protected override string PartA()
        {
            int answerCount = 0;
            for (int i = 0; i < _inputArr.Length; i++)
            {
                answerCount += new String(_inputArr[i].Replace("\n", "").Distinct().ToArray()).Length;
            }
            return answerCount.ToString();
        }

        /// <summary>
        /// --- Day 6: Custom Customs; Part B ---
        /// </summary>
        private protected override string PartB()
        {
            int answerCount = 0;
            for (int i = 0; i < _inputArr.Length; i++)
            {
                string repeatedChars = new string(_inputArr[i].GroupBy(x => x).Where(y => y.Count() > _inputArr[i].Split("\n").Length - 1).Select(z => z.Key).ToArray());
                answerCount += repeatedChars.Length;
            }
            return answerCount.ToString();
        }
    }
}