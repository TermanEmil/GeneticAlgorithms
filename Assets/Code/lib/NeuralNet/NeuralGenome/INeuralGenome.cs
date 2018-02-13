using System.Collections.Generic;

using GA.Gene;
using GA.Genome;
using GA.NeuralNet.SynapseStruct;
using GA.NeuralNet.NeuronClass;

namespace GA.NeuralNet.NeuralGenome
{
    public interface INeuralGenome : IGenome<Synapse>
    {
        IList<Neuron> NeuronLst { get; set; }
        IDictionary<int, Neuron> Neurons { get; }
        IDictionary<Neuron, IList<Synapse>> Network { get; }
        IList<double> FeedNetwork(double[] inputs);
    }
}