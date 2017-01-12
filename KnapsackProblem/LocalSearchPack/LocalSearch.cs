using System.Collections.Generic;
using System.Linq;
using KnapsackProblem.Helpers;
using KnapsackProblem.Knapsack;

namespace KnapsackProblem.LocalSearchPack
{
    public class LocalSearch
    {
        public bool ShuffleNeighborhoodGenerators { get; }

        public LocalSearch(ISortingPolicy sorter,IInitializationPolicy initializer, List<INeighborhoodGenerator> neighborhoodGenerators, ILocalSearchPolicy localSearcher, int timesToRun = 1,bool shuffleNeighborhoodGenerators = false, bool addMoreItems = false)
        {
            AddMoreItemsToCandidate = addMoreItems;
            ShuffleNeighborhoodGenerators = shuffleNeighborhoodGenerators;
            TimesToRun = timesToRun;
            Sorter = sorter;
            Initializer = initializer;
            NeighborhoodGenerators = neighborhoodGenerators;
            LocalSearcher = localSearcher;
        }

        /// <summary>
        /// first step of algorithm is sorting the items
        /// </summary>
        /// <param name="items"></param>
        public void Sort(List<Item> items)
        {
            Sorter.Sort(items);
        }

        /// <summary>
        /// creates a primary solution
        /// </summary>
        /// <param name="items"></param>
        /// <param name="capacity"></param>
        /// <param name="unchosenItems"></param>
        /// <param name="initialItems">only for Debug</param>
        /// <returns></returns>
        public EvenKnapsack Initialize(List<Item> items, uint capacity, HashSet<Item> unchosenItems, /*only for debug*/ List<Item> initialItems = null)
        {
            EvenKnapsack knapsack = Initializer.Initialize(items, capacity,initialItems);

            //remove elements from items (UnchosenItems = items - greedy.Items) //todo: find another way to do this (item should be removed from items when they are chosen, and then we do not need UnchosenItems)
            unchosenItems.UnionWith(items.ToList());
            unchosenItems.ExceptWith(knapsack.Items);

            return knapsack;
        }

        public ISortingPolicy Sorter { get; set; }
        public IInitializationPolicy Initializer { get; set; }
        public List<INeighborhoodGenerator> NeighborhoodGenerators { get; set; }
        public ILocalSearchPolicy LocalSearcher { get; set; }
        public static MyWriter Sw = Program.Sw;
        public EvenKnapsack Algorithm(List<Item> items, uint capacity)
        {
            //step 1. sort items
            Sort(items);

            //step 2. get a knapsack (empty one or greedy one) and update UnchosenItems
            UnchosenItems = new HashSet<Item>();
            
            var knapsack = Initialize(items, capacity, UnchosenItems,null);
            //sw.WriteLine("\t\tinitial value: " + knapsack.TotalValue);
            //Trace.WriteLine("\t\tinitial value: " + knapsack.TotalValue);

            //step 3. search locally for a better solution
            int step = 0;
            bool found = true;
            while (found) //keep searching for a better (local) solution (UnchosenItems and knapsack are updated by SearchLocally)
            {
                if(ShuffleNeighborhoodGenerators) NeighborhoodGenerators.Shuffle();

                var neighborhoodManager = new NeighborhoodManager(UnchosenItems, knapsack, NeighborhoodGenerators,
                    ShuffleNeighborhoodGenerators);
                found = LocalSearcher.SearchLocally(neighborhoodManager);

                step++;

                if (UnchosenItems.Count + knapsack.Items.Count() != items.Count)
                {
                    if (UnchosenItems.Count + knapsack.Items.Count() != items.Count)
                    {
                        Sw.WriteLine($"!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!error in step: {step}");
                    }

                    var sorted = new List<Item>(UnchosenItems);
                    new SortWithComparer(new Item.CompareItemsById()).Sort(sorted);
                }
                if(!found) continue;
               //sw.WriteLine($"\t\tLocal search, step {step}: { knapsack.ToString()}");
               //sw.WriteLine($"\t\t\t{ neighborhoodManager.Current?.ToString()}");
               //Trace.WriteLine($"\t\tLocal search, step {step}: { knapsack.ToString()}");
               //Trace.WriteLine($"\t\t\t{ neighborhoodManager.Current?.ToString()}");
            }

            //step4: Add More Items To Candidate knapsack
            if(AddMoreItemsToCandidate) knapsack.AddMoreItemsToCandidate(new SolutionDiff(), UnchosenItems);


            return knapsack;
        }

        public bool AddMoreItemsToCandidate { get; set; }

        public HashSet<Item> UnchosenItems { get; set; }

        public int TimesToRun { get; }

        /// <summary>
        /// </summary>
        /// <param name="unchosenItems"></param>
        /// <param name="currentSolution"></param>
        /// <param name="neighborhoodGenerators"></param>
        /// <param name="shuffle"></param>
        /// <returns></returns>
        private bool SearchLocally(HashSet<Item> unchosenItems, EvenKnapsack currentSolution, List<INeighborhoodGenerator> neighborhoodGenerators,bool shuffle = true)
        {
            return LocalSearcher.SearchLocally(new NeighborhoodManager(unchosenItems, currentSolution, neighborhoodGenerators,shuffle));
        }

        public override string ToString()
        {
            return string.Format("{0}\n\t{1}\n\t{2}\n\t{3}",
                Initializer.GetType().Name,
                $"NeighborhoodGenerators: {string.Join(",", NeighborhoodGenerators.Select(ng => ng.GetType().Name))}",
                LocalSearcher.GetType().Name,
                Sorter.GetType().Name + (Sorter is SortWithComparer ?
                    $"({((SortWithComparer) Sorter).ItemComparisonPolicy.GetType().Name})" : ""));
        }
    }
}
