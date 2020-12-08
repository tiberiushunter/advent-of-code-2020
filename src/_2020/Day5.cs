using System.Linq;

namespace AdventOfCode._2020
{
    class Day5 : DayBase
    {
        private readonly string _input = Program.GetInput(2020, 5);
        private int[] _seats;

        /// <summary>
        /// --- Day 5: Binary Boarding ---
        /// </summary>
        public Day5()
        {
            // Split the input into an array and calculate the seat IDs
            SetSeats(_input.Trim().Split("\n"));
        }

        /// <summary>
        /// --- Day 5: Binary Boarding; Part A ---
        /// </summary>
        private protected override string PartA()
        {
            return _seats.Max().ToString();
        }

        /// <summary>
        /// --- Day 5: Binary Boarding; Part B ---
        /// </summary>
        private protected override string PartB()
        {
            _seats = _seats.OrderBy(c => c).ToArray();

            int emptySeat = Enumerable.Range(_seats.First(), _seats.Last() - _seats.First() + 1)
                                .Except(_seats).ToArray()[0];

            return emptySeat.ToString();
        }

        private void SetSeats(string[] input)
        {
            _seats = new int[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                int[,] rowRange = new int[1, 2] { { 0, 127 } };
                int[,] colRange = new int[1, 2] { { 0, 7 } };

                int row = BinarySearchPartition(rowRange, input[i].Substring(0, 7), 'F');
                int col = BinarySearchPartition(colRange, input[i].Substring(7, 3), 'L');

                _seats[i] = (row * 8) + col;
            }
        }

        private static int BinarySearchPartition(int[,] range, string input, char lowerPart)
        {
            int result = -1;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == lowerPart)
                {
                    range[0, 1] = (range[0, 1] - (range[0, 1] - range[0, 0]) / 2) - 1;

                    if (i == input.Length - 1)
                    {
                        result = range[0, 1];
                    }
                }
                else
                {
                    range[0, 0] = (range[0, 0] + (range[0, 1] - range[0, 0]) / 2) + 1;

                    if (i == input.Length - 1)
                    {
                        result = range[0, 0];
                    }
                }
            }
            return result;
        }
    }
}