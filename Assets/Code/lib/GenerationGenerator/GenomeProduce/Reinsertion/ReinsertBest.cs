using System.Collections.Generic;
using System.Linq;
using GA.Genome;
using System;

namespace GA.GenerationGenerator.GenomeProducer.Reinsertion
{
    public class ReinsertBest<T> : IReinsertion<T>
    {
        public int GenomesToGetCount { get; set; } = 0;

        public ReinsertBest(int n)
        {
            GenomesToGetCount = n;
        }

        public IList<IGenome<T>> Generate(IList<IGenome<T>> genomes)
        {
            int n;

            n = Count(genomes);
            if (n > genomes.Count())
                ThrowInvalidNb(n, genomes.Count());
            
            return genomes.OrderByDescending(x => x.Fitness)
                          .Take(n).Select(x => x.CreateNew(true)).ToArray();
        }

        public int Count(IList<IGenome<T>> genomes)
        {
            return GenomesToGetCount;
        }

        private void ThrowInvalidNb(int n, int maxNb)
        {
            throw new Exception(
                string.Format("Reinsertion: Invalid number: {0} / {1}",
                              n, maxNb));
        }
    }
}