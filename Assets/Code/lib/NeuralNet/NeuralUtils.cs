using System.Collections.Generic;
using System.Linq;

using GA.Gene;
using GA.NeuralNet.SynapseStruct;
using GA.NeuralNet.NeuralGenome;
using GA.NeuralNet.NeuronClass;

namespace GA.NeuralNet
{
    public static class NeuralUtils
    {
        public static IList<Neuron> GetNeurons(
            INeuralGenome genome,
            ENeurType neurType)
        {
            return genome.NeuronLst
                         .Where(x => x.neurType == neurType)
                         .ToArray();
        }

        public static Neuron GetBias(INeuralGenome genome)
        {
            return genome.NeuronLst
                         .FirstOrDefault(x => x.neurType == ENeurType.bias);
        }

        public static Gene<Synapse> FindGene(
            IList<Gene<Synapse>> genes,
            Synapse synapse)
        {
            return genes.First(x => x.Val.Equals(synapse));
        }
    }
}