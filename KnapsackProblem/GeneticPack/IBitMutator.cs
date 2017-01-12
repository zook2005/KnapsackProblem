using System.Collections.Generic;
using System.Linq;
using KnapsackProblem.Helpers;
using KnapsackProblem.Knapsack;

namespace KnapsackProblem.GeneticPack
{
    public interface IBitMutator
    {
        IIndividual Mutate(IIndividual individual);
    }

    /// <summary>
    /// Algorithm: 
    /// 1. let excludedItem to be a randomly chosen item from individual
    /// 1.1 if individual contain no items, let newItem to be a randomly (unchosen) item that is 'small enough' to fit in the bag
    /// 1.1.1 return new individual with newItem
    /// 1.2 else - we know that individual has at least 1 element that can be removed
    /// 2. let allowedItems be all the items in AllItems\individual.knapsack that are 'small enough' to fit in the bag instead of excludedItem 
    /// 3. if allowedItems is empty return new individual with the original list (with/without removing excludedItem)
    /// 4. let newItem(s) to be item(s) chosen from allowedItems (the quantity depends on _addGreedily)
    /// 5. remove excludedItem and add newItem(s)
    /// </summary>
    public class BitMutator : IBitMutator
    {
        /// <summary>
        ///  determine how many items the mutator will try and insert after excluding excludedItem 
        /// </summary>
        private readonly bool _addGreedily;
        /// <summary>
        /// in case there is no suitable item to replace excludedItem, this boolean variable determine whether or not to exclude excludedItem
        /// </summary>
        private readonly bool _removeItem;

        public BitMutator(bool addGreedily, bool removeItem)
        {
            _addGreedily = addGreedily;
            _removeItem = removeItem;
        }

        public IIndividual Mutate(IIndividual individual)
        {
            var includedItems = individual.Chromosome.Where(kvp => kvp.Value.Equals(true)).Select(kvp => kvp.Key).ToList(); //items that will be included in the output

            if (!includedItems.Any()) //in rare cases individual might contain zero items in its knapsack. in that case choose randomly an item
            {
                var allowedSize = individual.Knapsack.MaxCapacity;
               var allowedItems = individual.AllItems.Where(item => item.Size < allowedSize).ToList();
                includedItems = new List<Item>(1) { allowedItems.ChooseRandomElement() };
                return new BitIndividual(includedItems, individual.Knapsack.MaxCapacity);
            }

            else //allowedItems is not empty!
            {
                var excludedItem = includedItems.ChooseRandomElement(); //will be removed
                var allowedSize = individual.Knapsack.MaxCapacity - (individual.Knapsack.Capacity - excludedItem.Size);
                var allowedItems = new HashSet<Item>(individual.AllItems);
                allowedItems.ExceptWith(includedItems); // allowedItems = AllItems\includedItems

                allowedItems.RemoveWhere(item => item.Size > allowedSize); //remove all items too big to fit into knapsack
                if (!allowedItems.Any()) //if allowedItems is empty return the original list with no change
                {
                    if (_removeItem)
                    {
                        includedItems.Remove(excludedItem); //remove the randomly chosen Item 
                    }
                    else
                    {    
                        //do nothing!
                    }
                }
                else
                {
                    includedItems.Remove(excludedItem); //remove the randomly chosen Item 
                    if (_addGreedily)
                    {
                        var allowedItemsList = allowedItems.ToList();
                        allowedItemsList.Shuffle(); //for randomization
                        var littleBag = Knapsacks.GreedyKnapsack(allowedItemsList, allowedSize, false,sortByDensity:true ,reverse:true); //greedily try to add as many new items as possible
                        includedItems.AddRange(littleBag.Items); //add the items to includedItems

                    }
                    else
                    {    
                        includedItems.Add(allowedItems.ChooseRandomElement()); //choose a random element from allowedItems and add it
                    }
                }
            }
            return new BitIndividual(individual.AllItems, new EvenKnapsack(individual.Knapsack.MaxCapacity,includedItems));
        }
    }
}