using GA.Genome;

namespace GA.GenerationGenerator.GenomeProducer.Breeding.Mutation
{
    public interface IMutation<T>
    {
        void Mutate(IGenome<T> target);
    }
}