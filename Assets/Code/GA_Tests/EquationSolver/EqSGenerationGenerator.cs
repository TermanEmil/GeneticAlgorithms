using System.Collections.Generic;
using GA.Genome;
using GA.GenerationGenerator;
using GA.GenerationGenerator.GenomeProducer.Breeding.Crossover;
using GA.GenerationGenerator.GenomeProducer.Breeding.Selection;
using GA.GenerationGenerator.GenomeProducer.Breeding.Mutation;
using System.Linq;

namespace GA_Tests.EquationSolver
{
    public class EqSGenerationGenerator : IGenerationGenerator<int>
    {
        public ISelection<int> Selector { get; set; }
        public ICrossover<int> Crossover { get; set; }
        public IMutation<int> Mutator { get; set; }

        public EqSGenerationGenerator(
            ISelection<int> selector,
            ICrossover<int> crossover,
            IMutation<int> mutator)
        {
            Selector = selector;
            Crossover = crossover;
            Mutator = mutator;
        }

        public IList<IGenome<int>> Generate(
            IList<IGenome<int>> prevGeneration)
        {
            return null;
        }
    }
}