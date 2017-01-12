using System.Collections.Generic;
using KnapsackProblem.Helpers;

namespace KnapsackProblem.Knapsack
{
    public interface ISortingPolicy
    {
        void Sort(List<Item> items);
    }

    public class AsIsSortingPolicy : ISortingPolicy
    {
        public void Sort(List<Item> items)
        {
            //do nothing - list of items should stay "as is"
        }
    }

    public class ShuffleSortingPolicy : ISortingPolicy
    {
        public void Sort(List<Item> items)
        {
            items.Shuffle();
        }
    }

    public class SortWithComparer : ISortingPolicy
    {
        private readonly bool _reversed;

        public SortWithComparer(Item.IItemComparisonPolicy itemComparisonPolicy, bool reversed = false)
        {
            _reversed = reversed;
            ItemComparisonPolicy = itemComparisonPolicy;
        }

        public Item.IItemComparisonPolicy ItemComparisonPolicy { get; set; }

        public void Sort(List<Item> items)
        {
            Item.ItemComparisonPolicy = ItemComparisonPolicy;
            items.Sort();
            if(_reversed) items.Reverse();
        }
    }
}
