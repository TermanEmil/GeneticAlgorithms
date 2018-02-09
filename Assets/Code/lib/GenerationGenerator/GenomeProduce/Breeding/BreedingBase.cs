using System.Collections.Generic;
using GA.Genome;
using GA.GenerationGenerator.GenomeProducer.Breeding.Selection;
using GA.GenerationGenerator.GenomeProducer.Breeding.Crossover;
using GA.GenerationGenerator.GenomeProducer.Breeding.Mutation;
using System.Linq;

namespace GA.GenerationGenerator.GenomeProducer.Breeding
{
    public class BreedingBase<T> : IBreeding<T>
    {
        public int BabiesToMakeCount { get; set; } = 0;
        public ISelection<T> Selector { get; set; }
        public ICrossover<T> Crossover { get; set; }
        public IMutation<T> Mutator { get; set; }

        public BreedingBase(
            int babiesToMake,
            ISelection<T> selector,
            ICrossover<T> crossover,
            IMutation<T> mutator)
        {
            BabiesToMakeCount = babiesToMake;

            Selector = selector;
            Crossover = crossover;
            Mutator = mutator;
        }

        public IList<IGenome<T>> Generate(IList<IGenome<T>> genomes)
        {
            IList<IGenome<T>> result;
            IList<IGenome<T>> babies;

            result = new List<IGenome<T>>(BabiesToMakeCount);
            Selector.BeforeAllSelections(BabiesToMakeCount, genomes);
            while (result.Count < BabiesToMakeCount)
            {
                babies = Crossover.Crossover(Selector.SelectNext());
                if (babies.Count == 0)
                {
                    throw new System.Exception("Crossover is required to " +
                                               "make at least one child.");
                }

                foreach (var babie in babies)
                {
                    Mutator.Mutate(babie);
                    result.Add(babie);
                    if (result.Count() > BabiesToMakeCount)
                        break;
                }
            }

            if (result.Count() > BabiesToMakeCount)
                result = result.Take(BabiesToMakeCount).ToArray();

            if (result.Count() != BabiesToMakeCount)
                throw new System.Exception("Invalid number of children bread.");

            return result;
        }

        public int Count(IList<IGenome<T>> genomes)
        {
            return 0;
        }
    }
}