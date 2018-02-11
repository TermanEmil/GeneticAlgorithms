using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

using GA.Population;
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

        [Header("Nerual net")]
        public double randomWeight = 1;
        public int inputCount = 6;
        public int outputCount = 2;
        public int hiddenLayers = 1;
        public int hiddenLayerNeurons = 6;

        [Header("Unity agents config")]
        public Transform spawnPoint;
        public Transform agentsBuf;
        public Transform target;
        public GameObject agentPrefab;
        public List<AgentCtrl> agents = new List<AgentCtrl>();

        private void Start()
        {
            Debug.Assert(spawnPoint != null);
            Debug.Assert(agentsBuf != null);
            Debug.Assert(target != null);
            Debug.Assert(agentsBuf != null);
            InitAll();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                InitAll();
        }

        public void InitAll()
        {
            InitGAInternal();
            InitAgents();
        }

        public void InitAgents()
        {
            if (agents.Count() != 0)
            {
                foreach (var agent in agents)
                    GameObject.Destroy(agent);
            }
            agents.Clear();

            for (int i = 0; i < poplLen; i++)
            {
                var newAgent = Instantiate(agentPrefab, agentsBuf)
                    .GetComponent<AgentCtrl>();
                newAgent.transform.position = spawnPoint.position;
                newAgent.Init(population.Genomes[i] as INeuralGenome, target);
            }
        }

        public void InitGAInternal()
        {
            System.Random rand;

            rand = (seedRandom) ? new System.Random(1) : new System.Random();

            var randomGenomeGenerator = new PthF_RandGenomeGenerator(
                rand,
                randomWeight,
                inputCount,
                outputCount,
                hiddenLayers,
                hiddenLayerNeurons);

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
    }
}