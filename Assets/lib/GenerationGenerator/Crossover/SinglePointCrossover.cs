using System.Collections.Generic;
using GA.Genome;

namespace GA.GenerationGenerator.Crossover
{
    public class SinglePointCrossover<T> : CrossoverBase<T>
    {
        public SinglePointCrossover(int childrenToProduce = 1) : base(childrenToProduce)
        {
            if (ChildrenToProduce > 2)
            {
                throw new System.Exception(
                    "Single point crossover can't produce more that 2 children.");
            }
        }

        public override IEnumerable<IGenome<T>> Crossover(IList<IGenome<T>> parents)
        {
            if (parents.Count != 2)
                throw new System.Exception("Signle point crossover needs 2 parents only.");


            return null;
        }
    }
}