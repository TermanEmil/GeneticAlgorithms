using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

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

namespace GA_Tests.PathFinder
{
    public class PathFinderCtrl : MonoBehaviour
    {
        public bool seedRandom = false;
        public int poplLen = 50;
        public IPopulation<Synapse> population;
        public double partToReinsert = 0.1d;

        [Header("Mutation")]
        public double geneMutChance = 0.1d;
        public double weightMutRange = 1d;

        [Header("Fitness")]
        public float fitnessExpGradeint = 2;
        public float fitnessMultInArea = 3;

        [Header("Nerual net")]
        public double randomWeight = 1;
        public int inputCount = 6;
        public int outputCount = 2;
        public int[] hiddenLayers = null;
        public bool createBias = true;

        [Header("Unity agents config")]
        public Transform spawnPoint;
        public Transform agentsBuf;
        public Transform target;
        public GameObject agentPrefab;
        public float lifeSpan = 10f;
        private float generationStartTime;
        public List<AgentCtrl> agents = new List<AgentCtrl>();

        [Header("Dynamic output")]
        public double currentBestFitness;

        private System.Random rand;

        private void Start()
        {
            Debug.Assert(spawnPoint != null);
            Debug.Assert(agentsBuf != null);
            Debug.Assert(target != null);
            Debug.Assert(agentsBuf != null);

            //rand = (seedRandom) ? new System.Random(1) : new System.Random();
            rand = new System.Random(1);
            //InitAll();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                InitAll();

            if (!agents.Any())
                return;

            if (agents.FirstOrDefault(x => x.gameObject.activeSelf) == null ||
                generationStartTime + lifeSpan < Time.time)
            {
                NextGeneration();
                generationStartTime = Time.time;
            }
        }

        public void InitAll()
        {
            InitGAInternal();
            InitAgents();
            generationStartTime = Time.time;
        }

        public void InitAgents()
        {
            if (agents.Count() != 0)
            {
                foreach (var agent in agents)
                    Destroy(agent.gameObject);
            }
            agents.Clear();

            for (int i = 0; i < poplLen; i++)
            {
                var newAgent = Instantiate(agentPrefab, agentsBuf)
                    .GetComponent<AgentCtrl>();
                newAgent.transform.rotation = spawnPoint.rotation;
                newAgent.transform.position = spawnPoint.position;
                newAgent.Init(population.Genomes[i] as INeuralGenome, target);
                agents.Add(newAgent);
            }
        }

        public void InitGAInternal()
        {
            var randomGenomeGenerator = new RandNeuralGenomeGeneratorBase(
                rand,
                new Sigmoid(),
                randomWeight,
                inputCount,
                outputCount,
                hiddenLayers,
                createBias);

            var reinsertion = new ReinsertBest<Synapse>(
                (int)(poplLen * partToReinsert));
            var breeding = NewBreeding(rand);
            var generationGenerator = new GenerationGeneratorBase<Synapse>(
                poplLen,
                reinsertion,
                breeding);

            population = new PopulationBase<Synapse>(
                poplLen,
                randomGenomeGenerator,
                generationGenerator);
        }

        private IBreeding<Synapse> NewBreeding(System.Random random)
        {
            var selector = new EliteSelection<Synapse>(2);
            //var selector = new RouletteWheelSelection<Synapse>(random, 2);

            var crossover = new SinglePointCrossover<Synapse>(random);
            var mutator = new FixedNeuralNetMutation(
                random,
                geneMutChance,
                weightMutRange);
            
            return new BreedingBase<Synapse>(
                poplLen - (int)(partToReinsert * poplLen),
                selector,
                crossover,
                mutator);
        }

        private void NextGeneration()
        {
            if (!agents.Any())
                return;
            
            currentBestFitness = population.Genomes
                                           .OrderByDescending(x => x.Fitness)
                                           .First().Fitness;
            population.Evolve();
            InitAgents();
        }
    }
}