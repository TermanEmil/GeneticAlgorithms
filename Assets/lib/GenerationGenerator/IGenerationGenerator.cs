using System.Collections.Generic;
using GA.Genome;

namespace GA.GenerationGenerator
{
    public interface IGenerationGenerator<T>
    {
        IList<IGenome<T>> Generate(IList<IGenome<T>> prevGeneration);
    }
}