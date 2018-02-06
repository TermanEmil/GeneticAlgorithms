using System.Collections.Generic;
using GA.Gene;

namespace GA.Genome
{
    public interface IGenome<T>
    {
        IList<Gene<T>> Genes { get; set; }
    }
}