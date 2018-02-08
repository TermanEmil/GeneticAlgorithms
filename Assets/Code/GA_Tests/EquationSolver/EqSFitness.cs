using GA.Fitness;
using GA.Genome;

namespace GA_Tests.EquationSolver
{
    public class EqSFitness : IFitnessEval<int>
    {
        public double Evaluate(IGenome<int> genome)
        {
            return 0d;
        }
    }
}