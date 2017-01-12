using System;
using System.Collections.Generic;
using System.Linq;
using KnapsackProblem.Helpers;
using KnapsackProblem.Knapsack;

namespace KnapsackProblem.GeneticPack
{
    public interface IPopulation
    {
        List<IIndividual> Individuals { get; }
        IIndividual Alpha { get; }
        IPopulation GenerateNextGeneration();
        IGod God { get; set; }
        uint SumOfAllFitness { get; }
        int GenerationCounter { get; }
        IIndividualInitializer IndividualInitializer { get; }
    }

    public class Population : IPopulation
    {
        public Population(IEnumerable<Item> items, uint halfOfPoplulationSize, uint knapsackMaxCapacity, IGod god, IIndividualInitializer individualInitializer)
        {
            God = god;
            IndividualInitializer = individualInitializer;
            Initialize(items.ToList(), halfOfPoplulationSize, knapsackMaxCapacity);
        }

        private void Initialize(List<Item> items, uint halfOfPoplulationSize, uint knapsackMaxCapacity)
        {
            SumOfAllFitness = 0;
            int n = (int)(halfOfPoplulationSize * 2);
            Individuals = new List<IIndividual>(n);
            for (var i = 0; i < n; i++)
            {
                items.Shuffle();
                IIndividual individual = IndividualInitializer.GetInstance(items, knapsackMaxCapacity);
                Individuals.Add(individual);
                SumOfAllFitness += individual.Fitness;
            }
        }

        public IIndividualInitializer IndividualInitializer { get; set; }

        public Population(List<IIndividual> individuals, IGod god)
        {
            Individuals = individuals;
            God = god;
            SumOfAllFitness = (uint) individuals.Sum(ind => ind.Fitness);
        }

        public List<IIndividual> Individuals { get; private set; }
        public IGod God { get; set; }

        public uint SumOfAllFitness { get; private set; }

        public int GenerationCounter { get; private set; }

        public IIndividual Alpha => Individuals?.Max();

        public IPopulation GenerateNextGeneration()
        {
            var sizeOfPopulation = Individuals.Count;
            var half = sizeOfPopulation/2;

            List<IIndividual> newPopulation = new List<IIndividual>(sizeOfPopulation);

            var timesToMutate = Convert.ToInt32(Math.Ceiling(half * God.PercentageOfMutation / 100));
            //timesToMutate = timesToMutate == 0 ? 1 : timesToMutate;
            
            for (int i= 0; i< timesToMutate; i++)
            {
                newPopulation.Add(God.Mutate(this));
                newPopulation.Add(God.Mutate(this));
            }

            var timesToXo = half - timesToMutate;
            for (int i = 0; i < timesToXo; i++)
            {
                var tuple = God.CrossOver(this);
                newPopulation.Add(tuple.Item1);
                newPopulation.Add(tuple.Item2);
            }
            var nextCounter =
                this.GenerationCounter + 1;
            return new Population(newPopulation, God) {GenerationCounter = nextCounter };
        }
    }
}