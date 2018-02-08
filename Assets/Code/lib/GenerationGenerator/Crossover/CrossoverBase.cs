using System.Collections.Generic;
using GA.Genome;

namespace GA.GenerationGenerator.Crossover
{
    public abstract class CrossoverBase<T> : ICrossover<T>
    {
        public IList<IGenome<T>> Crossover(IList<IGenome<T>> parents)
        {
            return PerformCross(parents);
        }

        protected abstract IList<IGenome<T>> PerformCross(IList<IGenome<T>> parents);
    }
}