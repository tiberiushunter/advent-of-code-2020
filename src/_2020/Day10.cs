using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode._2020
{
    class Day10 : DayBase
    {
        private readonly string _input;
        private int[] _adapterJolts;
        private Dictionary<int, long> _adapterBranches;

        /// <summary>
        /// --- Day 10: Adapter Array ---
        /// </summary>
        public Day10()
        {
            _input = Program.GetInput(2020, 10);
            _adapterJolts = _input.Split('\n').Select(n => Convert.ToInt32(n)).OrderBy(x => x).ToArray();
        }

        /// <summary>
        /// --- Day 10: Adapter Array; Part A ---
        /// </summary>
        private protected override string PartA()
        {
            int oneJoltCount = _adapterJolts.First();
            int threeJoltCount = 1; // Includes Jolt at the end

            int currentJolt = _adapterJolts.First();

            while (currentJolt != _adapterJolts.Last())
            {
                for (int i = 0; i < _adapterJolts.Length; i++)
                {
                    if (currentJolt == _adapterJolts[i] - 1)
                    {
                        oneJoltCount++;
                    }
                    else if (currentJolt == _adapterJolts[i] - 3)
                    {
                        threeJoltCount++;
                    }
                    currentJolt = _adapterJolts[i];
                }
            }
            return (oneJoltCount * threeJoltCount).ToString();
        }

        /// <summary>
        /// --- Day 10: Adapter Array; Part B ---
        /// </summary>
        private protected override string PartB()
        {
            _adapterBranches = new Dictionary<int, long>();
            _adapterBranches[_adapterJolts.Last()] = 0;

            long numOfBranches = 0;
            int i = 0;

            while (_adapterJolts[i] <= 3)
            {
                numOfBranches += 1 + CalcTotalBranches(_adapterJolts, i);
                i++;
            }

            return numOfBranches.ToString();
        }

        /// <summary>
        /// Recursively creates the different branches and keeps track of the count.
        /// </summary>
        /// <remarks>
        /// Cycles through each of the three types of valid arrangements (1, 2, 3 jolts) and 
        /// tracks how many valid branches lead to the required jolts.
        /// </remarks>
        /// <param name="adapters">Array of adapters.</param>
        /// <param name="currIndex">Current Index in the list of adapters.</param>
        /// <returns>The count of valid branches</returns>
        private long CalcTotalBranches(int[] adapters, int currIndex)
        {
            int jolt = adapters[currIndex];
            long count = 0;

            for (int i = currIndex + 1; i < adapters.Length && adapters[i] - jolt <= 3; i++)
            {
                if (_adapterBranches.ContainsKey(adapters[i]))
                {
                    count += _adapterBranches[adapters[i]] + 1;
                }
                else
                {
                    count += CalcTotalBranches(adapters, i) + 1;
                }
            }
            
            _adapterBranches[jolt] = count - 1;

            return _adapterBranches[jolt];
        }
    }
}
