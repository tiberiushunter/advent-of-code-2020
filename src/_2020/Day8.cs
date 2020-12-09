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
            var gameBoy = new GameBoy(_input);
            gameBoy.Boot();
            int acc = gameBoy.Acc;

            return acc.ToString();
        }

        /// <summary>
        /// --- Day 8: Handheld Halting; Part B ---
        /// </summary>
        private protected override string PartB()
        {
            int acc = 0;

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

                var gameBoy = new GameBoy(modifiedInput);
                gameBoy.Boot();

                if (gameBoy.BootComplete)
                {
                    acc = gameBoy.Acc;
                    break;
                }

                modifiedInput = (string[])_input.Clone();
            }
            return acc.ToString();
        }

        /// <summary>
        /// Class for the GameBoy object
        /// </summary>
        internal class GameBoy
        {
            private string[] bootCode;
            private int acc = 0;
            private bool bootComplete = false;
            public GameBoy(string[] input)
            {
                this.BootCode = input;
            }

            public string[] BootCode { get => bootCode; set => bootCode = value; }
            public bool BootComplete { get => bootComplete; set => bootComplete = value; }
            public int Acc { get => acc; set => acc = value; }

            /// <summary>
            /// Attempts to boot the GameBoy using the execution instructions
            /// </summary>
            public void Boot()
            {
                int line = 0;
                HashSet<int> visited = new HashSet<int>();

                while (!bootComplete)
                {
                    if (visited.Contains(line))
                    {
                        break;
                    }
                    else
                    {
                        visited.Add(line);
                    }

                    if (line >= BootCode.Length)
                    {
                        bootComplete = true;
                    }
                    else
                    {
                        string[] command = bootCode[line].Split(" ");
                        switch (command[0])
                        {
                            case "acc":
                                Accumulate(Int32.Parse(command[1]));
                                line++;
                                break;

                            case "jmp":
                                line = Jump(Int32.Parse(command[1]), line);
                                break;

                            case "nop":
                                line++;
                                break;
                        }
                    }
                }
            }

            private void Accumulate(int value)
            {
                Acc += value;
            }

            private int Jump(int value, int line)
            {
                return value += line;
            }
        }
    }
}