using System.Collections.Generic;
using GA.Genome;
using GA.Gene;
using System;
using System.Linq;

namespace GA.GenerationGenerator.GenomeProducer.Breeding.Crossover
{
    public class SinglePointCrossover<T> : CrossoverBase<T>
    {
        public int Pivot { get; set; } = -1;
        public int MaxBabiesToMake { get; set; } = 1;
        private Random RandomInst { get; set; }

        public SinglePointCrossover(Random random, int maxBabies = 1)
        {
            RandomInst = random;
            MaxBabiesToMake = maxBabies;
        }

        protected override IList<IGenome<T>> SelectBabies(
            IList<IGenome<T>> babies)
        {
            if (MaxBabiesToMake >= babies.Count())
                return babies;
            else
                return babies.Take(MaxBabiesToMake).ToArray();
        }

        protected override IList<IGenome<T>> PerformCross(
            IList<IGenome<T>> parents)
        {
            IList<Gene<T>> genes1, genes2;
            int genesLen;

            if (parents.Count != 2)
                ThrowInvalidParentCount(parents.Count());
            if (parents[0].Genes.Count() != parents[1].Genes.Count())
                ThrowInvalidGeneLen(parents[0], parents[1]);

            genesLen = parents[0].Genes.Count();
            genes1 = parents[0].Genes.OrderBy(x => x.InnovNb).ToArray();
            genes2 = parents[1].Genes.OrderBy(x => x.InnovNb).ToArray();

            if (Pivot == -1)
                Pivot = RandomInst.Next(genesLen);
            
            return new IGenome<T>[2]
            {
                NewChild(parents[0].CreateNew(), NewChildGenes(genes1, genes2)),
                NewChild(parents[1].CreateNew(), NewChildGenes(genes2, genes1))
            };
        }

        private IList<Gene<T>> NewChildGenes(
            IList<Gene<T>> genes1,
            IList<Gene<T>> genes2)
        {
            IEnumerable<Gene<T>> result;

            result = genes1.Take(Pivot);
            result = result.Concat(genes2.ToList()
                                   .GetRange(Pivot, genes2.Count() - Pivot))
                           .Select(x => new Gene<T>(x));

            if (result.Count() != genes1.Count())
                ThrowInvalidResultGenesLen(result.Count());
            
            return result.ToArray();
        }

        private IGenome<T> NewChild(IGenome<T> childInst, IList<Gene<T>> genes)
        {
            childInst.Genes = genes;
            return childInst;
        }

        private void ThrowInvalidParentCount(int parentCount)
        {
            throw new Exception(string.Format("2 parents are required: {0}/2",
                                              parentCount));
        }

        private void ThrowInvalidGeneLen(IGenome<T> parent1, IGenome<T> parent2)
        {
            throw new Exception(string.Format("Single point crossover " +
                                              "requires equal length " +
                                              "Genes: {0} != {1}",
                                              parent1.Genes.Count(),
                                              parent2.Genes.Count()));   
        }

        private void ThrowInvalidResultGenesLen(int resultCount)
        {
            throw new Exception("Invalid length of resulting genes: "
                                + resultCount);
        }
    }
}
