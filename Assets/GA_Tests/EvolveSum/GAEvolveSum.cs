using GA.Population;
using System;

namespace GA_Tests.EvolveSum
{
    public class GAEvolveSum
    {
        public int TargetSum { get; private set; }
        public int GenomeNumbers { get; private set; }
        public PopulationBase<int> Population { get; private set; }

        private Random RandomInst { get; set; }

        public GAEvolveSum(int targetSum, int poplLen, int genomeNumbers, int randomInterval)
        {
            EvolveSumGenomeGenerator genomeGenerator;

            TargetSum = targetSum;
            GenomeNumbers = genomeNumbers;

            RandomInst = new Random(Seed: 0);
            genomeGenerator = new EvolveSumGenomeGenerator(genomeNumbers, randomInterval, RandomInst);
            //Population = new PopulationBase<int>(poplLen, genomeGenerator);
        }

        public void Start()
        {
            
        }
    }
}