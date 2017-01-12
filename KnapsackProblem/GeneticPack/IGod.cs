using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using KnapsackProblem.GeneticPack;
using KnapsackProblem.Helpers;
using KnapsackProblem.Knapsack;
using Microsoft.VisualStudio.GraphModel;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Data;

namespace KnapsackProblem.GeneticPack
{
    /// <summary>
    /// God creates offspring and mutations
    /// </summary>
    public interface IGod
    {
        Tuple<IIndividual,IIndividual> CrossOver(IPopulation population);
        IIndividual Mutate(IPopulation population);
        double PercentageOfMutation { get; set; }
    }

    public class BitGod : IGod
    {
        public BitGod(double percentageOfMutation, IBitMutator bitMutator)
        {
            PercentageOfMutation = percentageOfMutation;
            BitMutator = bitMutator;
        }

        /// <summary>
        /// Algorithm for childA: 
        /// 1. put intersection of A and B in the bag 
        /// 2. let symmetricDifference be A\B U B\A 
        /// 3. shuffle symmetricDifference
        /// 4. add to bag greedily items from symmetricDifference 
        /// algorithm for childB: 
        /// 1. let union be A U B
        /// 2. add to bag greedily items from union 
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public Tuple<IIndividual, IIndividual> CrossOver(IPopulation population)
        {
            List<IIndividual> individuals = population.Individuals;
            int index;
            index = MyExtensions.ChooseByProbability(population.SumOfAllFitness, individuals.Select(ind => ind.Fitness).ToList());
            var firstParent = individuals.ElementAt(index);
            var allItems = firstParent.AllItems;
            var firstParentItems = firstParent.AllItems;
            index = MyExtensions.ChooseByProbability(population.SumOfAllFitness, individuals.Select(ind => ind.Fitness).ToList());
            var secondParent = individuals.ElementAt(index);
            var A = firstParent.Chromosome;
            var B = secondParent.Chromosome;

            var intersection = new List<Item>(allItems.Count);
            var symmetricDifference = new List<Item>(allItems.Count);
            var excluded = new List<Item>(allItems.Count);
            
            for (int i = 0; i < firstParentItems.Count; i++)
            {
                var item = allItems[i];
                if (A[i].Value == true && B[i].Value == true) //if item is in the intersection
                {
                    intersection.Add(item);
                }
                else if (firstParent.Chromosome[i].Value ^ secondParent.Chromosome[i].Value) //item in the Symmetric difference (A\B U B\A)
                {
                    symmetricDifference.Add(item);
                }
                else //item is not a member of any parent
                {
                    excluded.Add(item);
                }
            }

            symmetricDifference.Shuffle(); //this is the list that will create the difference between the child and its parents
            var union = intersection.Concat(symmetricDifference.Concat(excluded)).ToList();
            var firstChild = new BitIndividual(allItems, Knapsacks.GreedyKnapsack(union,population.Alpha.Knapsack.MaxCapacity, reverse: true , sortByDensity:true));

            union.Shuffle(); //this is the list that will create the difference between the child and its parents
            var secondChild = new BitIndividual(allItems, Knapsacks.GreedyKnapsack(union, population.Alpha.Knapsack.MaxCapacity, reverse:false));

            return new Tuple<IIndividual, IIndividual>(firstChild, secondChild);

        }

        /// <summary>
        /// Algorithm: 
        /// 1. randomly choose an individual
        /// 2. call BitMutator.Mutate(individual)
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public IIndividual Mutate(IPopulation population)
        {
            var individuals = population.Individuals;
            var index = MyExtensions.ChooseByProbability(population.SumOfAllFitness, individuals.Select(ind => ind.Fitness).ToList());
            var individual = individuals.ElementAt(index);
            return BitMutator.Mutate(individual);
        }

        public double PercentageOfMutation { get; set; }
        public IBitMutator BitMutator { get; set; }
    }

    public class God : IGod
    {
        public double PercentageOfMutation { get; set; }

        public God(double percentageOfMutation)
        {
            PercentageOfMutation = percentageOfMutation;
        }

        /// <summary>
        /// choose individual from a population by probability ; then, switch the places of two items and creates an individual
        /// </summary>
        /// <param name="population"></param>
        /// <returns></returns>
        public IIndividual Mutate(IPopulation population)
        {
            List<IIndividual> individuals = population.Individuals;

            int index = MyExtensions.ChooseByProbability(population.SumOfAllFitness, individuals.Select(ind=>ind.Fitness).ToList());
            var individual = individuals.ElementAt(index);

            var i = ThreadSafeRandom.ThisThreadsRandom.Next(individual.AllItems.Count);
            var j = ThreadSafeRandom.ThisThreadsRandom.Next(individual.AllItems.Count);

            var copy = individual.AllItems.CopyAndSwap(i, j);
            return new Individual(copy, individual.Knapsack.MaxCapacity);
        }

        /// <summary>
        /// 1. randomly chooses 2 Individuals from the population to be parents
        /// 2. chooses a segment in the lists (randomly) (same segment)
        /// 3. swaps the items in the two lists (that are in the segment)
        /// 4. items that are both inside and outside of the segment ("inside" in one list and "outside" in the other) are swapped too/
        /// </summary>
        /// <param name="population"></param>
        /// <returns>2 new children</returns>
        /// <example>
        /// parent #1 = 3,4,5,2,7,1,6,0
        /// parent #2 = 6,4,0,3,2,1,5,7
        /// segment = [1,6]
        /// child #1 = 7,4,0,3,2,1,6,5
        /// child #2 = 6,4,5,2,7,1,0,3
        /// </example>
        public Tuple<IIndividual, IIndividual> CrossOver(IPopulation population)
        {
            List<IIndividual> individuals = population.Individuals;
            int index;
            index = MyExtensions.ChooseByProbability(population.SumOfAllFitness, individuals.Select(ind => ind.Fitness).ToList());
            var firstParent = individuals.ElementAt(index);
            var firstParentItems = firstParent.AllItems;
            index = MyExtensions.ChooseByProbability(population.SumOfAllFitness, individuals.Select(ind => ind.Fitness).ToList());
            var secondParent = individuals.ElementAt(index);
            var secondParentItems = secondParent.AllItems;

            var i = ThreadSafeRandom.ThisThreadsRandom.Next(firstParent.AllItems.Count);
            var j = ThreadSafeRandom.ThisThreadsRandom.Next(firstParent.AllItems.Count);

            var lowerBoundry = Math.Min(i, j);
            var upperBoundry = Math.Max(i, j);

            var firstChild = foo1(firstParentItems, secondParentItems, lowerBoundry, upperBoundry);
            var secondChild = foo1(secondParentItems, firstParentItems, lowerBoundry, upperBoundry);

            return new Tuple<IIndividual, IIndividual>(new Individual(firstChild, firstParent.Knapsack.MaxCapacity), new Individual(secondChild, firstParent.Knapsack.MaxCapacity));
        }


        private List<Item> foo1(List<Item> firstParent, List<Item> secondParent, int lowerBoundry, int upperBoundry)
        {
            var child = new List<Item>(firstParent);

            var map = new Dictionary<Item, Item>();

            for (int k = lowerBoundry; k < upperBoundry; k++)
            {
                //create an Edge between two arrays, at index k
                map.Add(secondParent[k], firstParent[k]);
                //switch places of elements on the same index (children's list, parents do not change)
                child[k] = secondParent[k];
            }

            foo(lowerBoundry, upperBoundry, child, map, null);
            return child;
        }

        private void foo(int lowerBoundry, int upperBoundry, List<Item> firstChild, Dictionary<Item, Item> currentMap, Dictionary<Item, Item> lastMap)
        {
            var reminder = new Dictionary<Item, Item>();
            foreach (var kvp in currentMap.ToList())
            {
                //search kvp.key outside the [lowerBoundry,upperBoundry] segment. once found replace it with kvp.value and move on to next kvp. if not found, keep it in reminder and search it again in the next recursion step.
                var found = false;

                for (int k = 0; k < lowerBoundry; k++)
                {
                    var x = kvp.Key;
                    if (firstChild[k] == x)
                    {
                        found = true;
                        firstChild[k] = kvp.Value; //switch
                        currentMap.Remove(x);//update current map
                        break;
                    }
                }
                if (found) continue;
                for (int k = upperBoundry; k < firstChild.Count; k++)
                {
                    var x = kvp.Key;
                    if (firstChild[k] == x)
                    {
                        found = true;
                        firstChild[k] = kvp.Value; //switch
                        currentMap.Remove(x);//update current map
                        break;
                    }
                }
                if (found) continue;
                reminder.Add(kvp.Key, kvp.Value); //Edge was not used in this call. keep it to next call

            }
            //if (reminder.ToList().SequenceEqual(currentMap.ToList())) return;
            if (lastMap != null && currentMap.ToList().SequenceEqual(lastMap.ToList())) return;

            foo(lowerBoundry, upperBoundry, firstChild, reminder, currentMap);
        }

    }
}