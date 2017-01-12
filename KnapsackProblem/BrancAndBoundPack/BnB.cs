using System;
using System.Collections.Generic;
using System.Linq;
using KnapsackProblem.Helpers;
using KnapsackProblem.Knapsack;

namespace KnapsackProblem.BrancAndBoundPack
{
    public class BnB
    {
        public ISortingPolicy Sorter { get; set; }
        
        public BnB(ISortingPolicy sorter, int timesToRun = 1)
        {
            Sorter = sorter;
            this.TimesToRun = timesToRun;
        }

        public int TimesToRun { get; }
        private EvenKnapsack _actingSolution;

        public void Sort(List<Item> items)
        {
            Sorter.Sort(items);
        }

        public EvenKnapsack Algorithm(List<Item> items, uint size)
        {
            Sw = Program.Sw;
            Items = items;
            var itemsLeft = new LinkedList<Item>(items);
            var root = new BnBNode() { Knapsack=Knapsacks.EmptyKnapsack(items,size), Index = -1, ItemsLeft = itemsLeft};
            Sort(Items);
            _actingSolution = Lowerbound(root);
            Sw.WriteLine($"acting solution is:      {_actingSolution}");
            if (!_actingSolution.IsLegalKnapsack)
            {
                throw new Exception("the acting solution must me legal. check your algorithm/input");
            }
            Sort(Items);
            BranchAndBound(root);
            return _actingSolution;
        }

        public MyWriter Sw { get; set; }

        public List<Item> Items { get; set; }

        void BranchAndBound(BnBNode node)
        {

            if(node.Knapsack.Capacity == node.Knapsack.MaxCapacity) return; //bag is full, no need to continue
            if(node.Knapsack.Capacity >  node.Knapsack.MaxCapacity) return; //bag is not legal - skip this solution (probably should be redundant )

            var upperBound = Upperbound(node);
            EvenKnapsack lowerBound = Lowerbound(node);
            Sw.WriteLine($"index: {node.Index}, boundaries: [{lowerBound.TotalValue},{upperBound}]");
            //might have an odd number of items
            if (!lowerBound.IsLegalKnapsack) return;

            if (upperBound <= _actingSolution.TotalValue) return;
            if (upperBound == lowerBound.TotalValue)
            {
                //if lowerBound is better then ActingSolution
                if (lowerBound.TotalValue > _actingSolution.TotalValue)
                {
                    _actingSolution = lowerBound;
                    Sw.WriteLine($"acting solution changed: {_actingSolution}");
                }
                return;
            }
            var children = GenerateChildren(node);
       
            foreach (var child in children)
            {
                BranchAndBound(child);
            }
        }

        private EvenKnapsack Lowerbound(BnBNode node)
        {
            var subItems = node.ItemsLeft.ToList();
            var subMaxCapacity = node.Knapsack.MaxCapacity - node.Knapsack.Capacity;
            var subKnapsack = Knapsacks.GreedyKnapsack(subItems.ToList(), subMaxCapacity, false); //greedily collect new items (might be odd number of items)

            if (node.Knapsack.Even != subKnapsack.Even) //if number of total items is odd
                subKnapsack = subKnapsack.Legalize(true); //remove minimum item from added items

            //at this point the Parity of both knapsacks is unknown. also true for their joined Parity 

            //return a joined knapsack
            return new EvenKnapsack(maxCapacity: node.Knapsack.MaxCapacity,initialItems: node.Knapsack.Items.Concat(subKnapsack.Items).ToList());
        }

        private uint Upperbound(BnBNode node)
        {
            var subItems = node.ItemsLeft.ToList();
            var subMaxCapacity = node.Knapsack.MaxCapacity - node.Knapsack.Capacity;
            var subFractional = Math.Floor(Knapsacks.GreedyFractionalKnapsack(subItems,subMaxCapacity));

            return (uint) (subFractional + node.Knapsack.TotalValue);
        }

        private IEnumerable<BnBNode> GenerateChildren(BnBNode parent)
        {
            LinkedList<Item> itemsLeftFromParent = parent.ItemsLeft;
            LinkedListNode<Item> p = itemsLeftFromParent.First;
            for (p=itemsLeftFromParent.First; p!=null; p = p.Next) //searching for the first item that fits in the knapsack
            {
                itemsLeftFromParent.RemoveFirst(); //we just removed p from the list
                if (p.Value.Size <= parent.Knapsack.MaxCapacity - parent.Knapsack.Capacity)
                {
                    break; //item fits - bingo!
                }
                //else -> no room for this item -> forget this item and move on to the next one
            }
            if (p == null) return new List<BnBNode>(); //no items left

            var copy = new EvenKnapsack(parent.Knapsack);
            copy.AddItem(p.Value);
            var yesNode = new BnBNode { Index = parent.Index+1, Knapsack = copy };

            var noNode = new BnBNode { Index = parent.Index+1, Knapsack = new EvenKnapsack(parent.Knapsack) };


            LinkedList<Item> noList = new LinkedList<Item>();
            LinkedList<Item> yesList = new LinkedList<Item>();

            for (p = itemsLeftFromParent.First; p != null; p = p.Next) //filtering items 'too big'
            {
                if (p.Value.Size <= yesNode.Knapsack.MaxCapacity - yesNode.Knapsack.Capacity) yesList.AddLast(new LinkedListNode<Item>(p.Value));
                if (p.Value.Size <= noNode.Knapsack.MaxCapacity - noNode.Knapsack.Capacity) noList.AddLast(new LinkedListNode<Item>(p.Value));

            }

            noNode.ItemsLeft = noList;
            yesNode.ItemsLeft = yesList;

            return new[]
            {
                yesNode,noNode
            };
        }
    }

}