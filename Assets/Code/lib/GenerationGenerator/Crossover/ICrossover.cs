using System.Collections.Generic;
using GA.Genome;

namespace GA.GenerationGenerator.Crossover
{
    public interface ICrossover<T>
    {
        IList<IGenome<T>> Crossover(IList<IGenome<T>> parents);
    }
}