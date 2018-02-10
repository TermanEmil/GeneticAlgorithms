using System;

using GA.Genome.RandGenerator;
using GA.Gene;
using GA.Genome;
using System.Linq;

namespace GA_Tests.Shakespeare
{
    public class Sh_RandGenomeGenerator : IRandGenomeGenerator<char>
    {
        public Random RandomInst { get; set; }
        public int GenesCount { get; set; }
        public int RandInterval { get; set; }
        public string Chars { get; set; }

        public Sh_RandGenomeGenerator(
            Random random,
            int genesCount,
            int randInterval,
            string chars)
        {
            RandomInst = random;
            GenesCount = genesCount;
            RandInterval = randInterval;
            Chars = chars;
        }

        public IGenome<char> NewRandomGenome()
        {
            var genes = Enumerable.Range(0, GenesCount)
                                  .Select(x => NewRandomGene(x))
                                  .ToArray();
            return new GenomeBase<char>(genes);
        }

        private Gene<char> NewRandomGene(int i)
        {
            Gene<char> rs;
            
            rs = new Gene<char>(i)
            {
                Val = Chars[RandomInst.Next(Chars.Length)]
            };
            return rs;
        }
    }
}