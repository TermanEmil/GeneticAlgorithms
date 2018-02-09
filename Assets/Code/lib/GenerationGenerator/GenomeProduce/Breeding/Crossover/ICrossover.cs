using System.Collections.Generic;
using GA.Genome;

namespace GA.GenerationGenerator.GenomeProducer.Breeding.Crossover
{
    public interface ICrossover<T>
    {
        IList<IGenome<T>> Crossover(IList<IGenome<T>> parents);
    }
}