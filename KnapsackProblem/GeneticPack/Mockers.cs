using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnapsackProblem.Knapsack;

namespace KnapsackProblem.GeneticPack
{
    class MockGod : IGod
    {
        public Tuple<IIndividual, IIndividual> CrossOver(IPopulation population)
        {
            var individuals = population.Individuals;
            return new Tuple<IIndividual, IIndividual>(individuals[0], individuals[1]);
        }

        public IIndividual Mutate(IPopulation population)
        {
            return population.Individuals[0];
        }

        public double PercentageOfMutation { get; set; } = 5;
    }

    class MockReaper : IReaper
    {
        private int _counter = 100;
        public bool EndOfEvolution(IPopulation currentGeneration)
        {
            return --_counter <= 0;
        }
    }

    class MockIndividualInitializer : IIndividualInitializer
    {
        public IIndividual GetInstance(List<Item> items, uint knapsackMaxCapacity)
        {
            return new Individual(items, knapsackMaxCapacity);
        }
    }
}
