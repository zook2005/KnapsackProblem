using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using KnapsackProblem.Knapsack;

namespace KnapsackProblem.GeneticPack
{
    public interface IIndividual : IComparable<IIndividual>
    {
        EvenKnapsack Knapsack { get; }
        uint Fitness { get; }
        List<Item> AllItems { get; }
        List<KeyValuePair<Item, bool>> Chromosome { get; set; }
    }

    public class Individual : IIndividual
    {
        private EvenKnapsack _knapsack;
        private uint _fitness;
        private List<Item> _allItems;
        private List<KeyValuePair<Item, bool>> _chromosome;

        public Individual(IEnumerable<Item> allItems, uint maxCapacity)
        {
            _allItems = allItems.ToList();
            _knapsack = Knapsacks.KnapsackForGenetic(_allItems, maxCapacity);
            _fitness = _knapsack.TotalValue * _knapsack.TotalValue;
        }

        public int CompareTo(IIndividual other)
        {
            return Knapsack.CompareTo(other.Knapsack);
        }

        public EvenKnapsack Knapsack => _knapsack;

        public uint Fitness => _fitness;

        public List<Item> AllItems => _allItems;

        public List<KeyValuePair<Item, bool>> Chromosome
        {
            get { return _chromosome; }
            set { _chromosome = value; }
        }

        public override string ToString()
        {
            return Knapsack.ToString();
        }
    }

    /// <summary>
    /// representation of Individual with a bit array <see cref="Chromosome"/>
    /// </summary>
    public class BitIndividual : IIndividual
    {
        private readonly List<Item> _allItems;

        /// <summary>
        /// initialize with a full list - generate a knapsack
        /// </summary>
        /// <param name="allItems"></param>
        /// <param name="maxCapacity"></param>
        public BitIndividual(List<Item> allItems, uint maxCapacity) : this(allItems, Knapsacks.GreedyKnapsack(allItems, maxCapacity, reverse:false))
        {
                //greedily collect items
        }

        /// <summary>
        /// initialize with a given knapsack
        /// </summary>
        /// <param name="allItems"></param>
        /// <param name="knapsack"></param>
        public BitIndividual(List<Item> allItems, EvenKnapsack knapsack)
        {
            Knapsack = knapsack;

            Fitness = Knapsack.TotalValue;
            var tmpArr = new KeyValuePair<Item, bool>[allItems.Count()];  //add these items with value 'true'
            foreach (var item in Knapsack.Items)
            {
                tmpArr[(int) item.Id] = new KeyValuePair<Item, bool>(item,true);
            }
            var excluded = new HashSet<Item>(allItems);
            excluded.ExceptWith(Knapsack.Items); //add these items with value 'false'
            foreach (var item in excluded)
            {
                tmpArr[(int)item.Id] = new KeyValuePair<Item, bool>(item, false);
            }
            _allItems = new List<Item>(tmpArr.Select(kvp=>kvp.Key));
            Chromosome = tmpArr.ToList();
        }

        public int CompareTo(IIndividual other)
        {
            return Knapsack.CompareTo(other.Knapsack);
        }

        public EvenKnapsack Knapsack { get; }

        public uint Fitness { get; }

        public List<Item> AllItems => _allItems;

        public List<KeyValuePair<Item, bool>> Chromosome { get; set; }

        public override string ToString()
        {
            return Knapsack.ToString();
        }
    }

}
