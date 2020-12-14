using System;
using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    class Day12 : DayBase
    {
        private readonly string[] _input;

        private Regex _regEx = new Regex(@"([A-Z])([0-9]+)", RegexOptions.Compiled);

        /// <summary>
        /// --- Day 12: Rain Risk ---
        /// </summary>
        public Day12()
        {
            _input = Program.GetInput(2020, 12).Split("\n");
        }

        /// <summary>
        /// --- Day 12: Rain Risk; Part A ---
        /// </summary>
        private protected override string PartA()
        {
            int x = 0;
            int y = 0;

            int rotation = 90;

            foreach (string instruction in _input)
            {
                Match m = _regEx.Match(instruction);
                if (m.Success)
                {
                    switch (m.Groups[1].Value[0])
                    {
                        case Heading.North:
                            x += Int32.Parse(m.Groups[2].Value);
                            break;

                        case Heading.South:
                            x -= Int32.Parse(m.Groups[2].Value);
                            break;

                        case Heading.East:
                            y += Int32.Parse(m.Groups[2].Value);
                            break;

                        case Heading.West:
                            y -= Int32.Parse(m.Groups[2].Value);
                            break;

                        case Heading.Left:
                            rotation -= Int32.Parse(m.Groups[2].Value);
                            rotation = rotation % 360;
                            if (rotation < 0)
                            {
                                rotation += 360;
                            }
                            break;

                        case Heading.Right:
                            rotation += Int32.Parse(m.Groups[2].Value);
                            rotation = rotation % 360;
                            if (rotation < 0)
                            {
                                rotation += 360;
                            }
                            break;

                        case Heading.Forward:
                            if (rotation == 0)
                            {
                                x += Int32.Parse(m.Groups[2].Value);
                            }
                            else if (rotation == 90)
                            {
                                y += Int32.Parse(m.Groups[2].Value);
                            }
                            else if (rotation == 180)
                            {
                                x -= Int32.Parse(m.Groups[2].Value);
                            }
                            else if (rotation == 270)
                            {
                                y -= Int32.Parse(m.Groups[2].Value);
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            return (Math.Abs(x) + Math.Abs(y)).ToString();
        }

        /// <summary>
        /// --- Day 12: Rain Risk; Part B ---
        /// </summary>
        private protected override string PartB()
        {
            int x = 0;
            int y = 0;

            int wayPointX = 10;
            int wayPointY = 1;

            int rotation = 90;

            int prevWayPointX;

            foreach (string instruction in _input)
            {
                Match m = _regEx.Match(instruction);
                if (m.Success)
                {
                    switch (m.Groups[1].Value[0])
                    {
                        case Heading.North:
                            wayPointY += Int32.Parse(m.Groups[2].Value);
                            break;

                        case Heading.South:
                            wayPointY -= Int32.Parse(m.Groups[2].Value);
                            break;

                        case Heading.East:
                            wayPointX += Int32.Parse(m.Groups[2].Value);
                            break;

                        case Heading.West:
                            wayPointX -= Int32.Parse(m.Groups[2].Value);
                            break;

                        case Heading.Left:
                            rotation = Int32.Parse(m.Groups[2].Value);
                            prevWayPointX = wayPointX;

                            if (rotation == 90)
                            {
                                wayPointX = -wayPointY;
                                wayPointY = prevWayPointX;
                            }
                            else if (rotation == 180)
                            {
                                wayPointX = -wayPointX;
                                wayPointY = -wayPointY;
                            }
                            else if (rotation == 270)
                            {
                                wayPointX = wayPointY;
                                wayPointY = -prevWayPointX;
                            }
                            break;

                        case Heading.Right:
                            rotation = Int32.Parse(m.Groups[2].Value);
                            prevWayPointX = wayPointX;
                            if (rotation == 90)
                            {
                                wayPointX = wayPointY;
                                wayPointY = -prevWayPointX;
                            }
                            else if (rotation == 180)
                            {
                                wayPointX = -wayPointX;
                                wayPointY = -wayPointY;
                            }
                            else if (rotation == 270)
                            {
                                wayPointX = -wayPointY;
                                wayPointY = prevWayPointX;
                            }
                            break;

                        case Heading.Forward:
                            x += wayPointX * Int32.Parse(m.Groups[2].Value);
                            y += wayPointY * Int32.Parse(m.Groups[2].Value);
                            break;

                        default:
                            break;
                    }
                }
            }
            return (Math.Abs(x) + Math.Abs(y)).ToString();
        }

        /// <summary>
        /// Used to store the values for each heading
        /// </summary>
        internal sealed class Heading
        {
            public const char North = 'N';
            public const char South = 'S';
            public const char East = 'E';
            public const char West = 'W';
            public const char Left = 'L';
            public const char Right = 'R';
            public const char Forward = 'F';
        }
    }
}
