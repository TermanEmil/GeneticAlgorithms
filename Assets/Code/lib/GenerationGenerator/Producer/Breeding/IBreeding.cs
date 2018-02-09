using System.Collections.Generic;
using GA.Genome;
using GA.GenerationGenerator.Breeding.Selection;
using GA.GenerationGenerator.Breeding.Crossover;
using GA.GenerationGenerator.Breeding.Mutation;

namespace GA.GenerationGenerator.Breeding
{
    public interface IBreeding<T>
    {
        ISelection<T> Selector { get; set; }
        ICrossover<T> Crossover { get; set; }
        IMutation<T> Mutator { get; set; }

        IList<IGenome<T>> Breed(
            int babiesToMakeCount,
            IList<IGenome<T>> genomes);
    }
}