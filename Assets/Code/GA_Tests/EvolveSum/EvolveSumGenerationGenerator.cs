using System.Collections.Generic;
using GA.Genome;
using GA.GenerationGenerator;
using GA.GenerationGenerator.Breeding.Crossover;
using GA.GenerationGenerator.Breeding.Selection;
using System.Linq;

namespace GA_Tests.EvolveSum
{
    /// <summary>
    /// The problem: Evolve an array of numbers of a given length, to make
    /// a sum equal to a given number.
    /// </summary>
    public class EvolveSumGenerationGenerator : IGenerationGenerator<int>
    {
        public System.Random RandomInst { get; set; }
        public double GenomesToKeep { get; set; } = 0.2d;

        public double GeneMutationChance = 0.1d;
        public int GeneMutationRange = 10;

        public EvolveSumGenerationGenerator(System.Random rand)
        {
            RandomInst = rand;
        }

        public IList<IGenome<int>> Generate(IList<IGenome<int>> prevGeneration)
        {
            IGenome<int> newGenome;
            ISelection<int> selector;
            ICrossover<int> crossover;
            EvolveSumMutation mutator;
            List<IGenome<int>> result = null;
            int genomesToKeep;
            int availableGenomes;

            selector = new RouletteWheelSelection<int>(RandomInst, 2);
            //selector = new EliteSelection<int>();

            crossover = new SinglePointCrossover<int>(RandomInst);
            mutator = new EvolveSumMutation(
                RandomInst, GeneMutationChance, GeneMutationRange);

            result = new List<IGenome<int>>(prevGeneration.Count());

            genomesToKeep = (int)(prevGeneration.Count() * GenomesToKeep);

            result.AddRange(prevGeneration.OrderByDescending(x => x.Fitness)
                                          .Take(genomesToKeep));
            
            availableGenomes = prevGeneration.Count() - genomesToKeep;
            selector.BeforeAllSelections(
                (int)(prevGeneration.Count() * 0.8f), prevGeneration);

            for (int i = 0; i < availableGenomes; i++)
            {
                var babies = crossover.Crossover(selector.SelectNext());

                if (RandomInst.NextDouble() <= 0.5f)
                    newGenome = babies.First();
                else
                    newGenome = babies.Last();

                mutator.Mutate(newGenome);
                result.Add(newGenome);
            }

            if (result.Count() != prevGeneration.Count())
            {
                throw new System.Exception(string.Format(
                    "New generation invalid size: {0}.", result.Count()));
            }
            
            return result;
        }
    }
}