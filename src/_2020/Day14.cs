using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    class Day14 : DayBase
    {
        private readonly string[] _input;
        private BitArray _bAMaskTrue, _bAMaskFalse, _bAMaskFloating;
        private Dictionary<long, BitArray> _memory; // Full state of Memory
        private readonly Regex _maskRegex = new Regex(@"mask = ([01X]+)");
        private readonly Regex _memRegex = new Regex(@"mem\[([0-9]+)\] = ([0-9]+)");

        HashSet<long> memAddresses = new HashSet<long>();

        /// <summary>
        /// --- Day 14: Docking Data ---
        /// </summary>
        public Day14()
        {
            _input = Program.GetInput(2020, 14).Split("\n");

            // Initialises the Masks
            _bAMaskTrue = new BitArray(36);
            _bAMaskFalse = new BitArray(36);
            _bAMaskFloating = new BitArray(36);
        }

        /// <summary>
        /// --- Day 14: Docking Data; Part A ---
        /// </summary>
        private protected override string PartA()
        {
            // Resets the Memory
            _memory = new Dictionary<long, BitArray>();

            for (int i = 0; i < _input.Length; i++)
            {
                // Firstly we calculate the current mask
                Match matchMask = _maskRegex.Match(_input[i]);
                if (matchMask.Success)
                {
                    for (int j = 0; j < matchMask.Groups[1].Length; j++)
                    {
                        switch (matchMask.Groups[1].Value[j])
                        {
                            case '1':
                                _bAMaskTrue.Set(j, true);
                                break;
                            case '0':
                                _bAMaskFalse.Set(j, true);
                                break;
                        }
                    }
                }
                else
                {
                    continue;
                }

                for (int j = i + 1; j < _input.Length; j++)
                {
                    if (_input[j].StartsWith("mask"))
                    {
                        break;
                    }
                    else
                    {
                        Match matchMem = _memRegex.Match(_input[j]);

                        if (matchMem.Success)
                        {
                            int memAddress = Int32.Parse(matchMem.Groups[1].Value);
                            long value = Int64.Parse(matchMem.Groups[2].Value);

                            var result = GetBitArrayFromInt36(value).Or(_bAMaskTrue).And(_bAMaskFalse.Not());
                            _bAMaskFalse.Not(); // Resets the False mask

                            _memory[memAddress] = result;
                        }
                    }
                }
                _bAMaskTrue = new BitArray(36);
                _bAMaskFalse = new BitArray(36);
            }

            long ans = 0;

            var x = _memory.Keys.ToArray();
            for (int i = 0; i < x.Length; i++)
            {
                ans += GetLongFromBitArray(_memory.GetValueOrDefault(x[i]));
            }
            return ans.ToString();
        }

        /// <summary>
        /// --- Day 14: Docking Data; Part B ---
        /// </summary>
        private protected override string PartB()
        {
            // Resets the Memory
            _memory = new Dictionary<long, BitArray>();

            for (int i = 0; i < _input.Length; i++)
            {
                // Firstly we calculate the current mask
                Match matchMask = _maskRegex.Match(_input[i]);

                List<int> floatPositions = new List<int>();

                if (matchMask.Success)
                {
                    for (int j = 0; j < matchMask.Groups[1].Length; j++)
                    {
                        switch (matchMask.Groups[1].Value[j])
                        {
                            case '1':
                                _bAMaskTrue.Set(j, true);
                                break;
                            case '0':
                                _bAMaskFalse.Set(j, true);
                                break;
                            case 'X':
                                _bAMaskFloating.Set(j, true);
                                floatPositions.Add(j);
                                break;
                        }
                    }
                }
                else
                {
                    continue;
                }

                for (int j = i + 1; j < _input.Length; j++)
                {
                    if (_input[j].StartsWith("mask"))
                    {
                        break;
                    }
                    else
                    {
                        Match matchMem = _memRegex.Match(_input[j]);

                        if (matchMem.Success)
                        {
                            long memAddress = Int64.Parse(matchMem.Groups[1].Value);
                            long value = Int64.Parse(matchMem.Groups[2].Value);

                            var result = GetBitArrayFromInt36(memAddress).Or(_bAMaskTrue).And(_bAMaskFloating.Not());
                            _bAMaskFloating.Not(); // Resets the False mask

                            // Resets the current set of memory addresses
                            memAddresses = new HashSet<long>();

                            // Result doesn't get used here, it's used to recursively find the memory addresses.
                            result = CalculateMemoryAddresses(0, floatPositions, result);

                            for (int h = 0; h < memAddresses.Count; h++)
                            {
                                _memory[memAddresses.ElementAt(h)] = GetBitArrayFromInt36(value);
                            }
                        }
                    }
                }
                // Reset the Masks
                _bAMaskTrue = new BitArray(36);
                _bAMaskFalse = new BitArray(36);
                _bAMaskFloating = new BitArray(36);
            }

            long ans = 0;
            long[] keys = _memory.Keys.ToArray();

            for (int i = 0; i < keys.Length; i++)
            {
                ans += GetLongFromBitArray(_memory.GetValueOrDefault(keys[i]));
            }

            return ans.ToString();
        }

        /// <summary>
        /// Recursively calculates the memory addresses for Part B
        /// </summary>
        /// <param name="position">Current position of the floatPosition array to switch.</param>
        /// <param name="floatPositions">Array of float positions</param>
        /// <param name="bits">Bit Array to convert.</param>
        /// <returns>Bit Array with switched bits.</returns>
        public BitArray CalculateMemoryAddresses(int position, List<int> floatPositions, BitArray bits)
        {
            while (position < floatPositions.Count)
            {
                SwitchBitAtPosition(bits, floatPositions[position]);
                memAddresses.Add(GetLongFromBitArray(bits));

                if (position != floatPositions.Count - 1)
                {
                    bits = CalculateMemoryAddresses(position + 1, floatPositions, bits);
                    SwitchBitAtPosition(bits, floatPositions[position]);
                    memAddresses.Add(GetLongFromBitArray(bits));
                    position++;
                }
                else
                {
                    break;
                }
            }
            return bits;
        }

        /// <summary>
        /// Switches the bool values of a certain bit for a given position.
        /// </summary>
        /// <remarks>
        /// Reverses the polarity of the neutron flow.
        /// </remarks>
        /// <param name="bitArray">Bit Array to switch.</param>
        /// <param name="position">Position in the Bit Array to switch.</param>
        public static void SwitchBitAtPosition(BitArray bitArray, int position)
        {
            bitArray.Set(position, bitArray.Get(position) ? false : true);
        }

        /// <summary>
        /// Prints the current state of a Bit Array.
        /// </summary>
        /// <remarks>
        /// Primarily used for debugging the bit arrays during cycles.
        /// </remarks>
        /// <param name="bitArray">Bit Array to convert.</param>
        private static void PrintBitArray(BitArray bitArray)
        {
            foreach (bool x in bitArray)
            {
                Console.Write("{0}", x ? '1' : '0');
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Calculates the decimal value of a Bit Array.
        /// </summary>
        /// <param name="bitArray">Bit Array to convert.</param>
        /// <returns>Long decimal value of the Bit Array.</returns>
        private static long GetLongFromBitArray(BitArray bitArray)
        {
            long value = 0L;
            long pow = 1L;

            for (int i = bitArray.Length - 1; i >= 0; i--)
            {
                value += ((bitArray.Get(i) ? 1L : 0L) * pow);
                pow *= 2L;
            }

            return value;
        }

        /// <summary>
        /// Converts a Long (but actually its a 36-bit Integer) to a 36-bit Bit Array.
        /// </summary>
        /// <remarks>
        /// There was a whole Int36 struct made for this, but in the end 
        /// cutting the ends off of longs was just easier.
        /// </remarks>
        /// <param name="value">Long (cough Int36) to convert.</param>
        /// <returns>Bit Array representing a 36-bit integer.</returns>
        public static BitArray GetBitArrayFromInt36(long value)
        {
            BitArray bitArray = new BitArray(36);

            for (int i = 35; i >= 0; i--)
            {
                long mask = 1L << i;
                bitArray.Set(35 - i, (value & mask) != 0 ? true : false);
            }
            return bitArray;
        }
    }
}
