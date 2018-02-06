using System.Collections.Generic;
using System.Linq;
using GA.Genome;
using GA.Gene;
using GA.Gene.GeneChoice;

namespace GA.Genome.GeneticDiff
{
    public struct GeneticDiff<T>
    {
        public IList<Gene<T>> Matching { get; set; }
        public IList<Gene<T>> Disjoint { get; set; }
        public IList<Gene<T>> Excess { get; set; }

        public GeneticDiff(IEnumerable<Gene<T>> genes1, IEnumerable<Gene<T>> genes2, IGeneChoice<T> geneChoice)
        {
            int maxInvoNb;
            IEnumerable<int> commonInnovNbs;
            IEnumerable<Gene<T>> allGenes;

            commonInnovNbs = genes1.Select(x => x.InnovNb)
                                   .Intersect(genes2.Select(x => x.InnovNb));
            Matching = commonInnovNbs.Select(x =>
            {
                return geneChoice.Choice(genes1.First(y => y.InnovNb == x),
                                         genes2.First(y => y.InnovNb == x));
            }).ToArray();

            allGenes = genes1.Concat(genes2);
            maxInvoNb = allGenes.Max(x => x.InnovNb);

            Disjoint = allGenes.Where(x => !commonInnovNbs.Contains(x.InnovNb) && x.InnovNb <= maxInvoNb)
                               .ToArray();
            Excess = allGenes.Where(x => !commonInnovNbs.Contains(x.InnovNb) && x.InnovNb > maxInvoNb)
                             .ToArray();
        }
    }
}