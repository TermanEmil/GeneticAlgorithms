using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics;

using GA.Population;
using GA.Gene;
using GA.Genome;
using GA.NeuralNet.SynapseStruct;
using GA.NeuralNet.NeuronClass;
using GA.NeuralNet.NeuralGenome;
using GA.NeuralNet.Activation;
using GA.GenerationGenerator;
using GA.Genome.RandGenerator;
using GA.GenerationGenerator.GenomeProducer.Reinsertion;
using GA.GenerationGenerator.GenomeProducer.Breeding;
using GA.GenerationGenerator.GenomeProducer.Breeding.Selection;
using GA.GenerationGenerator.GenomeProducer.Breeding.Crossover;
using GA.GenerationGenerator.GenomeProducer.Breeding.Mutation;

namespace GA_Tests.PathFinder
{
    public class PthF_RandGenomeGenerator : IRandGenomeGenerator<Synapse>
    {
        public Random RandomInst { get; set; }
        public double WeightInitRange { get; set; }

        public int InputCount { get; set; }
        public int OutputCount { get; set; }
        public int HiddenLayers { get; set; }
        public int HiddenLayerNeuronsCount { get; set; }

        public PthF_RandGenomeGenerator(
            Random random,
            double weightInitRange,
            int inputCount,
            int outputCount,
            int hiddenLayers,
            int hiddenLayerNeuronsCount)
        {
            RandomInst = random;
            WeightInitRange = weightInitRange;

            InputCount = inputCount;
            OutputCount = outputCount;
            HiddenLayers = hiddenLayers;
            HiddenLayerNeuronsCount = hiddenLayerNeuronsCount;
        }

        public IGenome<Synapse> NewRandomGenome()
        {
            int innovNb = 0;

            var inputNeurons = NewNeurons(
                innovNb,
                InputCount,
                ENeurType.input);
            innovNb = inputNeurons.Last().innovNb + 1;

            var outputNeurons = NewNeurons(
                innovNb,
                OutputCount,
                ENeurType.output);
            innovNb = outputNeurons.Last().innovNb + 1;

            var hiddenLayers = new List<Neuron[]>(HiddenLayers);
            for (int i = 0; i < HiddenLayers; i++)
            {
                var newLayer = NewNeurons(
                    innovNb,
                    HiddenLayerNeuronsCount,
                    ENeurType.hidden);
                innovNb = newLayer.Last().innovNb + 1;
                hiddenLayers.Add(newLayer);
            }

            var biasNeuron = new Neuron(innovNb, ENeurType.bias, 1);

            var neuralLayers = new List<Neuron[]>
            {
                inputNeurons
            };
            neuralLayers.AddRange(hiddenLayers);
            neuralLayers.Add(outputNeurons);

            var genes = new List<Gene<Synapse>>();

            for (int i = 0; i < neuralLayers.Count() - 1; i++)
            {
                genes.AddRange(ConnectLayers(
                    neuralLayers[i],
                    neuralLayers[i + 1]));
            }

            for (int i = 1; i < neuralLayers.Count(); i++)
                genes.AddRange(ConnectLayers(
                    new Neuron[] { biasNeuron },
                    neuralLayers[i]));

            innovNb = 0;
            foreach (var gene in genes)
            {
                gene.InnovNb = innovNb;
                innovNb++;
            }
            //return new NeuralGenomeBase(genes, new Sigmoid());
            return null;
        }

        private List<Gene<Synapse>> ConnectLayers(
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

        private Neuron[] NewNeurons(
            int startingInnovNb,
            int n,
            ENeurType neurType)
        {
            return Enumerable.Range(0, n)
                             .Select(x =>
                                     new Neuron(x + startingInnovNb, neurType))
                             .ToArray();
        }

        private Gene<Synapse> NewSynapseGene(
            Neuron transmiter,
            Neuron receiver)
        {
            var newSynapse = new Synapse(
                transmiter.innovNb,
                receiver.innovNb,
                RandomWeight());
            
            var newGene = new Gene<Synapse>();
            newGene.Val = newSynapse;
            return newGene;
        }

        private double RandomWeight()
        {
            return RandomInst.NextDouble() * 2 * WeightInitRange -
                             WeightInitRange;
        }
    }
}