using System;
using System.Collections.Generic;
using System.Linq;
using KnapsackProblem.LocalSearchPack;

namespace KnapsackProblem.Knapsack
{
    public interface IKnapsack : IComparable<IKnapsack>
    {
        uint TotalValue { get; }
        uint Capacity { get; }
        List<Item> Items { get; }
        uint MaxCapacity { get; }
    }

    public class EvenKnapsack : IKnapsack
    {
        private readonly List<Item> _items;
        private uint _capacity;
        private bool _even;
        private uint _totalValue;

        public EvenKnapsack(uint maxCapacity)
        {
            MaxCapacity = maxCapacity;
            _totalValue = 0;
            _capacity = 0;
            _even = true;
            _items = new List<Item>();
        }

        public EvenKnapsack(EvenKnapsack other)
        {
            _totalValue = other._totalValue;
            _capacity = other._capacity;
            _even = other._even;
            _items = new List<Item>(other.Items);
            MaxCapacity = other.MaxCapacity;
        }

        public EvenKnapsack(uint maxCapacity, List<Item> initialItems) : this(maxCapacity)
        {
            _items.Capacity = initialItems.Count;
            foreach (var item in initialItems)
            {
                AddItem(item);
            }
        }

        public bool IsLegalKnapsack => _even;

        public bool Even => _even;

        public uint Capacity => _capacity;

        public uint TotalValue => _totalValue;


        public uint MaxCapacity { get; }

        public List<Item> Items => _items;

        public uint AddItem(Item item)
        {
            _items.Add(item);
            _capacity += item.Size;
            _totalValue += item.Value;
            _even = !_even;
            return _capacity;
        }

        public uint RemoveItem(Item item)
        {
            if (!_items.Remove(item)) return _capacity;
            _capacity -= item.Size;
            _totalValue -= item.Value;
            _even = !_even;
            return _capacity;
        }

        public EvenKnapsack Legalize(bool skipPairityCheck = false)
        {
            if (!skipPairityCheck)
            {
                if (IsLegalKnapsack) return this;
            }

            Item.ItemComparisonPolicy = new Item.CompareItemsByValue();
            RemoveItem(_items.Min()); //removing item  with Min value from knapsack if num of element is odd
            return this;
        }

        public override string ToString()
        {
            return $"value: {TotalValue}, size: {Capacity}, quantity: {Items.Count()}";
        }

        public int CompareTo(IKnapsack other)
        {
            if (TotalValue > other.TotalValue || (TotalValue == other.TotalValue && Capacity < other.Capacity))
            {
                return 1;
            }
            if (TotalValue == other.TotalValue && Capacity == other.Capacity)
            {
                return 0;
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns>true if suggested candidate improves current solution</returns>
        public bool WouldBeImprovedBy(SolutionDiff candidate)
        {
            var zeroDiff = new SolutionDiff(); //a default empty candidate
            //check who is better
            return candidate.CompareTo(zeroDiff) > 0;
        }

        public bool WouldBeLegalAfterChange(SolutionDiff candidate)
        {
            if(Capacity + candidate.SizeDiff <= MaxCapacity) /*size limit*/
                return ((candidate.AddedItems.Count - candidate.RemovedItems.Count) % 2 == 0) /*even limit*/ ;
            return false;
        }

        /// <summary>
        /// this method updates candidate (and keeping it legal candidate!) when provided with a legal candidate.
        /// this method does not update unchosenItems
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="unchosenItems"></param>
        public void AddMoreItemsToCandidate(SolutionDiff candidate, HashSet<Item> unchosenItems)
        {
            //calculate the "new capacity" for this candidate
            var newCapacity = Capacity + candidate.SizeDiff;
            var newMaxCapacity = MaxCapacity;

            if (newCapacity > newMaxCapacity) return; //illegal!

            ;
            ;
            ;
            candidate.AddedItems.Clear();
            //candidate.RemovedItems.Clear();
            ;
            ;
            ;
            var newAdds = new List<Item>();
            foreach (var item in unchosenItems)
            {
                if (newCapacity + item.Size > MaxCapacity) continue; //skip this item
                newAdds.Add(item);
                newCapacity += item.Size;
            }
            candidate.AddedItems.AddRange(newAdds); //else add it
            //step 3. make sure candidate offers a legal solution (items.count is even)
            if ((candidate.AddedItems.Count - candidate.RemovedItems.Count)%2 == 0) return; //legal!
            //else - fix candidate
            Item.ItemComparisonPolicy = new Item.CompareItemsByValue();
            candidate.AddedItems.Remove(candidate.AddedItems.Min()); //removing item with Min value from candidate.AddedItems if num of element is odd

        }
    }
}