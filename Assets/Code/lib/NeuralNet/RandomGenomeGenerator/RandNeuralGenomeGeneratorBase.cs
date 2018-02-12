using System.Collections.Generic;
using System.Linq;
using System;

using GA.Gene;
using GA.Genome;
using GA.NeuralNet.Activation;
using GA.NeuralNet.NeuronClass;
using GA.NeuralNet.NeuralGenome;
using GA.NeuralNet.SynapseStruct;

namespace GA.NeuralNet.RandomGenomeGenerator
{
    public class RandNeuralGenomeGeneratorBase :
        IRandNeuralGenomeGenerator
    {
        public Random RandomInst { get; set; }
        public IActivation Activator { get; set; }
        public double WeightRange { get; set; }
        public int InputCount { get; set; }
        public int OutputCount { get; set; }
        public int[] HiddenLayers { get; set; }
        public bool CreateBias { get; set; }

        public RandNeuralGenomeGeneratorBase(
            Random random_,
            IActivation activator,
            double weightRange,
            int inputCount,
            int outputCount,
            int[] hiddenLayers,
            bool createBias)
        {
            RandomInst = random_;
            Activator = activator;
            WeightRange = weightRange;
            InputCount = inputCount;
            OutputCount = outputCount;
            HiddenLayers = hiddenLayers;
            CreateBias = createBias;
        }

        public IGenome<Synapse> NewRandomGenome()
        {
            List<Neuron> allNeurons;

            var layers = CreateGenomeNeuralLayers(out allNeurons);

            var genes = new List<Gene<Synapse>>();
            for (int i = 0; i < layers.Count() - 1; i++)
            {
                genes.AddRange(ConnectLayers(
                    layers[i],
                    layers[i + 1]));
            }

            if (CreateBias)
            {
                var bias = new Neuron(
                    layers.Last().Last().innovNb + 1,
                    ENeurType.bias);
                for (int i = 1; i < layers.Count(); i++)
                {
                    genes.AddRange(ConnectLayers(
                        new Neuron[] { bias },
                        layers[i]));
                }
                allNeurons.Add(bias);
            }

            AssignGenesInnovNb(genes);
            return new NeuralGenomeBase(allNeurons, genes, Activator);
        }

        /// <summary>
        /// Creates the neural net's layers.
        /// </summary>
        protected IList<Neuron[]> CreateGenomeNeuralLayers(
            out List<Neuron> allNeuronss)
        {
            List<int> layerCount;

            layerCount = new List<int>() { InputCount };
            if (HiddenLayers != null)
                layerCount.AddRange(HiddenLayers);
            layerCount.Add(OutputCount);

            var layers = CreateLayers(
                0,
                layerCount,
                ENeurType.hidden,
                out allNeuronss);

            foreach (var neuron in layers[0])
                neuron.neurType = ENeurType.input;
            foreach (var neuron in layers.Last())
                neuron.neurType = ENeurType.output;

            return layers;
        }

        protected Neuron[] NewNeurons(
            int startingInnovNb,
            int n,
            ENeurType neurType)
        {
            return Enumerable.Range(0, n)
                             .Select(x =>
                                     new Neuron(x + startingInnovNb, neurType))
                             .ToArray();
        }

        protected IList<Neuron[]> CreateLayers(
            int startingInnovNb,
            IList<int> layersNeuronCount,
            ENeurType neurType,
            out List<Neuron> allNeurons)
        {
            List<Neuron[]> layers = new List<Neuron[]>();
            allNeurons = new List<Neuron>();

            if (layersNeuronCount == null)
                return layers;
            foreach (var layerCount in layersNeuronCount)
            {
                var neurons = NewNeurons(
                    startingInnovNb,
                    layerCount,
                    neurType);

                allNeurons.AddRange(neurons);
                startingInnovNb = neurons.Last().innovNb + 1;
                layers.Add(neurons);
            }
            return layers;
        }

        protected List<Gene<Synapse>> ConnectLayers(
            Neuron[] layer1,
            Neuron[] layer2)
        {
            var genes = new List<Gene<Synapse>>();

            for (int i = 0; i < layer1.Count(); i++)
            {
                for (int j = 0; j < layer2.Count(); j++)
                    genes.Add(NewSynapseGene(layer1[i], layer2[j]));
            }
            return genes;
        }

        protected Gene<Synapse> NewSynapseGene(
            Neuron transmiter,
            Neuron receiver)
        {
            var newSynapse = new Synapse(
                transmiter.innovNb,
                receiver.innovNb,
                RandomWeight());
            var newGene = new Gene<Synapse>() { Val = newSynapse };
            return newGene;
        }

        protected double RandomWeight()
        {
            return RandomInst.NextDouble() * 2 * WeightRange -
                         WeightRange;
        }

        protected void AssignGenesInnovNb(IList<Gene<Synapse>> genes)
        {
            int innovNb = 0;

            foreach (var gene in genes)
            {
                gene.InnovNb = innovNb;
                innovNb++;
            }
        }
    }
}