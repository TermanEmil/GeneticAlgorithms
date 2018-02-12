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

namespace GA_Tests.XOR
{
    public class XOR : MonoBehaviour
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
        public double fitenssPower = 2;

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
                2,
                1,
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

        public void ComputeUIButtonRequest()
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

        private IBreeding<Synapse> NewBreeding(System.Random random)
        {
            //var selector = new EliteSelection<Synapse>(2);
            var selector = new RouletteWheelSelection<Synapse>(random, 2);
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

        private double ComputeFitness(INeuralGenome genome)
        {
            //Debug.Log(genome);
            double error = 0;

            genome.Fitness = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    var output = genome.FeedNetwork(new double[2] { i, j })[0];
                    //Debug.Log(string.Format("[{0} {1}]: {2:0.00}", i, j, output));
                    error += Math.Abs((i ^ j) - output);
                }
            }

            genome.Fitness = Math.Pow(4 - error + 1, fitenssPower);
            return error;
        }

        private bool MaxReached()
        {
            foreach (var genome in population.Genomes)
                ComputeFitness(genome as INeuralGenome);

            var best = population.Genomes.OrderByDescending(x => x.Fitness)
                                 .First() as INeuralGenome;

            return ComputeFitness(best) <= desiredError;
        }

        private void ComputeGenerations()
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
    }
}