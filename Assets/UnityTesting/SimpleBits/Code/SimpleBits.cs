using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;

using GA.Population;
using GA.NeuralNet.RandomGenomeGenerator;
using GA.NeuralNet.SynapseStruct;
using GA.NeuralNet.NeuronClass;
using GA.NeuralNet.NeuralGenome;
using GA.NeuralNet.Activation;
using GA.NeuralNet.Mutation;
using GA.GenerationGenerator;
using GA.GenerationGenerator.GenomeProducer.Reinsertion;
using GA.GenerationGenerator.GenomeProducer.Breeding;
using GA.GenerationGenerator.GenomeProducer.Breeding.Selection;
using GA.GenerationGenerator.GenomeProducer.Breeding.Crossover;
using GA.GenerationGenerator.GenomeProducer.Breeding.Mutation;
using System.Collections;

namespace GA_Tests.SimpleBits
{
    public abstract class SimpleBits : MonoBehaviour
    {
        public IPopulation<Synapse> population;

        [Header("Other config")]
        public bool autostart = true;
        public double desiredError = 0.1;
        public int maxGenerations = 100;

        [Header("Algorithm config")]
        public int poplLne = 50;
        public double reinsertPart = 0.1;
        public double geneMutChance = 0.1;
        public double weightRange = 1;
        public double mutRange = 0.1;
        public bool seedRandom = false;
        public int[] hiddenLayers = new int[1] { 1 };
        public bool createBias = true;
        public float fitenssPower = 2;
        public int inputCount = 2;
        public int outputCount = 1;
        public bool roulteWheelSelection = false;

        [Header("Canvas")]
        public InputField inputField1;
        public InputField inputField2;
        public Text textField;

        private void Start()
        {
            if (autostart)
            {
                InitGA();
                ComputeGenerations();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                InitGA();
                ComputeGenerations();
            }
        }

        public void InitGA()
        {
            System.Random rand;

            rand = (seedRandom) ? new System.Random(1) : new System.Random();
            var randGenomGenerator = new RandNeuralGenomeGeneratorBase(
                rand,
                new Sigmoid(),
                weightRange,
                inputCount,
                outputCount,
                hiddenLayers,
                createBias);

            var reinsertion = new ReinsertBest<Synapse>(
                (int)(poplLne * reinsertPart));
            var breeding = NewBreeding(rand);
            var generationGenerator = new GenerationGeneratorBase<Synapse>(
                poplLne,
                reinsertion,
                breeding);

            population = new PopulationBase<Synapse>(
                poplLne,
                randGenomGenerator,
                generationGenerator);
        }

        public virtual void ComputeUIButtonRequest()
        {
            foreach (var genome in population.Genomes)
                ComputeFitness(genome as INeuralGenome);
            
            int x1 = int.Parse(inputField1.text);
            int x2 = int.Parse(inputField2.text);
            var bestGenome = population.Genomes
                                       .OrderByDescending(x => x.Fitness)
                                       .First() as INeuralGenome;
            var output = bestGenome.FeedNetwork(new double[2] { x1, x2 })[0];
            textField.text = string.Format("{0:0.00}", output);
        }

        public INeuralGenome GetBestGenome()
        {
            foreach (var genome in population.Genomes)
                ComputeFitness(genome as INeuralGenome);
            return population.Genomes
                             .OrderByDescending(x => x.Fitness)
                             .First() as INeuralGenome;
                             
        }

        protected abstract double ComputeFitness(INeuralGenome genome);

        protected IBreeding<Synapse> NewBreeding(System.Random random)
        {
            ISelection<Synapse> selector;

            if (roulteWheelSelection)
                selector = new RouletteWheelSelection<Synapse>(random, 2);
            else
                selector = new EliteSelection<Synapse>(2);

            var crossover = new SinglePointCrossover<Synapse>(random);
            var mutator = new FixedNeuralNetMutation(
                random,
                geneMutChance,
                mutRange);
            return new BreedingBase<Synapse>(
                poplLne - (int)(poplLne * reinsertPart),
                selector,
                crossover,
                mutator);
        }

        protected bool MaxReached()
        {
            foreach (var genome in population.Genomes)
                ComputeFitness(genome as INeuralGenome);

            var best = population.Genomes.OrderByDescending(x => x.Fitness)
                                 .First() as INeuralGenome;

            return ComputeFitness(best) <= desiredError;
        }

        protected void ComputeGenerations()
        {
            for (int i = 0; i < maxGenerations; i++)
            {
                foreach (var genome in population.Genomes)
                    ComputeFitness(genome as INeuralGenome);
                
                var maxFitness = population.Genomes.Max(x => x.Fitness);
                Debug.Log(population.Generation + ": " + maxFitness.ToString());

                if (MaxReached())
                    break;

                population.Evolve();
            }
        }

        protected double[] ToBits(int nb, int maxNb = 8)
        {
            var length = Mathf.RoundToInt(Mathf.Log(maxNb, 2));
            var bits = new BitArray(new int[] { nb });
            double[] result = new double[length];
            for (int i = 0; i < length; i++)
                result[i] = (bits[i]) ? 1 : 0;
            return result;
        }

        protected float GetNetworkOutputAsNb(INeuralGenome genome, int inNb)
        {
            var outputs = genome.FeedNetwork(ToBits(inNb));
            double result = 0;

            for (int i = 0; i < outputs.Count(); i++)
                result += outputs[i] * Mathf.Pow(2, i);
            return (float)result;
        }
    }
}