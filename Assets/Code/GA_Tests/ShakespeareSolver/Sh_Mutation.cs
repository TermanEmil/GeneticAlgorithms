using GA.Gene;
using GA.Genome;
using GA.GenerationGenerator.GenomeProducer.Breeding.Mutation;
using System;

namespace GA_Tests.Shakespeare
{
    public class Sh_Mutation : MutationBase<char>
    {
        public int MutationRange { get; set; }
        public string Chars { get; set; }

        public Sh_Mutation(
            Random random,
            double geneMutChance,
            int mutRange,
            string chars)
            : base(random, geneMutChance)
        {
            MutationRange = mutRange;
            Chars = chars;
        }

        public override void MutateGene(Gene<char> targetGene)
        {
            int newIndex;

            newIndex = Chars.IndexOf(targetGene.Val);
            newIndex += RandomInst.Next(-MutationRange, MutationRange);
            if (newIndex < 0)
                newIndex += Chars.Length;
            else if (newIndex >= Chars.Length)
                newIndex -= Chars.Length;
            targetGene.Val = Chars[newIndex];
        }
    }
}