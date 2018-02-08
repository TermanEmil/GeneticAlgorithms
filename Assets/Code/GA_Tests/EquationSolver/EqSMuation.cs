using System;
using GA.Gene;
using GA.GenerationGenerator.Mutation;

namespace GA_Tests.EquationSolver
{
    public class EqSMuation : MutationBase<int>
    {
        public int MutationRange;

        public EqSMuation(
            Random random,
            double geneMutChance,
            int mutRange) : base(random, geneMutChance)
        {
            MutationRange = mutRange;
        }

        public override void MutateGene(Gene<int> targetGene)
        {
            
        }
    }
}