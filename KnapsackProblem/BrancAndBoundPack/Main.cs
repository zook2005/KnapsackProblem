using System;
using System.Collections.Generic;
using System.Linq;
using KnapsackProblem.Knapsack;

namespace KnapsackProblem.BrancAndBoundPack
{
    public static class Main
    {
        public static void EntryPoint(string reportPath, List<Item> items, uint size, string inputFilePath,
            double fractional)
        {
            IEnumerable<BnB> bnBs = ChooseBnBs();
            BnB bestBnB = null;
            var bestRes = new EvenKnapsack(0);
            var sw = Program.Sw;
            var tryNumber = 1;

                foreach (var bnB in bnBs)
                {
                    sw.WriteLine($"@@@@@@@ Now using BnB");
                    //sw.WriteLine($"@@@@@@@ try #{tryNumber}:");
                    tryNumber++;
                    for (var cycle = 1; cycle <= bnB.TimesToRun; cycle++)
                    {
                        //sw.WriteLine($"\tCycle: {cycle} out of: {bnB.TimesToRun}");

                        //create another copy of items
                        var itemsCopy = items.ToList();
                        var startingTime = DateTime.Now;
                        EvenKnapsack res = bnB.Algorithm(itemsCopy, size);
                        var endingTime = DateTime.Now;
                        sw.WriteLine($"running time: {endingTime.Subtract(startingTime)}");
                        if (res.TotalValue > bestRes.TotalValue)
                        {
                            bestBnB = bnB;
                            bestRes = res;
                        }

                        //sw.WriteLine($"\tres: {res}");
                        sw.WriteLine();
                    }
                
                CreateReport(reportPath, inputFilePath, bestBnB, bestRes, fractional);
            }
        }

        private static IEnumerable<BnB> ChooseBnBs()
        {
            return new[]
            {
                new BnB(new SortWithComparer(new Item.CompareItemsByDensity(),true)),
                new BnB(new SortWithComparer(new Item.CompareItemsBySize(),true)),
                new BnB(new ShuffleSortingPolicy()),
            };
        }

        public static void CreateReport(string outputFilePath, string inputFilePath, BnB bestBnB, EvenKnapsack evenKnapsack, double fractional)
        {
            var sw = Program.Sw;
            sw.WriteLine("------------BnB Summary------------");
            sw.WriteLine($"fractional = {fractional}");   
            sw.WriteLine($"solution = {evenKnapsack.ToString()}"); 
            sw.WriteLine();
            sw.WriteLine("###################################");
            sw.WriteLine("Items that were chosen:");

            foreach (var item in evenKnapsack.Items)
            {
                sw.WriteLine(item.ToString());
            }
            sw.WriteLine("**************************************");
            sw.WriteLine("Items that were NOT chosen:");
            
            //remove elements from items (UnchosenItems = items - evenKnapsack.Items) 
            HashSet<Item> unchosenItems = new HashSet<Item>();
            unchosenItems.UnionWith(bestBnB.Items.ToList());
            unchosenItems.ExceptWith(evenKnapsack.Items);
            foreach (var item in unchosenItems)
            {
                sw.WriteLine(item.ToString());
            }
            sw.WriteLine();
        }
    }
}
