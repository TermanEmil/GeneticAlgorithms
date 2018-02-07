using System.Collections.Generic;
using GA.Genome;

namespace GA.GenerationGenerator.Crossover
{
    public abstract class CrossoverBase<T> : ICrossover<T>
    {
        public int ChildrenToProduce { get; private set; }

        public CrossoverBase(int childrenToProduce = 1)
        {
            if (childrenToProduce <= 0)
                throw new System.Exception("Crossover can't produce no children.");
            
            ChildrenToProduce = childrenToProduce;
        }

        public IList<IGenome<T>> Crossover(IList<IGenome<T>> parents)
        {
            return PerformCross(parents);
        }

        protected abstract IList<IGenome<T>> PerformCross(IList<IGenome<T>> parents);
    }
}