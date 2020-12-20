
namespace AdventOfCode._2020
{
    class Day3 : DayBase
    {
        private readonly string[] _input;

        /// <summary>
        /// --- Day 3: Toboggan Trajectory ---
        /// </summary> 
        public Day3()
        {
            _input = Program.GetInput(2020, 3).Split("\n");
        }

        /// <summary>
        /// --- Day 3: Toboggan Trajectory; Part A ---
        /// </summary>
        private protected override string PartA()
        {
            return CalcTree(_input, 3, 1).ToString();
        }

        /// <summary>
        /// --- Day 3: Toboggan Trajectory; Part B ---
        /// </summary>
        private protected override string PartB()
        {
            return (CalcTree(_input, 1, 1) *
                CalcTree(_input, 3, 1) *
                CalcTree(_input, 5, 1) *
                CalcTree(_input, 7, 1) *
                CalcTree(_input, 1, 2)).ToString();
        }

        private long CalcTree(string[] input, int x, int y)
        {
            int numOfTrees = 0;

            for (int i = y; i < input.Length; i += y)
            {
                int mod = (x * i) % (input[i - 1].Length);

                if (input[i][mod] == '#')
                {
                    numOfTrees++;
                }
            }
            return numOfTrees;
        }
    }
}