using GA.Genome;

namespace GA.GenerationGenerator.Mutation
{
    public interface IMutation<T>
    {
        void Mutate(IGenome<T> target, double probability);
    }
}