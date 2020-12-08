
namespace AdventOfCode._2020
{
    class Day3 : DayBase
    {
        private readonly string _input = Program.GetInput(2020, 3);

        private string[] _inputArr;

        /// <summary>
        /// --- Day 3: Toboggan Trajectory ---
        /// </summary> 
        public Day3()
        {
            // Split the input into an array
            _inputArr = _input.Split("\n");
        }

        /// <summary>
        /// --- Day 3: Toboggan Trajectory; Part A ---
        /// </summary>
        private protected override string PartA()
        {
            return CalcTree(_inputArr, 3, 1).ToString();
        }

        /// <summary>
        /// --- Day 3: Toboggan Trajectory; Part B ---
        /// </summary>
        private protected override string PartB()
        {
            return (CalcTree(_inputArr, 1, 1) *
                CalcTree(_inputArr, 3, 1) *
                CalcTree(_inputArr, 5, 1) *
                CalcTree(_inputArr, 7, 1) *
                CalcTree(_inputArr, 1, 2)).ToString();
        }

        private long CalcTree(string[] inputArr, int x, int y)
        {
            int numOfTrees = 0;

            for (int i = y; i < inputArr.Length; i += y)
            {
                int mod = (x * i) % (inputArr[i - 1].Length);

                if (inputArr[i][mod] == '#')
                {
                    numOfTrees++;
                }
            }
            return numOfTrees;
        }
    }
}