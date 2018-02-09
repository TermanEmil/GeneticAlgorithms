using System.Collections.Generic;
using GA.Genome;
using GA.GenerationGenerator.GenomeProducer.Breeding.Selection;
using GA.GenerationGenerator.GenomeProducer.Breeding.Crossover;
using GA.GenerationGenerator.GenomeProducer.Breeding.Mutation;

namespace GA.GenerationGenerator.GenomeProducer.Breeding
{
    public interface IBreeding<T> : IGenomeProducer<T>
    {
        int BabiesToMakeCount { get; set; }
        ISelection<T> Selector { get; set; }
        ICrossover<T> Crossover { get; set; }
        IMutation<T> Mutator { get; set; }
    }
}