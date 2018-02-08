using System.Collections.Generic;
using GA.Genome;

namespace GA.GenerationGenerator.Selection
{
    /// <summary>
    /// Select genomes to breed.
    /// </summary>
    public interface ISelection<T>
    {
        int TotalRequiredNb { get; set; }
        IList<IGenome<T>> AllGenomes { get; set; }

        void BeforeAllSelections(int requiredNb, IList<IGenome<T>> allGenomes);
        void BeforeSelection(int iter);
        IGenome<T> SelectNext();
    }
}