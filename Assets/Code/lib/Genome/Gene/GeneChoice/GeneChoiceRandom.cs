using System;

namespace GA.Gene.GeneChoice
{
    public class GeneChoiceRandom<T> : IGeneChoice<T>
    {
        public Random RandomInst { get; set; }

        public GeneChoiceRandom(Random random)
        {
            RandomInst = random;
        }

        public Gene<T> Choice(Gene<T> gene1, Gene<T> gene2)
        {
            if (RandomInst.NextDouble() < 0.5f)
                return gene1;
            else
                return gene2;
        }
    }
}