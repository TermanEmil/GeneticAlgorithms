using System.Collections.Generic;
using GA.Genome;

namespace GA.GenerationGenerator.GenomeProducer.Breeding.Selection
{
    /// <summary>
    /// Select genomes to breed.
    /// </summary>
    public interface ISelection<T>
    {
        /// <summary>
        /// Number of genomes to be selected as a result from `SelectNext'.
        /// </summary>
        int CountToSelect { get; set; }

        IList<IGenome<T>> AllGenomes { get; set; }

        void BeforeAllSelections(IList<IGenome<T>> allGenomes);
        IList<IGenome<T>> SelectNext();
    }
}