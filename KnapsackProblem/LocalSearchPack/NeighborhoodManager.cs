using System;
using System.Collections.Generic;
using System.Linq;
using KnapsackProblem.Helpers;
using KnapsackProblem.Knapsack;

namespace KnapsackProblem.LocalSearchPack
{
    public class NeighborhoodManager
    {
        public EvenKnapsack OriginalSolution { get; set; }
        public List<INeighborhoodGenerator> NeighborhoodGenerators { get; set; }
        public HashSet<Item> UnchosenItems { get; set; }
        public IEnumerator<INeighborhoodGenerator> GeneratorsIter { get; set; }
        public SolutionDiff Current { get { return GeneratorsIter.Current?.Current; } } //Current is initiated to null like in Iterator

        public void UpdateOriginalSolution(SolutionDiff diff)
        {
            var removed = diff.RemovedItems;
            var added = diff.AddedItems;
            
            foreach (var newItem in added)
            {
                UnchosenItems.Remove(newItem);
                OriginalSolution.AddItem(newItem);
            }
            foreach (var oldItem in removed)
            {
                OriginalSolution.RemoveItem(oldItem);
                UnchosenItems.Add(oldItem);
            }
        }

        public NeighborhoodManager(HashSet<Item> unchosenItems, EvenKnapsack originalSolution, List<INeighborhoodGenerator> neighborhoodGenerators, bool shuffleNeighborhoodGenerators = false)
        {
            NeighborhoodGenerators = neighborhoodGenerators ?? new List<INeighborhoodGenerator>();
            if(shuffleNeighborhoodGenerators) NeighborhoodGenerators.Shuffle();
         

            OriginalSolution = originalSolution;
            UnchosenItems = unchosenItems;

            //initialize all neighborhoodGenerators
            foreach (var generator in neighborhoodGenerators)
            {
                generator.Initialize(unchosenItems,originalSolution);
            }

            //initialize iterators 
            GeneratorsIter = NeighborhoodGenerators.AsEnumerable().GetEnumerator();
            GeneratorsIter.MoveNext();
        }

        public bool MoveNext()
        {
            while (GeneratorsIter.Current != null) //iterate through all neighborhoods
            {
                bool moreNeighbors = GeneratorsIter.Current.MoveNext(); //next neighbor in current neighborhood
                if (moreNeighbors)
                {
                    return true;
                }
                //else
                GeneratorsIter.MoveNext(); //move to next neighborhood
            }
            return false;
        }
    }

    public class SolutionDiff : IComparable<SolutionDiff>
    {
        public SolutionDiff() : this(new List<Item>(), new List<Item>())
        {
        }

        public SolutionDiff(List<Item> removed, List<Item> added)
        {
            AddedItems = added;
            RemovedItems = removed;
        }
        public long SizeDiff { get { return AddedItems.Sum(i => i.Size) - RemovedItems.Sum(i => i.Size); } }
        public long ValueDiff { get { return AddedItems.Sum(i => i.Value) - RemovedItems.Sum(i => i.Value); } }

        /// <summary>
        /// does not check legality, only decides who is better
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(SolutionDiff other)
        {
            var valueDiff = this.ValueDiff - other.ValueDiff; //suggested knapsack is better if it's value greater
            if (valueDiff > 0) return 1; //we gain more by sticking to the current solution

            if (valueDiff < 0) return -1; //other adds more to the value then @this

            //same value - decide by size
            return - (this.SizeDiff.CompareTo(other.SizeDiff)); //suggested knapsack is better if it's size is smaller
        }

        public List<Item> RemovedItems { get; set; }
        public List<Item> AddedItems { get; set; }
        public override string ToString()
        {
            return $"Items added: {string.Join(",", AddedItems)} \n\t\t\tItems removed: {string.Join(",", RemovedItems)}";
        }
    }
}