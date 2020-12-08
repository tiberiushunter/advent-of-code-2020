using System;
using System.Collections.Generic;

namespace AdventOfCode._2020
{
    class Day8 : DayBase
    {
        private readonly string[] _input;

        /// <summary>
        /// --- Day 8: Handheld Halting ---
        /// </summary>
        public Day8()
        {
            _input = Program.GetInput(2020, 8).Split("\n");
        }

        /// <summary>
        /// --- Day 8: Handheld Halting; Part A ---
        /// </summary>
        private protected override string PartA()
        {
            int acc = 0;
            int line = 0;
            HashSet<int> visited = new HashSet<int>();

            while (true)
            {
                if (visited.Contains(line))
                {
                    break;
                }
                else
                {
                    visited.Add(line);
                }
                string[] command = _input[line].Split(" ");
                switch (command[0])
                {
                    case "acc":
                        acc = acc + Int32.Parse(command[1]);
                        line++;
                        break;

                    case "jmp":
                        line = line + Int32.Parse(command[1]);
                        break;

                    case "nop":
                        line++;
                        break;
                }
            }
            return acc.ToString();
        }

        /// <summary>
        /// --- Day 8: Handheld Halting; Part B ---
        /// </summary>
        private protected override string PartB()
        {
            int acc = 0;
            bool endOfProgram = false;

            string[] originalInput = (string[])_input.Clone();
            string[] modifiedInput = (string[])_input.Clone();

            for (int i = 0; i < modifiedInput.Length; i++)
            {
                if (modifiedInput[i].StartsWith("jmp"))
                {
                    modifiedInput[i] = modifiedInput[i].Replace("jmp", "nop");
                }
                else if (modifiedInput[i].StartsWith("nop"))
                {
                    modifiedInput[i] = modifiedInput[i].Replace("nop", "jmp");
                }
                else
                {
                    continue;
                }

                HashSet<int> visited = new HashSet<int>();
                int tempAcc = 0;
                int line = 0;

                while (!endOfProgram)
                {
                    if (visited.Contains(line))
                    {
                        break;
                    }
                    else
                    {
                        visited.Add(line);
                    }

                    if (line >= modifiedInput.Length)
                    {
                        acc = tempAcc;
                        endOfProgram = true;
                    }
                    else
                    {
                        string[] command = modifiedInput[line].Split(" ");
                        switch (command[0])
                        {
                            case "acc":
                                tempAcc = tempAcc + Int32.Parse(command[1]);
                                line++;
                                break;

                            case "jmp":
                                line = line + Int32.Parse(command[1]);
                                break;

                            case "nop":
                                line++;
                                break;
                        }
                    }
                }
                modifiedInput = (string[])originalInput.Clone();
            }
            return acc.ToString();
        }
    }
}