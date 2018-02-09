using System.Collections.Generic;
using GA.Genome;
using GA.GenerationGenerator.Breeding.Selection;
using GA.GenerationGenerator.Breeding.Crossover;
using GA.GenerationGenerator.Breeding.Mutation;
using System.Linq;

namespace GA.GenerationGenerator.Breeding
{
    public class BreedingBase<T> : IBreeding<T>
    {
        public ISelection<T> Selector { get; set; }
        public ICrossover<T> Crossover { get; set; }
        public IMutation<T> Mutator { get; set; }

        public BreedingBase(
            ISelection<T> selector,
            ICrossover<T> crossover,
            IMutation<T> mutator)
        {
            Selector = selector;
            Crossover = crossover;
            Mutator = mutator;
        }

        public IList<IGenome<T>> Breed(
            int babiesToMakeCount,
            IList<IGenome<T>> genomes)
        {
            IList<IGenome<T>> result;
            IList<IGenome<T>> babies;

            result = new List<IGenome<T>>(babiesToMakeCount);
            Selector.BeforeAllSelections(babiesToMakeCount, genomes);
            while (result.Count < babiesToMakeCount)
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
                    if (result.Count() > babiesToMakeCount)
                        break;
                }
            }

            if (result.Count() > babiesToMakeCount)
                result = result.Take(babiesToMakeCount).ToArray();

            if (result.Count() != babiesToMakeCount)
                throw new System.Exception("Invalid number of children bread.");

            return result;
        }
    }
}