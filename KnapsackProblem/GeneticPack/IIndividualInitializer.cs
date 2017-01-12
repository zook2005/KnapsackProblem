using System.Collections.Generic;
using KnapsackProblem.Knapsack;

namespace KnapsackProblem.GeneticPack
{
    public interface IIndividualInitializer
    {
        IIndividual GetInstance(List<Item> items, uint knapsackMaxCapacity);
    }

    class BitIndividualInitializer : IIndividualInitializer
    {
        public IIndividual GetInstance(List<Item> items, uint knapsackMaxCapacity)
        {
            return  new BitIndividual(items,knapsackMaxCapacity);
        }
    }

    public class IndividualInitializer : IIndividualInitializer
    {
        public IIndividual GetInstance(List<Item> items, uint knapsackMaxCapacity)
        {
            return new Individual(items, knapsackMaxCapacity);
        }
    }
}