using GA.Gene;
using GA.Genome;

namespace GA.Genome.RandGenerator
{
    public interface IRandGenomeGenerator<T>
    {
        IGenome<T> NewRandomGenome();
    }
}