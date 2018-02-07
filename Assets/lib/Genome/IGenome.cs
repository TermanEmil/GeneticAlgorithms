using System.Collections.Generic;
using GA.Gene;

namespace GA.Genome
{
    public interface IGenome<T>
    {
        IList<Gene<T>> Genes { get; set; }
        double Fitness { get; set; }

        IGenome<T> CreateNew(bool copyGenes = true);
    }
}