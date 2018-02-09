using System.Collections.Generic;

using GA.Genome;
using GA.GenerationGenerator.GenomeProducer;
using GA.GenerationGenerator.GenomeProducer.Reinsertion;
using GA.GenerationGenerator.GenomeProducer.Breeding;
using System.Linq;

namespace GA.GenerationGenerator
{
    public class GenerationGeneratorBase<T> : IGenerationGenerator<T>
    {
        public int ToProduceCount { get; set; } = 0;
        public IList<IGenomeProducer<T>> GenomeProducers { get; set; }

        public GenerationGeneratorBase(
            int toProduceCount,
            IReinsertion<T> reinsertion,
            IBreeding<T> breeding)
        {
            ToProduceCount = toProduceCount;
            GenomeProducers = new IGenomeProducer<T>[2]
            {
                reinsertion,
                breeding
            };
        }

        public GenerationGeneratorBase(
            int toProduceCount,
            IList<IGenomeProducer<T>> producers)
        {
            ToProduceCount = toProduceCount;
            GenomeProducers = producers;
        }

        public IList<IGenome<T>> Generate(IList<IGenome<T>> genomes)
        {
            List<IGenome<T>> result;

            result = new List<IGenome<T>>(genomes.Count);
            foreach (var producer in GenomeProducers)
                result.AddRange(producer.Generate(genomes));
            
            return result;
        }
    }
}