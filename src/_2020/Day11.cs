using System;

namespace AdventOfCode._2020
{
    class Day11 : DayBase
    {
        private readonly string[] _input;
        private int[,] _seats;
        private int _maxHeight, _maxWidth;

        /// <summary>
        /// --- Day 11: Seating System ---
        /// </summary>
        public Day11()
        {
            _input = Program.GetInput(2020, 11).Split("\n");

            _maxHeight = _input.Length;
            _maxWidth = _input[0].Length;

            _seats = new int[_maxHeight, _maxWidth];
        }

        /// <summary>
        /// --- Day 11: Seating System; Part A ---
        /// </summary>
        private protected override string PartA()
        {
            ResetSeatValues();

            // Defines rules for Part A
            int ruleEmptyAdj = 9;
            int ruleOccupiedAdj = 4;

            while (true)
            {
                int totalChanges = 0;
                for (int x = 0; x < _maxHeight; x++)
                {
                    for (int y = 0; y < _maxWidth; y++)
                    {
                        int count = 0;
                        int outOfBounds = 0;

                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                int dx = x + i;
                                int dy = y + j;

                                // If the current adjacent seat is out of bounds then increment and continue.
                                if (!CoordsInBounds(dx, dy) || dx == x && dy == y)
                                {
                                    outOfBounds++;
                                    continue;
                                }

                                if (_seats[x, y] == Seat.Empty)
                                {
                                    count += _seats[dx, dy] == Seat.Empty || _seats[dx, dy] == Seat.NextOccupied || _seats[dx, dy] == Seat.Floor ? 1 : 0;
                                }
                                else if (_seats[x, y] == Seat.Occupied)
                                {
                                    count += _seats[dx, dy] == Seat.Occupied || _seats[dx, dy] == Seat.NextEmpty ? 1 : 0;
                                }
                            }
                        }

                        if (_seats[x, y] == Seat.Empty && (count + outOfBounds) >= ruleEmptyAdj)
                        {
                            _seats[x, y] = Seat.NextOccupied;
                            totalChanges++;
                        }
                        else if (_seats[x, y] == Seat.Occupied && count >= ruleOccupiedAdj)
                        {
                            _seats[x, y] = Seat.NextEmpty;
                            totalChanges++;
                        }
                    }
                }
                CalibrateSeats();
                if (totalChanges == 0)
                {
                    break;
                }
            }
            return TotalOccupiedSeats().ToString();
        }

        /// <summary>
        /// --- Day 11: Seating System; Part B ---
        /// </summary>
        private protected override string PartB()
        {
            ResetSeatValues();

            int ruleEmptyAdj = 9;
            int ruleOccupiedAdj = 5;

            while (true)
            {
                int totalChanges = 0;
                for (int x = 0; x < _maxHeight; x++)
                {
                    for (int y = 0; y < _maxWidth; y++)
                    {
                        int count = 0;
                        int outOfBounds = 0;

                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                int dx = x + i;
                                int dy = y + j;

                                // If the current adjacent seat is out of bounds then increment and continue.
                                if (!CoordsInBounds(dx, dy) || dx == x && dy == y)
                                {
                                    outOfBounds++;
                                    continue;
                                }

                                if (_seats[dx, dy] == Seat.Floor)
                                {
                                    char dir = ' ';

                                    if (i > 0 && j == 0)        // Up
                                        dir = 'U';
                                    else if (i < 0 && j == 0)   // Down
                                        dir = 'D';
                                    else if (j > 0 && i == 0)   // Right
                                        dir = 'R';
                                    else if (j < 0 && i == 0)   // Left
                                        dir = 'L';
                                    else if (i > 0 && j < 0)    // Upper Left
                                        dir = 'Q';
                                    else if (i > 0 && j > 0)    // Upper Right
                                        dir = 'W';
                                    else if (i < 0 && j < 0)    // Lower Left
                                        dir = 'A';
                                    else if (i < 0 && j > 0)    // Lower Right
                                        dir = 'S';

                                    if (_seats[x, y] == Seat.Empty || _seats[x, y] == Seat.Occupied)
                                        count += VisiblyAdjacentSeatCheck(x, y, dir, _seats[x, y]);

                                }
                                else if (_seats[x, y] == Seat.Empty)
                                {
                                    count += _seats[dx, dy] == Seat.Empty || _seats[dx, dy] == Seat.NextOccupied || _seats[dx, dy] == Seat.Floor ? 1 : 0;
                                }
                                else if (_seats[x, y] == Seat.Occupied)
                                {
                                    count += _seats[dx, dy] == Seat.Occupied || _seats[dx, dy] == Seat.NextEmpty ? 1 : 0;
                                }
                            }
                        }

                        if (_seats[x, y] == Seat.Empty && (count + outOfBounds) >= ruleEmptyAdj)
                        {
                            _seats[x, y] = Seat.NextOccupied;
                            totalChanges++;
                        }
                        else if (_seats[x, y] == Seat.Occupied && count >= ruleOccupiedAdj)
                        {
                            _seats[x, y] = Seat.NextEmpty;
                            totalChanges++;
                        }
                    }
                }
                CalibrateSeats();
                if (totalChanges == 0)
                {
                    break;
                }
            }
            return TotalOccupiedSeats().ToString();
        }

        /// <summary>
        /// Calculates the total number of Occupied seats.
        /// </summary>
        /// <returns>The total number of Occupied seats.</returns>
        private int TotalOccupiedSeats()
        {
            int answer = 0;
            for (int i = 0; i < _maxHeight; i++)
            {
                for (int j = 0; j < _maxWidth; j++)
                {
                    if (_seats[i, j] == Seat.Occupied)
                    {
                        answer++;
                    }
                }
            }
            return answer;
        }

        /// <summary>
        /// Transforms NewEmpty and NewOccupied types of seats into 
        /// the regular Empty and Occupied seats.
        /// </summary>
        private void CalibrateSeats()
        {
            for (int i = 0; i < _maxHeight; i++)
            {
                for (int j = 0; j < _maxWidth; j++)
                {
                    _seats[i, j] = _seats[i, j] == Seat.Floor ? Seat.Floor : _seats[i, j] == Seat.NextEmpty || _seats[i, j] == Seat.Empty ? Seat.Empty : Seat.Occupied;
                }
            }
        }

        /// <summary>
        /// Resets the Seat values to the initial layout.
        /// </summary>
        private void ResetSeatValues()
        {
            for (int i = 0; i < _maxHeight; i++)
            {
                for (int j = 0; j < _maxWidth; j++)
                {
                    _seats[i, j] = _input[i][j] == 'L' ? Seat.Empty : Seat.Floor;
                }
            }
        }

        /// <summary>
        /// Checks the map for visibly adjacent seats.
        /// </summary>
        /// <remarks>
        /// Can be used for both checking for nearby Empty and Occupied seats depending on the type 
        /// passed in to the seatType paramerter.
        /// </remarks>
        /// <param name="x">X Coordinate to check.</param>
        /// <param name="y">Y Coordinate to check.</param>
        /// <param name="dir">Char used to determine the direction.</param>
        /// <param name="seatType">Initial seat type, used to determine what is used to count.</param>
        /// <returns>1 or 0 depending if there is an adjacent seat.</returns>
        private int VisiblyAdjacentSeatCheck(int x, int y, char dir, int seatType)
        {
            int count = 0;

            if (seatType == Seat.Empty)
                count++;

            switch (dir)
            {
                case 'U':                                           // Up
                    if (x + 1 >= 0)
                    {
                        x++;
                    }
                    else
                    {
                        return count;
                    }
                    break;
                case 'D':                                           // Down
                    if (x - 1 < _maxHeight)
                    {
                        x--;
                    }
                    else
                    {
                        return count;
                    }
                    break;
                case 'R':                                           // Right
                    if (y + 1 < _maxWidth)
                    {
                        y++;
                    }
                    else
                    {
                        return count;
                    }
                    break;
                case 'L':                                           // Left
                    if (y - 1 >= 0)
                    {
                        y--;
                    }
                    else
                    {
                        return count;
                    }
                    break;
                case 'Q':                                           // Upper Left
                    if (x + 1 < _maxHeight && y - 1 >= 0)
                    {
                        x++;
                        y--;
                    }
                    else
                    {
                        return count;
                    }
                    break;
                case 'W':
                    if (x + 1 < _maxHeight && y + 1 < _maxWidth)    // Upper Right
                    {
                        x++;
                        y++;
                    }
                    else
                    {
                        return count;
                    }
                    break;
                case 'A':                                           // Lower Left
                    if (x - 1 >= 0 && y - 1 >= 0)
                    {
                        x--;
                        y--;
                    }
                    else
                    {
                        return count;
                    }
                    break;
                case 'S':                                           // Lower Right
                    if (x - 1 >= 0 && y + 1 < _maxWidth)
                    {
                        x--;
                        y++;
                    }
                    else
                    {
                        return count;
                    }
                    break;
                default:
                    break;
            }

            if (CoordsInBounds(x, y))
            {
                if (_seats[x, y] == Seat.Floor)
                {
                    count = VisiblyAdjacentSeatCheck(x, y, dir, seatType);
                }
                else
                {
                    if (seatType == Seat.Empty)
                    {
                        count = _seats[x, y] == Seat.Empty || _seats[x, y] == Seat.NextOccupied || _seats[x, y] == Seat.Floor ? 1 : 0;
                    }
                    if (seatType == Seat.Occupied)
                    {
                        count = _seats[x, y] == Seat.Occupied || _seats[x, y] == Seat.NextEmpty ? 1 : 0;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Checks if the coordinates are within the bounds of the seating grid.
        /// </summary>
        /// <param name="x">X Coordinate to check.</param>
        /// <param name="y">Y Coordinate to check.</param>
        /// <returns>True if within the bounds of the grid, False if not.</returns>
        private bool CoordsInBounds(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < _maxHeight && y < _maxWidth)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Prints the current state of the map in the input format.
        /// </summary>
        /// <remarks>
        /// Primarily used for debugging the map during cycles.
        /// </remarks>
        private void PrintMap()
        {
            for (int i = 0; i < _maxHeight; i++)
            {
                for (int j = 0; j < _maxWidth; j++)
                {
                    char seat = _seats[i, j] == Seat.Floor ? '.' : _seats[i, j] == Seat.Empty ? 'L' : '#';
                    Console.Write("{0}", seat);
                }
                Console.WriteLine();
            }
        }
    }

    /// <summary>
    /// Used to store the values of each tile in the map of seats
    /// </summary>
    internal sealed class Seat
    {
        public const int Floor = -1;
        public const int Empty = 0;
        public const int Occupied = 1;
        public const int NextEmpty = 3;
        public const int NextOccupied = 4;
    }
}
