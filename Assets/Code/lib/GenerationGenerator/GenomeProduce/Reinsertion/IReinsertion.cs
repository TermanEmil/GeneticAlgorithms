using System.Collections.Generic;
using GA.Genome;

namespace GA.GenerationGenerator.GenomeProducer.Reinsertion
{
    /// <summary>
    /// Reinsertion is a stage when some genomes from the previous generation
    /// are chosen to be copied for the next generation. In some cases it may
    /// be desireable to also copy and mutate some genomes here.
    /// Anyway, this stage is invoked before selection-crossover, so the number
    /// of resulting 'copies' must be considered.
    /// </summary>
    public interface IReinsertion<T> : IGenomeProducer<T>
    {
    }
}