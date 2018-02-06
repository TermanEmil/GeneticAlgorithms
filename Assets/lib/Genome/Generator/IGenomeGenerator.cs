using GA.Gene;
using GA.Genome;

namespace GA.Genome.Generator
{
    public interface IGenomeGenerator<T>
    {
        IGenome<T> NewRandomGenome();
    }
}