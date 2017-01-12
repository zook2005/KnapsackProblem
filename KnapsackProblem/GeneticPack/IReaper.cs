using System.Linq;
using KnapsackProblem.Helpers;

namespace KnapsackProblem.GeneticPack
{
    public interface IReaper
    {
        bool EndOfEvolution(IPopulation currentGeneration);
    }

    public class GenerationCounterReaper : IReaper
    {
        private readonly int _maxNumberOfGenerations;

        public GenerationCounterReaper(int maxNumberOfGenerations)
        {
            _maxNumberOfGenerations = maxNumberOfGenerations;
        }

        public bool EndOfEvolution(IPopulation currentGeneration)
        {
            return _maxNumberOfGenerations < currentGeneration.GenerationCounter;
        }

        public override string ToString()
        {
            return $"max number of generations: {_maxNumberOfGenerations}";
        }
    }

    public class StandardDeviationReaper : IReaper
    {
        private readonly double _maxStdDev;

        public StandardDeviationReaper(double maxStdDev)
        {
            _maxStdDev = maxStdDev;
        }

        public bool EndOfEvolution(IPopulation currentGeneration)
        {
            return currentGeneration.Individuals.Select(i => (double) i.Fitness).CalculateStdDev() < _maxStdDev;
        }

        public override string ToString()
        {
            return $"max standard deviation: {_maxStdDev}";
        }
    }
}