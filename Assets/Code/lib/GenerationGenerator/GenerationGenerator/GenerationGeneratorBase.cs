using System.Collections.Generic;

using GA.Genome;
using GA.GenerationGenerator.Selection;
using GA.GenerationGenerator.Crossover;
using GA.GenerationGenerator.Mutation;

namespace GA.GenerationGenerator
{
    public class GenerationGeneratorBase<T> : IGenerationGenerator<T>
    {
        public ISelection<T> Selector { get; set; }
        public ICrossover<T> Crossover { get; set; }
        public IMutation<T> Mutator { get; set; }

        protected GenerationGeneratorBase(
            ISelection<T> selector,
            ICrossover<T> crossover,
            IMutation<T> mutator)
        {
            Selector = selector;
            Crossover = crossover;
            Mutator = mutator;
        }

        public IList<IGenome<T>> Generate(IList<IGenome<T>> prevGeneration)
        {
            List<IGenome<T>> result;

            result = new List<IGenome<T>>(prevGeneration.Count);
            return null;
        }
    }
}