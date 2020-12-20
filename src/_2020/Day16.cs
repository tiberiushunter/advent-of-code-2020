using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    class Day16 : DayBase
    {
        private readonly string[] _input;
        private readonly Regex _regexFields = new Regex(@"(.*): ([0-9]*)-([0-9]*) or ([0-9]*)-([0-9]*)");

        // All the fields, fully enumerated.
        private Dictionary<string, int[]> _fields;
        private int[] _myTicket;
        private List<int[]> _nearbyTickets;

        /// <summary>
        /// --- Day 16: Ticket Translation ---
        /// </summary>
        public Day16()
        {
            _input = Program.GetInput(2020, 16).Split("\n\n");
        }

        /// <summary>
        /// Formats the intital input into the various collections
        /// </summary>
        private void FormatInput()
        {
            _fields = new Dictionary<string, int[]>();
            _myTicket = new int[20];
            _nearbyTickets = new List<int[]>();


            // Start with the fields
            string[] rawFields = _input[0].Split("\n");
            for (int i = 0; i < rawFields.Length; i++)
            {
                Match m = _regexFields.Match(rawFields[i]);
                if (m.Success)
                {
                    int startA = Int32.Parse(m.Groups[2].Value);
                    int endA = Int32.Parse(m.Groups[3].Value);

                    int startB = Int32.Parse(m.Groups[4].Value);
                    int endB = Int32.Parse(m.Groups[5].Value);

                    int[] range = Enumerable.Range(startA, endA - startA + 1)
                                    .Concat(Enumerable.Range(startB, endB - startB + 1))
                                    .ToArray();

                    _fields.Add(m.Groups[1].Value, range);
                }
            }

            // Next to My Ticket
            _myTicket = _input[1].Split("\n")[1]
                            .Split(",")
                            .Select(n => Convert.ToInt32(n))
                            .ToArray();

            // Finally the Nearby Tickets
            string[] rawNearbyTickets = _input[2].Split("\n");

            for (int i = 1; i < rawNearbyTickets.Length; i++)
            {
                _nearbyTickets.Add(rawNearbyTickets[i]
                            .Split(",")
                            .Select(n => Convert.ToInt32(n))
                            .ToArray());
            }
        }

        /// <summary>
        /// --- Day 16: Ticket Translation; Part A ---
        /// </summary>
        private protected override string PartA()
        {
            FormatInput();

            return RemoveInvalidTickets().ToString();
        }

        /// <summary>
        /// --- Day 16: Ticket Translation; Part B ---
        /// </summary>
        private protected override string PartB()
        {
            FormatInput();
            RemoveInvalidTickets();

            long answer = 1;

            var fieldKeys = _fields.Values.ToArray();
            List<List<int>> keys = new List<List<int>>();
            for (int i = 0; i < fieldKeys.Length; i++)
            {
                List<int> currentValues = new List<int>();

                // Add my ticket
                currentValues.Add(_myTicket[i]);

                // Add all the nearby tickets
                foreach (int[] ticket in _nearbyTickets)
                {
                    currentValues.Add(ticket[i]);
                }

                List<int> currentField = new List<int>();

                for (int j = 0; j < fieldKeys.Length; j++)
                {
                    var notInField = currentValues.Except(fieldKeys[j]).ToArray();

                    if (notInField.Length == 0)
                    {
                        currentField.Add(j);
                    }
                }
                keys.Add(currentField);
            }

            // Used to keep track of the of the field list
            int[] fieldOrder = keys.Select((x, y) => x.Count).ToArray();

            keys.Sort((x, y) => x.Count.CompareTo(y.Count));

            for (int i = 0; i < keys.Count; i++)
            {
                if (i >= 1)
                {
                    string field = _fields.ElementAt(keys[i].Except(keys[i - 1]).ToArray()[0]).Key;
                    if (field.StartsWith("departure "))
                    {
                        for (int j = 0; j < fieldOrder.Length; j++)
                        {
                            if (fieldOrder[j] == i + 1)
                            {
                                answer *= _myTicket[j];
                            }
                        }
                    }
                }
            }

            return answer.ToString();
        }

        /// <summary>
        /// Removes all of the invalid tickets from the initial input.
        /// </summary>
        /// <returns>Error rate (sum of all the invalid values).</returns>
        private int RemoveInvalidTickets()
        {
            int errorRate = 0;
            var fieldKeys = _fields.Values.ToArray();
            List<int[]> invalidTickets = new List<int[]>();
            HashSet<int> allValidValues = new HashSet<int>();

            foreach (int[] field in fieldKeys)
            {
                allValidValues = allValidValues.Concat(field).ToHashSet();
            }

            foreach (int[] ticket in _nearbyTickets)
            {
                int tempErrorRate = ticket.Except(allValidValues).Sum();

                if (tempErrorRate > 0 || ticket.Contains(0))
                {
                    errorRate += tempErrorRate;
                    invalidTickets.Add(ticket);
                }
            }
            for (int i = 0; i < invalidTickets.Count; i++)
            {
                _nearbyTickets.Remove(invalidTickets[i]);
            }

            return errorRate;
        }
    }
}
