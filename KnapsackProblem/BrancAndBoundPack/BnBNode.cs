using System.Collections.Generic;
using KnapsackProblem.Knapsack;

namespace KnapsackProblem.BrancAndBoundPack
{
    internal class BnBNode
    {
        public LinkedList<Item> ItemsLeft;

        public BnBNode()
        {
        }

        public int Index { get; set; }

        public EvenKnapsack Knapsack { get; set; }
    }
}