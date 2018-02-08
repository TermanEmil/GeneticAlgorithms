using System.Linq;
using GA.Genome;
using System;

namespace GA.Fitness
{
    public class EvolveSumFitnessEval : IFitnessEval<int>
    {
        public int TargetSum { get; set; }

        public EvolveSumFitnessEval(int targetSum)
        {
            TargetSum = targetSum;
        }

        public double Evaluate(IGenome<int> genome)
        {
            return TargetSum - Math.Abs(TargetSum - genome.Genes.Sum(x => x.Val));
        }
    }
}