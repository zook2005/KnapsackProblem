using System.Collections.Generic;

namespace KnapsackProblem.Knapsack
{
    /// <summary>
    /// initializes a new knapsack
    /// </summary>
    public interface IInitializationPolicy
    {
        EvenKnapsack Initialize(List<Item> items, uint maxCapacity,/*only for debug*/ List<Item> initialItems = null);
    }

    public class InitializeEmpty : IInitializationPolicy
    {
        public EvenKnapsack Initialize(List<Item> items, uint maxCapacity,/*only for debug*/ List<Item> initialItems = null)
        {
            return Knapsacks.EmptyKnapsack(items, maxCapacity);
        }
    }
    public class InitializeGreedily : IInitializationPolicy
    {
        public EvenKnapsack Initialize(List<Item> items, uint maxCapacity, /*only for debug*/ List<Item> initialItems = null)
        {
            return Knapsacks.GreedyKnapsack(items, maxCapacity);
        }
    }

    public class InitializeWithItems : IInitializationPolicy
    {
        public EvenKnapsack Initialize(List<Item> items, uint maxCapacity, List<Item> initialItems = null)
        {
            return Knapsacks.KnapsackWithItems(items, maxCapacity,initialItems);
        }
    }
}