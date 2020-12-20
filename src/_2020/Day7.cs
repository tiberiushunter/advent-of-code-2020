
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    class Day7 : DayBase
    {
        private readonly string _input;
        private readonly Regex _regEx = new Regex(@"(\w+ \w+) bags contain ((?:\d+ \w+ \w+ bags?[\,\.] *)*)");
        private List<Bag> _bags = new List<Bag>();

        /// <summary>
        /// --- Day 7: Handy Haversacks ---
        /// </summary>
        public Day7()
        {
            _input = Program.GetInput(2020, 7);
            SetBags(_input.Trim().Split("\n"));
        }

        /// <summary>
        /// --- Day 7: Handy Haversacks; Part A ---
        /// </summary>
        private protected override string PartA()
        {
            int bagCount = 0;
            List<Bag> suspects = new List<Bag>();
            foreach (Bag bag in _bags)
            {
                foreach (Bag innerBag in bag.InnerBags)
                {
                    if (innerBag.Colour == "shiny gold")
                    {
                        suspects.Add(bag);
                    }
                }
            }

            while (bagCount != suspects.Count)
            {
                bagCount = suspects.Count;
                foreach (Bag bag in _bags)
                {
                    foreach (Bag innerBag in bag.InnerBags)
                    {
                        for (int i = 0; i < suspects.Count; i++)
                        {
                            if (innerBag.Colour == suspects.ElementAt(i).Colour)
                            {
                                suspects.Add(bag);
                            }
                        }
                    }
                }

                // Remove duplicates from the list.
                suspects = suspects.Distinct().ToList();
            }

            return bagCount.ToString();
        }

        /// <summary>
        /// --- Day 7: Handy Haversacks; Part B ---
        /// </summary>
        private protected override string PartB()
        {
            int bagCount = 0;
            List<Bag> nextLevelBags = new List<Bag>(); // Bags in the next iteration
            List<Bag> currentLevelBags = new List<Bag>(); // Bags in the current iteration

            // First loop to get the bags inside the Shiny Gold bag.
            foreach (Bag bag in _bags)
            {
                if (bag.Colour == "shiny gold")
                {
                    foreach (Bag innerBag in bag.InnerBags)
                    {
                        for (int i = 0; i < innerBag.NumOfBags; i++)
                        {
                            nextLevelBags.Add(innerBag);
                            bagCount++;
                        }
                    }
                }
            }

            // Loop through each level of bags, incrementing the bag counter for each bag found.
            while (nextLevelBags.Count > 0)
            {
                foreach (Bag bag in _bags)
                {
                    for (int i = 0; i < nextLevelBags.Count; i++)
                    {
                        if (bag.Colour == nextLevelBags.ElementAt(i).Colour)
                        {
                            foreach (Bag innerBag in bag.InnerBags)
                            {
                                for (int j = 0; j < innerBag.NumOfBags; j++)
                                {
                                    currentLevelBags.Add(innerBag);
                                    bagCount++;
                                }
                            }
                        }
                    }
                }
                // Set the next bags to loop through to this loops list.
                nextLevelBags = currentLevelBags;

                // Empty the list for the next loop.
                currentLevelBags = new List<Bag>();
            }
            
            return bagCount.ToString();
        }

        /// <summary>
        /// Creates the List of Bags from the input text
        /// </summary>
        /// <param name="input">Challenge input, split by line in a string[]</param>
        private void SetBags(string[] input)
        {
            foreach (string line in input)
            {
                Match m = _regEx.Match(line);
                if (m.Success)
                {
                    string[] innerBags = m.Groups[2].Value.Split(new char[] { ',', '.' }, StringSplitOptions.RemoveEmptyEntries);
                    List<Bag> innerBagsList = new List<Bag>(innerBags.Length);

                    for (int i = 0; i < innerBags.Length; i++)
                    {
                        string[] innerBagDetails = innerBags[i].Trim().Split(" ");
                        innerBagsList.Add(new Bag(Int32.Parse(innerBagDetails[0]), innerBagDetails[1] + " " + innerBagDetails[2]));
                    }
                    _bags.Add(new Bag(m.Groups[1].Value, innerBagsList));
                }
            }
        }

        /// <summary>
        /// Class for the Bag object
        /// </summary>
        /// <remarks>
        /// Depending on the constructor used it will either be a root bag (i.e it only has 1 of itself and could have children or 
        /// it is a child bag which cannot have children (to match each given rule from the challenge input)
        /// </remarks>
        internal class Bag
        {
            private int numOfBags;
            private string colour;
            private List<Bag> innerBags;

            /// <summary>
            /// Child bag for a rule
            /// </summary>
            public Bag(int numOfBags, string colour)
            {
                this.NumOfBags = numOfBags;
                this.Colour = colour;
            }

            /// <summary>
            /// Parent bag for a rule
            /// </summary>
            public Bag(string colour, List<Bag> innerBags)
            {
                this.NumOfBags = 1;
                this.Colour = colour;
                this.InnerBags = innerBags;
            }

            public string Colour { get => colour; set => colour = value; }
            public List<Bag> InnerBags { get => innerBags; set => innerBags = value; }
            public int NumOfBags { get => numOfBags; set => numOfBags = value; }
        }
    }
}