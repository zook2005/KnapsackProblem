using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KnapsackProblem.Helpers;
using KnapsackProblem.Knapsack;

namespace KnapsackProblem.LocalSearchPack
{
    public static class Main
    {
        public static void EntryPoint(string reportPath, List<Item> items, uint size, string inputFilePath, double fractional)
        {
            LocalSearch[] localSearchers = ChooseLocalSearchers();
            LocalSearch bestLocalSearcher = null;
            var bestRes = new EvenKnapsack(0);

            var tryNumber = 1;
            MyWriter sw = Program.Sw;
                foreach (LocalSearch localSearch in localSearchers)
                {
                    sw.WriteLine($"@@@@@@@ try #{tryNumber}: Now using local search:\n\t{localSearch}");
                                        tryNumber++;
                    for (var cycle = 1; cycle <= localSearch.TimesToRun; cycle++)
                    {
                        sw.WriteLine($"\tCycle: {cycle} out of: {localSearch.TimesToRun}");
                        
                        //create another copy of items
                        var itemsCopy = items.ToList();
                        EvenKnapsack res = localSearch.Algorithm(itemsCopy, size);
                        if (res.TotalValue > bestRes.TotalValue)
                        {
                            bestLocalSearcher = localSearch;
                            bestRes = res;
                        }

                        sw.WriteLine($"\tres: {res.TotalValue}");
                        sw.WriteLine();
                    }
                }
            CreateReport(reportPath, inputFilePath, bestLocalSearcher, bestRes, fractional);
        }

        private static LocalSearch[] ChooseLocalSearchers()
        {
            return new LocalSearch[]
            {
                new LocalSearch(new ShuffleSortingPolicy(), new InitializeEmpty(), new List<INeighborhoodGenerator> {new AddTwo(), new OnePerOne(),new ThreePerOne(), new TwoPerTow(), new OnePerThree()},  new ChooseFirst(), 2, false, true),

                new LocalSearch(new SortWithComparer(new Item.CompareItemsByDensity()), new InitializeGreedily(), new List<INeighborhoodGenerator> {new AddTwo(), new OnePerOne(),new ThreePerOne(), new TwoPerTow(), new OnePerThree()},  new ChooseFirst(), 1, false, true)    
            };
        }

        public static void CreateReport(string outputFilePath, string inputFilePath, LocalSearch bestRes, EvenKnapsack evenKnapsack, double fractional)
        {
            var sw = Program.Sw;
                sw.WriteLine();
                sw.WriteLine("------------Local Search Summary------------");
                                // this is the best solution the Local search has found
                sw.WriteLine($"best result is: {evenKnapsack.ToString()}");
                                //this is the optimal solution for the fractional problem
                sw.WriteLine("Fractional greedy returned: " + fractional);
                sw.WriteLine($"bag's Max Capacity: {evenKnapsack.MaxCapacity}");


                sw.WriteLine($"Local search used: \n\t {bestRes?.ToString() ?? ""}");
                                if (!String.IsNullOrWhiteSpace(inputFilePath))
                {
                    sw.WriteLine("input file name: " + Path.GetFileName(inputFilePath));
                                    }
                ////if (!string.IsNullOrWhiteSpace(outputFilePath)) sw.WriteLine("output file name: " + Path.GetFileName(outputFilePath));
                sw.WriteLine();
                sw.WriteLine("###################################");
                sw.WriteLine("Items that were chosen:");
                //                //                //                //
                foreach (var item in evenKnapsack.Items)
                {
                    sw.WriteLine(item.ToString());
                                    }
                sw.WriteLine("**************************************");
                sw.WriteLine("Items that were NOT chosen:");
                foreach (var item in bestRes.UnchosenItems)
                {
                    sw.WriteLine(item.ToString());
                                    
                }

            }
        }
    }

