using GA.Genome;

namespace GA.GenerationGenerator.Breeding.Mutation
{
    public interface IMutation<T>
    {
        void Mutate(IGenome<T> target);
    }
}