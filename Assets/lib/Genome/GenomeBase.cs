using System.Collections.Generic;
using GA.Gene;

namespace GA.Genome
{
    public class GenomeBase<T> : IGenome<T>
    {
        public IList<Gene<T>> Genes { get; set; }
        public double Fitness { get; set; }

        public GenomeBase(IList<Gene<T>> genes)
        {
            Genes = genes;
        }
    }
}