using System.Linq;
using GA.Genome;
using System;
using GA.Fitness;

namespace GA_Tests.EvolveSum
{
    public class ES_FitnessEval : IFitnessEval<int>
    {
        public int TargetSum { get; set; }

        public ES_FitnessEval(int targetSum)
        {
            TargetSum = targetSum;
        }

        public double Evaluate(IGenome<int> genome)
        {
            return TargetSum - Math.Abs(TargetSum - genome.Genes.Sum(x => x.Val));
        }
    }
}