using System;
using System.Collections.Generic;
using System.Linq;
using KnapsackProblem.Knapsack;
using Microsoft.VisualStudio.GraphModel.Schemas;

namespace KnapsackProblem.GeneticPack
{
    public static class Main
    {      
        public static void EntryPoint(string reportPath, List<Item> items, uint size, string inputFilePath,
            double fractional)
        {
            var genetics = ChooseGenetics();
            Genetic bestGenetic = null;
            IKnapsack bestKnapsack = new EvenKnapsack(0);
            var sw = Program.Sw;
            var tryNumber = 1;

            foreach (var genetic in genetics)
            {
                sw.WriteLine($"@@@@@@@ Now using Genetic");
                //sw.WriteLine($"@@@@@@@ try #{tryNumber}:");
                tryNumber++;
                for (var cycle = 1; cycle <= genetic.TimesToRun; cycle++)
                {
                    //sw.WriteLine($"\tCycle: {cycle} out of: {bnB.TimesToRun}");

                    //create another copy of items
                    var itemsCopy = items.ToList();
                    var startingTime = DateTime.Now;
                    IKnapsack currentKnapsack = genetic.Algorithm(itemsCopy, size);
                    var endingTime = DateTime.Now;
                    sw.WriteLine($"running time: {endingTime.Subtract(startingTime)}");
                    if (currentKnapsack.TotalValue > bestKnapsack.TotalValue)
                    {
                        bestGenetic = genetic;
                        bestKnapsack = currentKnapsack;
                    }

                    //sw.WriteLine($"\best res: {res}");
                    sw.WriteLine();
                }
            
                CreateReport(reportPath, inputFilePath, bestGenetic, bestKnapsack, fractional);
            }
        }

        private static IEnumerable<Genetic> ChooseGenetics()
        {
            return new[]
            {
                //new Genetic(new MockReaper(), new God(2), new MockIndividualInitializer()),
                new Genetic(new GenerationCounterReaper(250), new BitGod(2,new BitMutator(false,true)), new BitIndividualInitializer(), 100),
                //new Genetic(new GenerationCounterReaper(100), new God(2), new IndividualInitializer()),
                
            };
        }

        public static void CreateReport(string outputFilePath, string inputFilePath, Genetic bestRes, IKnapsack evenKnapsack, double fractional)
        {
            var sw = Program.Sw;
            sw.WriteLine("------------Genetic Summary------------");
            sw.WriteLine($"fractional = {fractional}");   
            sw.WriteLine($"solution = {evenKnapsack.ToString()}"); 
            sw.WriteLine($" numberOfGenerations = {bestRes. GenerationsCounter}");
            sw.WriteLine($" Population size = {bestRes.PoplulationSize}");
            sw.WriteLine($"bag's Max Capacity: {evenKnapsack.MaxCapacity}");
            sw.WriteLine($"*Genetic* \n{bestRes.ToString()}");
            sw.WriteLine($"stopping condition determined by: {bestRes.Reaper.ToString()}");


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
            unchosenItems.UnionWith(bestRes.Items.ToList());
            unchosenItems.ExceptWith(evenKnapsack.Items);
            foreach (var item in unchosenItems)
            {
                sw.WriteLine(item.ToString());
            }
            sw.WriteLine();
        }
    }
}
