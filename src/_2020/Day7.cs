
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    class Day7 : DayBase
    {
        private readonly string _input = Program.GetInput(2020, 7);
        private Regex _regEx = new Regex(@"(\w+ \w+) bags contain ((?:\d+ \w+ \w+ bags?[\,\.] *)*)", RegexOptions.Compiled);
        private List<Bag> _bags = new List<Bag>();

        /// <summary>
        /// --- Day 7: Handy Haversacks ---
        /// </summary>
        /// <remarks>
        /// You land at the regional airport in time for your next flight. In fact, it looks like you'll even 
        /// have time to grab some food: all flights are currently delayed due to issues in luggage processing.
        /// </remarks>
        public Day7()
        {
            SetBags(_input.Trim().Split("\n"));
        }

        /// <summary>
        /// --- Day 7: Handy Haversacks; Part A ---
        /// </summary>
        /// <remarks>
        /// <para>Due to recent aviation regulations, many rules (your puzzle input) are being enforced about bags and their contents; 
        /// bags must be color-coded and must contain specific quantities of other color-coded bags. 
        /// Apparently, nobody responsible for these regulations considered how long they would take to enforce!</para>
        ///
        /// <para>For example, consider the following rules:</para>
        ///
        /// <code>light red bags contain 1 bright white bag, 2 muted yellow bags.
        /// dark orange bags contain 3 bright white bags, 4 muted yellow bags.
        /// bright white bags contain 1 shiny gold bag.
        /// muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
        /// shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
        /// dark olive bags contain 3 faded blue bags, 4 dotted black bags.
        /// vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
        /// faded blue bags contain no other bags.
        /// dotted black bags contain no other bags.</code>
        ///
        /// <para>These rules specify the required contents for 9 bag types. In this example, every faded blue bag is empty, 
        /// every vibrant plum bag contains 11 bags (5 faded blue and 6 dotted black), and so on.</para>
        ///
        /// <para>You have a shiny gold bag. If you wanted to carry it in at least one other bag, how many different bag colors would be 
        /// valid for the outermost bag? (In other words: how many colors can, eventually, contain at least one shiny gold bag?)</para>
        /// <para>In the above rules, the following options would be available to you:</para>
        /// <code>
        /// A bright white bag, which can hold your shiny gold bag directly.
        /// A muted yellow bag, which can hold your shiny gold bag directly, plus some other bags.
        /// A dark orange bag, which can hold bright white and muted yellow bags, either of which could then hold your shiny gold bag.
        /// A light red bag, which can hold bright white and muted yellow bags, either of which could then hold your shiny gold bag.
        /// </code>
        /// <para>So, in this example, the number of bag colors that can eventually contain at least one shiny gold bag is 4.</para>
        ///
        /// <para>How many bag colors can eventually contain at least one shiny gold bag? 
        /// (The list of rules is quite long; make sure you get all of it.)</para>
        /// </remarks>
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
        /// <remarks>
        /// <para>It's getting pretty expensive to fly these days - not because of ticket prices, but because of the 
        /// ridiculous number of bags you need to buy</para>
        /// 
        /// <para>Consider again your shiny gold bag and the rules from the above example</para>
        ///
        /// <code>faded blue bags contain 0 other bags.
        /// dotted black bags contain 0 other bags.
        /// vibrant plum bags contain 11 other bags: 5 faded blue bags and 6 dotted black bags.
        /// dark olive bags contain 7 other bags: 3 faded blue bags and 4 dotted black bags.</code>
        ///
        /// <para>So, a single shiny gold bag must contain 1 dark olive bag (and the 7 bags within it) plus 2 
        /// vibrant plum bags (and the 11 bags within each of those): 1 + 1*7 + 2 + 2*11 = 32 bags</para>
        ///
        /// <para>Of course, the actual rules have a small chance of going several levels deeper than this example; 
        /// be sure to count all of the bags, even if the nesting becomes topologically impractical</para>
        ///
        /// <para>Here's another example</para>
        ///
        /// <code>shiny gold bags contain 2 dark red bags.
        /// dark red bags contain 2 dark orange bags.
        /// dark orange bags contain 2 dark yellow bags.
        /// dark yellow bags contain 2 dark green bags.
        /// dark green bags contain 2 dark blue bags.
        /// dark blue bags contain 2 dark violet bags.
        /// dark violet bags contain no other bags.</code>
        /// 
        /// <para>In this example, a single shiny gold bag must contain 126 other bags</para>
        /// 
        /// <para>How many individual bags are required inside your single shiny gold bag?</para>
        /// </remarks>
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
                    string[] innerBags = m.Groups[2].ToString().Split(new char[] { ',', '.' }, StringSplitOptions.RemoveEmptyEntries);
                    List<Bag> innerBagsList = new List<Bag>();

                    for (int i = 0; i < innerBags.Length; i++)
                    {
                        string[] innerBagDetails = innerBags[i].Trim().Split(" ");
                        innerBagsList.Add(new Bag(Int32.Parse(innerBagDetails[0]), innerBagDetails[1] + " " + innerBagDetails[2]));
                    }
                    _bags.Add(new Bag(m.Groups[1].ToString(), innerBagsList));
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