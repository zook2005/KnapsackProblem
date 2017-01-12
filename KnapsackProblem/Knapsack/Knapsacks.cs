using System.Collections.Generic;

namespace KnapsackProblem.Knapsack
{
    public static class Knapsacks
    {
        public static EvenKnapsack GreedyKnapsack(List<Item> items, uint maxCapacity, bool legalize = true, bool sortByDensity = false, bool reverse = true)
        {
            //step 1. sort by density, big to small. update: step 1 removed to reserve items' order
            Item.ItemComparisonPolicy = new Item.CompareItemsByDensity(); //change sorting policy
            if(sortByDensity) items.Sort();
 
            if(reverse) items.Reverse();

            //step 2. initialize an empty knapsack
            var res = new EvenKnapsack(maxCapacity);

            //greedily add items, till size runs out
            foreach (var item in items)
            {
                var newCapacity = res.Capacity + item.Size;
                if (newCapacity > maxCapacity) continue; //skip this item
                res.AddItem(item);
            }

            if (legalize)
            {
                //step 3. make sure solution is legal (items.count is even)
                return res.Legalize();
            }
            return res;
        }

        public static double GreedyFractionalKnapsack(List<Item> items, uint maxCapacity)
        {
            //step 1. sort by density, big to small
            Item.ItemComparisonPolicy = new Item.CompareItemsByDensity(); //change sorting policy
            items.Sort();
#if DEBUG
            //items.ForEach(i => Trace.WriteLine(i));
#endif
            items.Reverse();

            //step 2. initialize an empty knapsack
            double res = 0;
            double capacity = 0;

            //greedily add items, till size runs out
            foreach (var item in items)
            {
                var newCapacity = capacity + item.Size;
                if (newCapacity <= maxCapacity)
                {
                    res += item.Value;
                }
                else
                {
                    var fraction = (maxCapacity - capacity)/item.Size; //take a fraction of the item
                    res += item.Value*fraction;
                    break;
                }
                capacity += item.Size;
            }
            return res;
        }

        public static EvenKnapsack EmptyKnapsack(List<Item> items, uint maxCapacity)
        {
            return new EvenKnapsack(maxCapacity);
        }

/*        /// <summary>
        /// no sorting. collect pairs till space runs out
        /// </summary>
        /// <param name="items"></param>
        /// <param name="maxCapacity"></param>
        /// <returns></returns>
        public static EvenKnapsack GreedyByPairsKnapsack(List<Item> items, uint maxCapacity)
        {
            var res = new  EvenKnapsack(maxCapacity);
           
            for (int i = 0; i < items.Count; i+=2)
            {
                if (i + 1 < items.Count)
                {
                    var item1 = items.ElementAt(i);
                    var item2 = items.ElementAt(i+1);

                    var newCapacity = res.Capacity + item1.Size + item2.Size;
                    if (newCapacity > maxCapacity) continue; //skip these item2
                    res.AddItem(item1);
                    res.AddItem(item2);
                }
            }
            return res;
        }*/

        public static EvenKnapsack KnapsackWithItems(List<Item> items, uint maxCapacity, List<Item> initialItems)
        {
            if (initialItems == null) return EmptyKnapsack(items, maxCapacity);
            return new EvenKnapsack(maxCapacity,initialItems);
        }

        public static EvenKnapsack KnapsackForGenetic(List<Item> items, uint maxCapacity)
        {
            if (items == null || items.Count == 0) return EmptyKnapsack(items, maxCapacity);
            bool even = true; //even number of items chosen
            var index = 0;
            uint currentItemSize = 0;
            uint size = 0; 
            for (;index < items.Count; index++ )
            {
                currentItemSize = items[index].Size;
                size += currentItemSize;
                if (size <= maxCapacity)
                {
                    //good - move on
                    even = !even;
                } else //this item can't get in - stop looping
                {
                    if (!even)
                        index--;
                    break;
                }
            }
            return  new EvenKnapsack(maxCapacity, items.GetRange(0,index)); //assuming index > 1
        }
    }
}