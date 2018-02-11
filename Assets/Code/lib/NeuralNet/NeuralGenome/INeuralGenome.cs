using System.Collections.Generic;

using GA.Gene;
using GA.Genome;
using GA.NeuralNet.SynapseStruct;
using GA.NeuralNet.NeuronClass;

namespace GA.NeuralNet.NeuralGenome
{
    public interface INeuralGenome : IGenome<Synapse>
    {
        IList<double> FeedNetwork(IDictionary<int, double> inputs);
    }
}