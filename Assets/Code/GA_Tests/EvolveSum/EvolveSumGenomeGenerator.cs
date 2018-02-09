using System;
using System.Linq;
using GA.Gene;
using GA.Genome;
using GA.Genome.RandGenerator;

namespace GA_Tests.EvolveSum
{
    public class EvolveSumGenomeGenerator : IRandGenomeGenerator<int>
    {
        private int NumbLen { get; set; }
        private int RandomInterval { get; set; }
        private Random RandomInst { get; set; }

        public EvolveSumGenomeGenerator(int numbLen, int randomInterval, Random random)
        {
            NumbLen = numbLen;
            RandomInterval = randomInterval;
            RandomInst = random;
        }

        public IGenome<int> NewRandomGenome()
        {
            Gene<int>[] genes;

            genes = Enumerable.Range(0, NumbLen)
                              .Select(x => NewRandomGene(x))
                              .ToArray();
            return new GenomeBase<int>(genes);
        }

        private Gene<int> NewRandomGene(int i)
        {
            Gene<int> result;

            result = new Gene<int>(i);
            result.Val = RandomInst.Next(-RandomInterval, RandomInterval);
            return result;
        }
    }
}