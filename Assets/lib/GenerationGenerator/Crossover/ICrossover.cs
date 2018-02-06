using System.Collections.Generic;
using GA.Genome;

namespace GA.GenerationGenerator.Crossover
{
    public interface ICrossover<T>
    {
        int ChildrenToProduce { get; }
        IEnumerable<IGenome<T>> Crossover(IList<IGenome<T>> parents);
    }
}