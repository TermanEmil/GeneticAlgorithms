using System.Collections.Generic;
using GA.Genome;
using GA.GenerationGenerator;
using GA.GenerationGenerator.Crossover;
using GA.GenerationGenerator.Selection;
using GA.GenerationGenerator.Mutation;
using System.Linq;

namespace GA_Tests.EvolveSum
{
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

            selector = new RouletteWheelSelection<int>(RandomInst);
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
                selector.BeforeSelection(i);
                IGenome<int>[] tmpParents =
                {
                    selector.SelectNext(),
                    selector.SelectNext()
                };
                var babies = crossover.Crossover(tmpParents);

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