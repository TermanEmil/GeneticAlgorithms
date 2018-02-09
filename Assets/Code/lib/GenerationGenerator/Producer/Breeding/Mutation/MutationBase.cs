using GA.Gene;
using GA.Genome;
using System;

namespace GA.GenerationGenerator.Breeding.Mutation
{
    public abstract class MutationBase<T> : IMutation<T>
    {
        public Random RandomInst { get; set; }
        public double GeneMutationChance { get; set; }

        protected MutationBase(Random random, double geneMutChance)
        {
            RandomInst = random;
            GeneMutationChance = geneMutChance;
        }

        public void Mutate(IGenome<T> target)
        {
            foreach (var gene in target.Genes)
                if (RandomInst.NextDouble() <= GeneMutationChance)
                    MutateGene(gene);
        }

        public abstract void MutateGene(Gene<T> targetGene);
    }
}