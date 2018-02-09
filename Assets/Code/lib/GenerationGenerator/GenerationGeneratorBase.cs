using System.Collections.Generic;

using GA.Genome;
using GA.GenerationGenerator.Breeding;
using GA.GenerationGenerator.Reinsertion;

namespace GA.GenerationGenerator
{
    public class GenerationGeneratorBase<T> : IGenerationGenerator<T>
    {
        public int ToProduceCount { get; set; } = 0;
        public IList<IReinsertion<T>> Reinsertions { get; set; }
        public IList<IBreeding<T>> Breedings { get; set; }

        public GenerationGeneratorBase(
            int toProduceCount,
            IReinsertion<T> reinsertion,
            IBreeding<T> breeding)
        {
            ToProduceCount = toProduceCount;
            Reinsertions = new IReinsertion<T>[1] { reinsertion };
            Breedings = new IBreeding<T>[1] { breeding };
        }

        public GenerationGeneratorBase(
            int toProduceCount,
            IList<IReinsertion<T>> reinsertions,
            IList<IBreeding<T>> breedings)
        {
            ToProduceCount = toProduceCount;
            Reinsertions = reinsertions;
            Breedings = breedings;
        }

        public IList<IGenome<T>> Generate(IList<IGenome<T>> genomes)
        {
            List<IGenome<T>> result;

            result = new List<IGenome<T>>(genomes.Count);
            foreach (var reinsertion in Reinsertions)
                result.AddRange(reinsertion.GetGenomes(genomes));

            //foreach (var breeding in Breedings)
                //result.AddRange(breeding.Breed())

            return null;
        }
    }
}