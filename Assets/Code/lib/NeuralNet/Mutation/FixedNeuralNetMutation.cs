using System;

using GA.Gene;
using GA.NeuralNet.SynapseStruct;
using GA.GenerationGenerator.GenomeProducer.Breeding.Mutation;

namespace GA.NeuralNet.Mutation
{
    public class FixedNeuralNetMutation : MutationBase<Synapse>
    {
        public double weightMutRange;

        public FixedNeuralNetMutation(
            Random random,
            double geneMutChance,
            double weightMutRange_) : base(random, geneMutChance)
        {
            weightMutRange = weightMutRange_;
        }

        public override void MutateGene(Gene<Synapse> targetGene)
        {
            var randomDeltaWeight = RandomInst.NextDouble()
                * 2 * weightMutRange - weightMutRange;

            targetGene.Val = new Synapse(
                targetGene.Val.transmitter,
                targetGene.Val.receiver,
                targetGene.Val.weight + randomDeltaWeight);
        }
    }
}