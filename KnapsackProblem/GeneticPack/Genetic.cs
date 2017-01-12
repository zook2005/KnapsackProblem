using System;
using System.Collections.Generic;
using System.Linq;
using KnapsackProblem.Helpers;
using KnapsackProblem.Knapsack;

namespace KnapsackProblem.GeneticPack
{
    public class Genetic
    {
        public int TimesToRun { get; set; }
        public List<Item> Items { get; set; }
        public IIndividual Alpha { get; set; }
        public IReaper Reaper { get; set; }
        public IGod God { get; set; }
        public IIndividualInitializer IndividualInitializer { get; set; }
        public uint HalfOfPoplulationSize { get; set; }
        public uint PoplulationSize => HalfOfPoplulationSize*2;

        public Genetic(IReaper reaper, IGod god, IIndividualInitializer individualInitializer, uint halfOfPoplulationSize, int timesToRun = 1)
        {
            Reaper = reaper;
            God = god;
            TimesToRun = timesToRun;
            IndividualInitializer = individualInitializer;
            HalfOfPoplulationSize = halfOfPoplulationSize;
        }

        /// <summary>
        /// step 1: initialize a population in the size of 2N.
        /// step 2: calculate the fitness of each individual - done in population constructor
        /// step 3: save the best solution.
        /// step 4: decide whether or not the genetic algorithm should keep running
        /// step 5: create next generation
        /// </summary>
        /// <param name="items"></param>
        /// <param name="bagMaxCapacity"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public IKnapsack Algorithm(List<Item> items, uint bagMaxCapacity)
        {
            Sw = Program.Sw;
            Items = items;

            //steps 1 + 2
            Population population = new Population(items, HalfOfPoplulationSize, bagMaxCapacity, God, IndividualInitializer);

            Alpha = population.Alpha;

            Evolve(population);
            foreach (var individual in population.Individuals)
            {
                Sw.WriteLine(string.Format("{0} {1}", string.Join(" ", individual.Chromosome.Select(kvp => kvp.Value ? 1 : 0)), individual.Fitness));
            }

            return Alpha.Knapsack;
        }

        /// <summary>
        /// recursively generates the next generation, till the Reaper decides to end evolution
        /// </summary>
        /// <param name="population"></param>
        private void Evolve(IPopulation population)
        {
           //count generations
            GenerationsCounter = population.GenerationCounter;

           //step 3 
            var newIsBigger = population.Alpha.CompareTo(Alpha) > 0;
            Alpha = !newIsBigger  ? Alpha : population.Alpha;

            //printings
            //Sw.WriteLine();
            //Sw.WriteLine("-----------");
            //if (newIsBigger) Sw.WriteLine("**Alpha has changed**");
            //Sw.WriteLine($"Generation #{GenerationsCounter}:");
            //Sw.WriteLine($"max solution = {population.Alpha.ToString()}");
            //Sw.WriteLine($"Average Fitness = {(double)population.SumOfAllFitness/population.Individuals.Count}");
            //Sw.WriteLine($"min solution = {population.Individuals.Min().ToString()}");
            //Sw.WriteLine($"Standard daviation: {population.Individuals.Select(i=>(double)i.Fitness).CalculateStdDev()}");
            //Sw.WriteLine("-----------");

            //step 4
            if (Reaper.EndOfEvolution(population)) { return; }

            //step 5
            population = population.GenerateNextGeneration();

            //return to step 2
            Evolve(population);
        }

        public int GenerationsCounter { get; private set; }

        public MyWriter Sw { get; set; }

        public override string ToString()
        {
            return $"Percentage Of Mutation: {God.PercentageOfMutation}%{Environment.NewLine}" +
                   $"number of generations: {GenerationsCounter}";
        }
    }
}