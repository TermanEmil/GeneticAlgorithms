using System.Collections.Generic;
using GA.Genome;

namespace GA.GenerationGenerator.GenomeProducer
{
    /// <summary>
    /// An interface that generates new genomes from the existing ones.
    /// </summary>
    public interface IGenomeProducer<T>
    {
        IList<IGenome<T>> Generate(IList<IGenome<T>> genomes);

        /// <summary>
        /// Number of genomes that will be produced.
        /// </summary>
        /// <param name="genomes">Just a reference.</param>
        int Count(IList<IGenome<T>> genomes);
    }
}