using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2020
{
    class Day17 : DayBase
    {
        private readonly string[] _input;
        private List<List<List<int>>> _simulation3D;
        private List<List<List<List<int>>>> _simulation4D;

        /// <summary>
        /// --- Day 17: Conway Cubes ---
        /// </summary>
        public Day17()
        {
            _input = Program.GetInput(2020, 17).Split("\n");
        }

        /// <summary>
        /// --- Day 17: Conway Cubes; Part A ---
        /// </summary>
        private protected override string PartA()
        {
            Initialise3D();

            int i = 1;
            while (i <= 6)
            {
                Simulate3D();
                i++;
            }

            int count = 0;
            for (int z = 0; z < _simulation3D.Count; z++)
            {
                for (int y = 0; y < _simulation3D[z].Count; y++)
                {
                    for (int x = 0; x < _simulation3D[z][y].Count; x++)
                    {
                        count += _simulation3D[z][y][x];
                    }
                }
            }

            return count.ToString();
        }

        /// <summary>
        /// --- Day 17: Conway Cubes; Part B ---
        /// </summary>
        private protected override string PartB()
        {
            Initialise4D();

            int i = 1;
            while (i <= 6)
            {
                Simulate4D();
                i++;
            }

            int count = 0;
            for (int w = 0; w < _simulation4D.Count; w++)
            {
                for (int z = 0; z < _simulation4D[w].Count; z++)
                {
                    for (int y = 0; y < _simulation4D[w][z].Count; y++)
                    {
                        for (int x = 0; x < _simulation4D[w][z][y].Count; x++)
                        {
                            count += _simulation4D[w][z][y][x];
                        }
                    }
                }
            }

            return count.ToString();
        }

        /// <summary>
        /// Initialises the system for a 3D simulation
        /// </summary>
        private void Initialise3D()
        {
            // Reset the current simulation
            _simulation3D = new List<List<List<int>>>();

            List<List<int>> currentZ = new List<List<int>>();
            for (int i = 0; i < _input.Length; i++)
            {
                List<int> currentY = new List<int>();
                for (int j = 0; j < _input[i].Length; j++)
                {
                    switch (_input[i][j])
                    {
                        case '.':
                            currentY.Add(Cube.Inactive);
                            break;
                        case '#':
                            currentY.Add(Cube.Active);
                            break;
                    }
                }
                currentZ.Add(currentY);
            }
            _simulation3D.Add(currentZ);
        }

        /// <summary>
        /// Initialises the system for a 4D simulation
        /// </summary>
        private void Initialise4D()
        {
            // Reset the current simulation
            _simulation4D = new List<List<List<List<int>>>>();
            List<List<List<int>>> currentState = new List<List<List<int>>>();

            List<List<int>> currentZ = new List<List<int>>();
            for (int i = 0; i < _input.Length; i++)
            {
                List<int> currentY = new List<int>();
                for (int j = 0; j < _input[i].Length; j++)
                {
                    switch (_input[i][j])
                    {
                        case '.':
                            currentY.Add(Cube.Inactive);
                            break;
                        case '#':
                            currentY.Add(Cube.Active);
                            break;
                    }
                }
                currentZ.Add(currentY);
            }
            currentState.Add(currentZ);
            _simulation4D.Add(currentState);
        }

        /// <summary>
        /// Simulates the system for a 3D simulation
        /// </summary>
        private void Simulate3D()
        {
            // First add padding around existing layers (X, Y)
            for (int z = 0; z < _simulation3D.Count; z++)
            {
                for (int y = 0; y < _simulation3D[z].Count; y++)
                {
                    _simulation3D[z][y].Insert(0, Cube.Inactive);
                    _simulation3D[z][y].Add(Cube.Inactive);
                }
                _simulation3D[z].Insert(0, _simulation3D[z].First().Select(x => { x = Cube.Inactive; return x; }).ToList());
                _simulation3D[z].Add(_simulation3D[z].First().Select(x => { x = Cube.Inactive; return x; }).ToList());
            }

            // Next add another Z to both ends
            List<List<int>> newFirstZ = new List<List<int>>();
            List<List<int>> newLastZ = new List<List<int>>();

            foreach (List<int> y in _simulation3D.First())
            {
                newFirstZ.Add(y.Select(x => { x = Cube.Inactive; return x; }).ToList());
                newLastZ.Add(y.Select(x => { x = Cube.Inactive; return x; }).ToList());
            }

            _simulation3D.Insert(0, newFirstZ);
            _simulation3D.Add(newLastZ);

            // Next perform the simulation.
            for (int z = 0; z < _simulation3D.Count; z++)
            {
                for (int y = 0; y < _simulation3D[z].Count; y++)
                {
                    for (int x = 0; x < _simulation3D[z][y].Count; x++)
                    {
                        int count = 0;

                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                for (int k = -1; k <= 1; k++)
                                {
                                    int dz = z + i;
                                    int dy = y + j;
                                    int dx = x + k;

                                    // If the current adjacent state is out of bounds then continue.
                                    if (!CoordsInBounds3D(dx, dy, dz, _simulation3D) || (dx == x && dy == y && dz == z))
                                    {
                                        continue;
                                    }

                                    if (_simulation3D[z][y][x] == Cube.Inactive)
                                    {
                                        count += _simulation3D[dz][dy][dx] == Cube.Active || _simulation3D[dz][dy][dx] == Cube.NextInactive ? 1 : 0;
                                    }
                                    else if (_simulation3D[z][y][x] == Cube.Active)
                                    {
                                        count += _simulation3D[dz][dy][dx] == Cube.Active || _simulation3D[dz][dy][dx] == Cube.NextInactive ? 1 : 0;
                                    }
                                }
                            }
                        }
                        if (_simulation3D[z][y][x] == Cube.Inactive && (count == 3))
                        {
                            _simulation3D[z][y][x] = Cube.NextActive;
                        }
                        else if (_simulation3D[z][y][x] == Cube.Active && ((count != 2) && (count != 3)))
                        {
                            _simulation3D[z][y][x] = Cube.NextInactive;
                        }
                    }
                }
            }

            // Finally calibrate the cube states to either an Active or Inactive state.
            for (int z = 0; z < _simulation3D.Count; z++)
            {
                for (int y = 0; y < _simulation3D[z].Count; y++)
                {
                    for (int x = 0; x < _simulation3D[z][y].Count; x++)
                    {
                        switch (_simulation3D[z][y][x])
                        {
                            case Cube.NextInactive:
                                _simulation3D[z][y][x] = Cube.Inactive;
                                break;
                            case Cube.NextActive:
                                _simulation3D[z][y][x] = Cube.Active;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Simulates the system for a 4D simulation
        /// </summary>
        private void Simulate4D()
        {
            // First add padding around existing layers (X, Y)
            for (int w = 0; w < _simulation4D.Count; w++)
            {
                for (int z = 0; z < _simulation4D[w].Count; z++)
                {
                    for (int y = 0; y < _simulation4D[w][z].Count; y++)
                    {
                        _simulation4D[w][z][y].Insert(0, Cube.Inactive);
                        _simulation4D[w][z][y].Add(Cube.Inactive);
                    }
                    _simulation4D[w][z].Insert(0, _simulation4D[w][z].First().Select(x => { x = Cube.Inactive; return x; }).ToList());
                    _simulation4D[w][z].Add(_simulation4D[w][z].First().Select(x => { x = Cube.Inactive; return x; }).ToList());
                }

                // Next add another Z to both ends
                List<List<int>> newFirstZ = new List<List<int>>();
                List<List<int>> newLastZ = new List<List<int>>();

                foreach (List<int> slice in _simulation4D[w].First())
                {
                    newFirstZ.Add(slice.Select(x => { x = Cube.Inactive; return x; }).ToList());
                    newLastZ.Add(slice.Select(x => { x = Cube.Inactive; return x; }).ToList());
                }

                _simulation4D[w].Insert(0, newFirstZ);
                _simulation4D[w].Add(newLastZ);
            }

            // Next add another empty W to both ends
            List<List<List<int>>> newFirstW = new List<List<List<int>>>();
            List<List<List<int>>> newLastW = new List<List<List<int>>>();

            foreach (List<List<int>> w in _simulation4D.First())
            {
                // Next add another Z to both ends
                List<List<int>> newFirstZ = new List<List<int>>();
                List<List<int>> newLastZ = new List<List<int>>();

                foreach (List<int> z in w)
                {
                    newFirstZ.Add(z.Select(x => { x = Cube.Inactive; return x; }).ToList());
                    newLastZ.Add(z.Select(x => { x = Cube.Inactive; return x; }).ToList());
                }
                newFirstW.Add(newFirstZ);
                newLastW.Add(newLastZ);
            }

            _simulation4D.Insert(0, newFirstW);
            _simulation4D.Add(newLastW);

            // Next perform the simulation.
            for (int w = 0; w < _simulation4D.Count; w++)
            {
                for (int z = 0; z < _simulation4D[w].Count; z++)
                {
                    for (int y = 0; y < _simulation4D[w][z].Count; y++)
                    {
                        for (int x = 0; x < _simulation4D[w][z][y].Count; x++)
                        {
                            int count = 0;

                            for (int i = -1; i <= 1; i++)
                            {
                                for (int j = -1; j <= 1; j++)
                                {
                                    for (int k = -1; k <= 1; k++)
                                    {
                                        for (int l = -1; l <= 1; l++)
                                        {
                                            int dw = w + i;
                                            int dz = z + j;
                                            int dy = y + k;
                                            int dx = x + l;

                                            // If the current adjacent state is out of bounds then continue.
                                            if (!CoordsInBounds4D(dx, dy, dz, dw, _simulation4D) || (dx == x && dy == y && dz == z && dw == w))
                                            {
                                                continue;
                                            }

                                            if (_simulation4D[w][z][y][x] == Cube.Inactive)
                                            {
                                                count += _simulation4D[dw][dz][dy][dx] == Cube.Active || _simulation4D[dw][dz][dy][dx] == Cube.NextInactive ? 1 : 0;
                                            }
                                            else if (_simulation4D[w][z][y][x] == Cube.Active)
                                            {
                                                count += _simulation4D[dw][dz][dy][dx] == Cube.Active || _simulation4D[dw][dz][dy][dx] == Cube.NextInactive ? 1 : 0;
                                            }
                                        }
                                    }
                                }
                            }
                            if (_simulation4D[w][z][y][x] == Cube.Inactive && (count == 3))
                            {
                                _simulation4D[w][z][y][x] = Cube.NextActive;
                            }
                            else if (_simulation4D[w][z][y][x] == Cube.Active && ((count != 2) && (count != 3)))
                            {
                                _simulation4D[w][z][y][x] = Cube.NextInactive;
                            }
                        }
                    }
                }
            }

            // Finally calibrate the cube states to either an Active or Inactive state.
            for (int w = 0; w < _simulation4D.Count; w++)
            {
                for (int z = 0; z < _simulation4D[w].Count; z++)
                {
                    for (int y = 0; y < _simulation4D[w][z].Count; y++)
                    {
                        for (int x = 0; x < _simulation4D[w][z][y].Count; x++)
                        {
                            switch (_simulation4D[w][z][y][x])
                            {
                                case Cube.NextInactive:
                                    _simulation4D[w][z][y][x] = Cube.Inactive;
                                    break;
                                case Cube.NextActive:
                                    _simulation4D[w][z][y][x] = Cube.Active;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Prints the current state of the simulation in 3D slices
        /// </summary>
        /// <remarks>
        /// Primarily used for debugging the simulation during cycles.
        /// </remarks>
        private void PrintState3D()
        {
            for (int z = 0; z < _simulation3D.Count; z++)
            {
                Console.WriteLine("z = {0}", z);
                for (int y = 0; y < _simulation3D[z].Count; y++)
                {
                    for (int x = 0; x < _simulation3D[z][y].Count; x++)
                    {
                        char cube = _simulation3D[z][y][x] == Cube.Inactive ? '.' : _simulation3D[z][y][x] == Cube.Active ? '#' : _simulation3D[z][y][x] == Cube.NextInactive ? ',' : '@';
                        Console.Write("{0}", cube);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Prints the current state of the simulation in 4D slices
        /// </summary>
        /// <remarks>
        /// Primarily used for debugging the simulation during cycles.
        /// </remarks>
        private void PrintState4D()
        {
            for (int w = 0; w < _simulation4D.Count; w++)
            {
                Console.WriteLine("w = {0}", w);
                for (int z = 0; z < _simulation4D[w].Count; z++)
                {
                    Console.WriteLine(" z = {0}", z);
                    for (int y = 0; y < _simulation4D[w][z].Count; y++)
                    {
                        for (int x = 0; x < _simulation4D[w][z][y].Count; x++)
                        {
                            char cube = _simulation4D[w][z][y][x] == Cube.Inactive ? '.' : _simulation4D[w][z][y][x] == Cube.Active ? '#' : _simulation4D[w][z][y][x] == Cube.NextInactive ? ',' : '@';
                            Console.Write("{0}", cube);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// Checks if the coordinates are within the bounds of the simulation.
        /// </summary>
        /// <param name="x">X Coordinate to check.</param>
        /// <param name="y">Y Coordinate to check.</param>
        /// <param name="z">Z Coordinate to check.</param>
        /// <param name="grid">Grid to check.</param>
        /// <returns>True if within the bounds of the grid, False if not.</returns>
        private static bool CoordsInBounds3D(int x, int y, int z, List<List<List<int>>> grid)
        {
            if (x >= 0 && y >= 0 && z >= 0 && z < grid.Count && y < grid[z].Count && x < grid[z][y].Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the coordinates are within the bounds of the simulation.
        /// </summary>
        /// <param name="x">X Coordinate to check.</param>
        /// <param name="y">Y Coordinate to check.</param>
        /// <param name="z">Z Coordinate to check.</param>
        /// <param name="w">W Coordinate to check.</param>
        /// <param name="grid">Grid to check.</param>
        /// <returns>True if within the bounds of the grid, False if not.</returns>
        private static bool CoordsInBounds4D(int x, int y, int z, int w, List<List<List<List<int>>>> grid)
        {
            if (x >= 0 && y >= 0 && z >= 0 && w >= 0 && w < grid.Count && z < grid[w].Count && y < grid[w][z].Count && x < grid[w][z][y].Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Used to store the state of each cube
        /// </summary>
        internal sealed class Cube
        {
            public const int Inactive = 0;
            public const int Active = 1;
            public const int NextInactive = 2;
            public const int NextActive = 3;
        }
    }
}
