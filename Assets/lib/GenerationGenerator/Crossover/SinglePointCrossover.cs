using System.Collections.Generic;
using GA.Genome;
using GA.Gene;
using GA.Genome.GeneticDiff;
using GA.Gene.GeneChoice;
using System;
using System.Linq;

namespace GA.GenerationGenerator.Crossover
{
    public class SinglePointCrossover<T> : CrossoverBase<T>
    {
        public int Pivot { get; set; } = -1;
        private Random RandomInst { get; set; }

        public SinglePointCrossover(Random random, int childrenToProduce = 1) : base(childrenToProduce)
        {
            if (ChildrenToProduce > 2)
            {
                throw new System.Exception(
                    "Single point crossover can't produce more that 2 children.");
            }
            RandomInst = random;
        }

        protected override IList<IGenome<T>> PerformCross(IList<IGenome<T>> parents)
        {
            IList<Gene<T>> genes1, genes2;
            int genesLen;

            if (parents.Count != 2)
                throw new Exception("Signle point crossover needs 2 parents only.");
            if (parents[0].Genes.Count() != parents[1].Genes.Count())
                throw new Exception("Single point crossover requires equal " +
                                    "length Genes.");

            genesLen = parents[0].Genes.Count();
            genes1 = parents[0].Genes.OrderBy(x => x.InnovNb).ToArray();
            genes2 = parents[1].Genes.OrderBy(x => x.InnovNb).ToArray();

            if (Pivot != -1)
                Pivot = RandomInst.Next(genesLen);

            return new IGenome<T>[2]
            {
                NewChild(parents[0].CreateNew(), NewChildGenes(genes1, genes2)),
                NewChild(parents[1].CreateNew(), NewChildGenes(genes2, genes1))
            };
        }

        private IList<Gene<T>> NewChildGenes(IList<Gene<T>> genes1, IList<Gene<T>> genes2)
        {
            IEnumerable<Gene<T>> result;

            result = genes1.Take(Pivot)
                           .Concat(genes2.ToList().GetRange(Pivot + 1, genes1.Count()))
                           .Select(x => new Gene<T>(x));
            if (result.Count() != genes1.Count())
                throw new Exception("Invalid length of resulting genes.");
            return result.ToArray();
        }

        private IGenome<T> NewChild(IGenome<T> childInst, IList<Gene<T>> genes)
        {
            childInst.Genes = genes;
            return childInst;
        }
    }
}