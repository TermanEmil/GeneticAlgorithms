using GA.Gene;
using GA.Genome;
using GA.GenerationGenerator.GenomeProducer.Breeding.Mutation;
using System;

namespace GA_Tests.EvolveSum
{
    public class ES_Mutation : MutationBase<int>
    {
        public int MutationRange { get; set; }

        public ES_Mutation(Random random, double geneMutChance, int mutRange)
            : base(random, geneMutChance)
        {
            MutationRange = mutRange;
        }

        public override void MutateGene(Gene<int> targetGene)
        {
            targetGene.Val += RandomInst.Next(-MutationRange, MutationRange);
        }
    }
}