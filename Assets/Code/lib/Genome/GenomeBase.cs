using System.Collections.Generic;
using System.Linq;
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

        public IGenome<T> CreateNew(bool copyGenes = true)
        {
            IList<Gene<T>> genes;

            if (copyGenes)
                genes = Genes.Select(x => new Gene<T>(x)).ToArray();
            else
                genes = Genes;
            return new GenomeBase<T>(genes);
        }

        public override string ToString()
        {
            var rs = "{" + Fitness.ToString("#.0") + ": ";

            foreach (var gene in Genes)
                rs += gene + " ";
            rs += "}";
            return rs;
        }
    }
}