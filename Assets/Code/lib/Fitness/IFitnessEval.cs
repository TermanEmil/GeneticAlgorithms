using GA.Genome;

namespace GA.Fitness
{
    public interface IFitnessEval<T>
    {
        double Evaluate(IGenome<T> target);
    }
}