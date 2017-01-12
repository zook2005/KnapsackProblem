using System.Collections.Generic;
using System.Linq;
using KnapsackProblem.Knapsack;

namespace KnapsackProblem.LocalSearchPack
{
    public interface INeighborhoodGenerator
    {
        /*List<Item>.Enumerator SolutionIter { get; set; }
        EvenKnapsack CurrentSolution { get; set; }
        
        HashSet<Item>.Enumerator UnchosenIter { get; set; }
        HashSet<Item> UnchosenItems { get; set; }*/
        SolutionDiff Current { get; set; }
        bool MoveNext();
        //void Initialize(HashSet<Item> unchosenItems, EvenKnapsack currentSolution, List<Item> addedItems, List<Item> removedItems);
        void Initialize(HashSet<Item> unchosenItems, EvenKnapsack knapsack);
    }

    public class OnePerOne : INeighborhoodGenerator
    {
        private System.Collections.Generic.IEnumerator<KnapsackProblem.Knapsack.Item> _solutionIter;
        private HashSet<Item>.Enumerator _unchosenIter;
        private EvenKnapsack _knapsack;
        private HashSet<Item> _unchosenItems;

        public SolutionDiff Current { get; set; }

        public void Initialize(HashSet<Item> unchosenItems, EvenKnapsack knapsack)
        {
            Current = null;
            _unchosenItems = unchosenItems;
            _knapsack = knapsack;
            _solutionIter = _knapsack.Items.GetEnumerator();
            _unchosenIter = _unchosenItems.GetEnumerator();
            _solutionIter.MoveNext();
        }

        public bool MoveNext()
        {
            Item.ItemComparisonPolicy = new Item.CompareItemsByValue();

            //for each item in currentSolution.Items - try replace it with an unchosen item from unChosenItems
            while (_solutionIter.Current != null)
            {
                var oldItem = _solutionIter.Current; //item to be removed from knapsack
                while (_unchosenIter.MoveNext())
                {
                    var newItem = _unchosenIter.Current; //item to be added to knapsack
                    var candidate = new SolutionDiff(new List<Item> { oldItem }, new List<Item> { newItem });

                    if (!_knapsack.WouldBeLegalAfterChange(candidate)) continue; //if this will result in an illegal knapsack - skip this
                    Current = candidate;
                    return true;
                }
                _unchosenIter = _unchosenItems.GetEnumerator();
                _solutionIter.MoveNext();
            }
            Current = null; //no more items -> update current to null
            return false;            
        }
    }


    public class AddTwo : INeighborhoodGenerator
    {
        private EvenKnapsack _knapsack;
        private Item[] _unchosenItems   ;
        private int _unchosenSize;
        private int _uIter1;
        private int _uIter2;


        public SolutionDiff Current { get; set; }
        public bool MoveNext()
        {

            Item.ItemComparisonPolicy = new Item.CompareItemsByValue();



                        
                        while (_uIter1 < _unchosenSize - 1)
                        {
                            var newItem1 = _unchosenItems[_uIter1]; //item to be added to knapsack
                            while (_uIter2 < _unchosenSize)
                            {
                                var newItem2 = _unchosenItems[_uIter2]; //item to be added to knapsack
                                _uIter2++;
                                var candidate = new SolutionDiff(new List<Item> {},
                                    new List<Item> {newItem1, newItem2});

                                if (!_knapsack.WouldBeLegalAfterChange(candidate))
                                    continue; //if this candid will result in an illegal knapsack - skip this
                                Current = candidate;
                                return true;
                            }
                _uIter1++;
                _uIter2 = _uIter1 + 1;

            }


            Current = null; //no more items -> update current to null
                return false;
            
        }

        public void Initialize(HashSet<Item> unchosenItems, EvenKnapsack knapsack)
        {
            Current = null;
            _knapsack = knapsack;
            _unchosenItems = unchosenItems.ToArray();
            _unchosenSize = unchosenItems.Count;
            _uIter1 = 0;
            _uIter2 = _uIter1 + 1; 
        }
    }

    public class ThreePerOne : INeighborhoodGenerator
    {
        private int _solutionIter1;
        private int _solutionIter2;
        private int _solutionIter3;
        private EvenKnapsack _knapsack;
        private Item[] _unchosenItems;
        private Item[] _solutionItems;
        private int _uIter;
        private int _unchosenSize;
        private int _solutionSize;

        public SolutionDiff Current { get; set; }

        public void Initialize(HashSet<Item> unchosenItems, EvenKnapsack knapsack)
        {
            Current = null;
            _unchosenItems = unchosenItems.ToArray();
            _unchosenSize = unchosenItems.Count;
            _knapsack = knapsack;
            _solutionItems = _knapsack.Items.ToArray();
            _solutionSize = _knapsack.Items.Count();

            _uIter = 0;

            _solutionIter1 = 0;
            _solutionIter2 = _solutionIter1 + 1;
            _solutionIter3 = _solutionIter2 + 1;
        }

        /// <summary>
        /// worse case running time: number of iterations is: f(x) =x*(a-x)^3 (where a = number of total items, x = the number of items in knapsack (maximum of f(x) is (27^4)/256 for a=4x)
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            Item.ItemComparisonPolicy = new Item.CompareItemsByValue();

            //for each item in currentSolution.Items - try replace it with an unchosen item from unChosenItems
            while (_uIter < _unchosenSize)
            {
                var newItem = _unchosenItems[_uIter]; //item to be added to knapsack
                while (_solutionIter1 < _solutionSize - 2)
                {
                    var oldItem1 = _solutionItems[_solutionIter1]; //item to be removed from knapsack
                    while (_solutionIter2 < _solutionSize - 1)
                    {
                        var oldItem2 = _solutionItems[_solutionIter2]; //item to be removed from knapsack
                        while (_solutionIter3 < _solutionSize)
                        {
                            var oldItem3 = _solutionItems[_solutionIter3]; //item to be removed from knapsack
                            _solutionIter3++;
                            var candidate = new SolutionDiff(new List<Item> { oldItem1, oldItem2, oldItem3 }, new List<Item> { newItem });

                            if (!_knapsack.WouldBeLegalAfterChange(candidate))
                                continue; //if this candid will result in an illegal knapsack - skip this
                            Current = candidate;
                            return true;
                        }
                        _solutionIter2++;
                        _solutionIter3 = _solutionIter2 + 1;
                    }
                    _solutionIter1++;
                    _solutionIter2 = _solutionIter1 + 1;
                    _solutionIter3 = _solutionIter2 + 1;
                }
                _solutionIter1 = 0;
                _solutionIter2 = _solutionIter1 + 1;
                _solutionIter3 = _solutionIter2 + 1;

                _uIter++;
            }
            Current = null; //no more items -> update current to null
            return false;
        }
    }

    /// <summary>
    /// removes one item and add three
    /// </summary>
    public class OnePerThree : INeighborhoodGenerator
    {
        private IEnumerator<Item> _solutionIter;
        private EvenKnapsack _knapsack;
        private Item[] _unchosenItems;
        private int _uIter1;
        private int _uIter2;
        private int _uIter3;
        private int _unchosenSize;

        public SolutionDiff Current { get; set; }

        public void Initialize(HashSet<Item> unchosenItems, EvenKnapsack knapsack)
        {
            Current = null;
            _unchosenItems = unchosenItems.ToArray();
            _unchosenSize = unchosenItems.Count;
            _knapsack = knapsack;
            _solutionIter = _knapsack.Items.GetEnumerator();
            _solutionIter.MoveNext();

            _uIter1 = 0;
            _uIter2 = _uIter1 + 1;
            _uIter3 = _uIter2 + 1;
        }

        /// <summary>
        /// worse case running time: number of iterations is: f(x) =x*(a-x)^3 (where a = number of total items, x = the number of items in knapsack (maximum of f(x) is (27^4)/256 for a=4x)
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            Item.ItemComparisonPolicy = new Item.CompareItemsByValue();

            //for each item in currentSolution.Items - try replace it with an unchosen item from unChosenItems


            while (_uIter1 < _unchosenSize - 2)
            {
                var newItem1 = _unchosenItems[_uIter1]; //item to be added to knapsack
                while (_uIter2 < _unchosenSize - 1)
                {
                    var newItem2 = _unchosenItems[_uIter2]; //item to be added to knapsack
                    while (_uIter3 < _unchosenSize)
                    {
                        var newItem3 = _unchosenItems[_uIter3]; //item to be added to knapsack
                        while (_solutionIter.Current != null)
                        {
                            var oldItem = _solutionIter.Current; //item to be removed from knapsack
                            _solutionIter.MoveNext();

                            var candidate = new SolutionDiff(new List<Item> {oldItem},
                                new List<Item> {newItem1, newItem2, newItem3});

                            if (!_knapsack.WouldBeLegalAfterChange(candidate))
                                continue; //if this candid will result in an illegal knapsack - skipthis
                            Current = candidate;
                            return true;
                        }
                        _uIter3++;
                    }
                    _uIter2++;
                    _uIter3 = _uIter2 + 1;
                }
                _uIter1++;
                _uIter2 = _uIter1 + 1;
                _uIter3 = _uIter2 + 1;
            }
            _uIter1 = 0;
            _uIter2 = _uIter1 + 1;
            _uIter3 = _uIter2 + 1;

                
            
            Current = null; //no more items -> update current to null
            return false;
        }
    }

    public class TwoPerTow : INeighborhoodGenerator
    {
        private EvenKnapsack _knapsack;
        private int _unchosenSize;
        private Item[] _unchosenItems;
        private Item[] _solutionItems;
        private int _solutionSize;
        private int _uIter1;
        private int _uIter2;
        private int _solutionIter1;
        private int _solutionIter2;
        public SolutionDiff Current { get; set; }
        public bool MoveNext()
        {
            Item.ItemComparisonPolicy = new Item.CompareItemsByValue();

            //for each item in currentSolution.Items - try replace it with an unchosen item from unChosenItems
            while (_uIter1 < _unchosenSize-1)
            {
                var newItem1 = _unchosenItems[_uIter1]; //item to be added to knapsack
                while (_uIter2 < _unchosenSize)
                {
                    var newItem2 = _unchosenItems[_uIter2]; //item to be added to knapsack
                    while (_solutionIter1 < _solutionSize - 1)
                    {
                        var oldItem1 = _solutionItems[_solutionIter1]; //item to be removed from knapsack
                        while (_solutionIter2 < _solutionSize)
                        {
                            var oldItem2 = _solutionItems[_solutionIter2]; //item to be removed from knapsack
                            _solutionIter2++;
                            var candidate = new SolutionDiff(new List<Item> { oldItem1, oldItem2}, new List<Item> { newItem1, newItem2 });

                            if (!_knapsack.WouldBeLegalAfterChange(candidate))
                                continue; //if this candid will result in an illegal knapsack - skip this
                            Current = candidate;
                            return true;
                        }
                        _solutionIter1++;
                        _solutionIter2 = _solutionIter1 + 1;
                    }
                    _solutionIter1 = 0;
                    _solutionIter2 = _solutionIter1 + 1;
                    _uIter2++;
                }
                
                _uIter1++;
                _uIter2 = _uIter1 + 1;
            }
            Current = null; //no more items -> update current to null
            return false;



        }

        public void Initialize(HashSet<Item> unchosenItems, EvenKnapsack knapsack)
        {

            Current = null;
            _unchosenItems = unchosenItems.ToArray();
            _unchosenSize = unchosenItems.Count;
            _knapsack = knapsack;
            _solutionItems = _knapsack.Items.ToArray();
            _solutionSize = _knapsack.Items.Count();

            _uIter1 = 0;
            _uIter2 = _uIter1 + 1;

            _solutionIter1 = 0;
            _solutionIter2 = _solutionIter1 + 1;
            
        }
    }

}


