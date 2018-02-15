using UnityEngine;

using GA.Population;
using GA.NeuralNet.SynapseStruct;
using GA.NeuralNet.NeuronClass;
using GA.NeuralNet.NeuralGenome;
using GA.NeuralNet.RandomGenomeGenerator;
using GA.NeuralNet.Activation;
using GA.NeuralNet.Mutation;
using GA.GenerationGenerator;
using GA.GenerationGenerator.GenomeProducer.Reinsertion;
using GA.GenerationGenerator.GenomeProducer.Breeding;
using GA.GenerationGenerator.GenomeProducer.Breeding.Selection;
using GA.GenerationGenerator.GenomeProducer.Breeding.Crossover;
using GA.GenerationGenerator.GenomeProducer.Breeding.Mutation;

namespace GA.UnityProxy.Genome
{
    public class GenomeProxy : MonoBehaviour
    {
        public INeuralGenome genome;

        public virtual void Init(INeuralGenome targetGenome)
        {
            genome = targetGenome;
        }
    }
}