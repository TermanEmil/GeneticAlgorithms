using System;
using GA.NeuralNet.Activation;
using GA.NeuralNet.SynapseStruct;
using GA.Genome.RandGenerator;

namespace GA.NeuralNet.RandomGenomeGenerator
{
    public interface IRandNeuralGenomeGenerator : IRandGenomeGenerator<Synapse>
    {
        Random RandomInst { get; set; }
        IActivation Activator { get; set; }
        double WeightRange { get; set; }
        int InputCount { get; set; }
        int OutputCount { get; set; }
        int[] HiddenLayers { get; set; }
        bool CreateBias { get; set; }
    }
}