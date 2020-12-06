
using System;

namespace AdventOfCode
{
    abstract class BaseDay
    {
        public void Solve(){
            Console.WriteLine("Part 1: {0}", PartA());
            Console.WriteLine("Part 2: {0}\n", PartB());
        }
        private protected abstract string PartA();
        private protected abstract string PartB();
    }
}