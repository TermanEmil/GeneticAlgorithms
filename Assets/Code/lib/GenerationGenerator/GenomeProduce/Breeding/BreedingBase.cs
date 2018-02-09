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
            int n;

            n = Count(genomes);
            result = new List<IGenome<T>>(n);
            Selector.BeforeAllSelections(genomes);
            while (result.Count < n)
            {
                babies = Crossover.Crossover(Selector.SelectNext());
                if (babies.Count == 0)
                    ThrowInvalidChildProducedCount();

                foreach (var babie in babies)
                {
                    Mutator.Mutate(babie);
                    result.Add(babie);
                    if (result.Count() > n)
                        break;
                }
            }

            if (result.Count() > n)
                result = result.Take(n).ToArray();

            if (result.Count() != n)
                ThrowInvalidBreadCount(result.Count(), n);

            return result;
        }

        public int Count(IList<IGenome<T>> genomes)
        {
            return BabiesToMakeCount;
        }

        private void ThrowInvalidChildProducedCount()
        {
            throw new System.Exception("Crossover is required to " +
                                       "make at least one child.");
        }

        private void ThrowInvalidBreadCount(int resultCount, int required)
        {
            throw new System.Exception(string.Format("Invalid number of " +
                                                     "children bread: {0}/{1}",
                                                     resultCount, required));
        }
    }
}