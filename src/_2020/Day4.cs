using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    class Day4 : DayBase
    {
        private readonly string _input = Program.GetInput(2020, 4);
        private string[] _inputArr;

        /// <summary>
        /// --- Day 4: Passport Processing ---
        /// </summary> 
        public Day4()
        {
            // Split the input into an array of passports
            _inputArr = _input.Split(new string[] { "\n\n" },
                               StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// --- Day 4: Passport Processing; Part A ---
        /// </summary> 
        private protected override string PartA()
        {
            int count = 0;
            for (int i = 0; i < _inputArr.Length; i++)
            {
                if (HasAllRequiredFields(_inputArr[i]))
                {
                    count++;
                }
            }
            return count.ToString();
        }

        /// <summary>
        /// --- Day 4: Passport Processing; Part B ---
        /// </summary>
        private protected override string PartB()
        {
            int count = 0;
            string[] eclValidArr = new string[7] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

            for (int i = 0; i < _inputArr.Length; i++)
            {
                if (HasAllRequiredFields(_inputArr[i]))
                {
                    // Then add the current passport to a dictionary
                    Dictionary<string, string> d = _inputArr[i].Replace("\n", " ").Split(' ')
                        .Select(value => value.Split(':'))
                        .ToDictionary(pair => pair[0], pair => pair[1]);

                    int byr = Int32.Parse(d["byr"]);
                    int iyr = Int32.Parse(d["iyr"]);
                    int eyr = Int32.Parse(d["eyr"]);

                    if (byr >= 1920 && byr <= 2002 &&
                        iyr >= 2010 && iyr <= 2020 &&
                        eyr >= 2020 && eyr <= 2030)
                    {
                        if (d["hgt"].EndsWith("cm") &&
                            (Int32.Parse(d["hgt"].Substring(0, d["hgt"].Length - 2)) >= 150 && Int32.Parse(d["hgt"].Substring(0, d["hgt"].Length - 2)) <= 193) ||
                            d["hgt"].EndsWith("in") &&
                            (Int32.Parse(d["hgt"].Substring(0, d["hgt"].Length - 2)) >= 59 && Int32.Parse(d["hgt"].Substring(0, d["hgt"].Length - 2)) <= 76))
                        {
                            if (Regex.Match(d["hcl"], "^#(?:[0-9a-fA-F]{3}){1,2}$").Success)
                            {
                                if (eclValidArr.Any(s => d["ecl"].Contains(s)))
                                {
                                    if (d["pid"].Length == 9)
                                    {
                                        count++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return count.ToString();
        }

        private bool HasAllRequiredFields(string input)
        {
            if (input.Contains("byr:") &&
                    input.Contains("iyr:") &&
                    input.Contains("eyr:") &&
                    input.Contains("hgt:") &&
                    input.Contains("hcl:") &&
                    input.Contains("ecl:") &&
                    input.Contains("pid:"))
            {
                return true;
            }
            else { return false; }
        }
    }
}