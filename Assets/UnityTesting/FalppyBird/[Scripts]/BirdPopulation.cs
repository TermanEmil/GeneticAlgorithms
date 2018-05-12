using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using GA.UnityProxy.Genome;
using GA.UnityProxy.Population;
using GA.NeuralNet.RandomGenomeGenerator;
using GA.NeuralNet.Activation;
using GA.NeuralNet.NeuralGenome;
using GA.GenerationGenerator.GenomeProducer.Breeding.Selection;
using GA.NeuralNet.SynapseStruct;

namespace GA_Tests.FlappyBird
{
    public class BirdPopulation : PopulationProxy
    {
        public static BirdPopulation instance;

        [Header("Additional Neural net config")]
        [SerializeField] private int[] hiddenLayers = null;
        public float columnFitness = 5;
        public float columnPowFitness = 2;
        public float xDistFitness = 0.5f;
        public float yDistFitness = 1f;

        [Header("Unity configs")]
        [SerializeField] private GameObject prefab = null;
        [SerializeField] private Transform agentsBuf = null;
        [SerializeField] private Transform spawnPoint = null;
        [SerializeField] private float lifespan = 60;
        private float generationStartTime = -1;
        private Transform groundPoint;

        protected override void Awake()
        {
            instance = this;
            base.Awake();
        }

        private void Start()
        {
            Init();
            groundPoint = GameObject.FindWithTag("GroundPoint").transform;
        }

        private void Update()
        {
            if ((generationStartTime + lifespan < Time.time) ||
                (agents.FirstOrDefault(x => x.gameObject.activeSelf) == null))
            {
                agents.ForEach(x => (x as BirdAgent).Die());
                NextGeneration();
                ColumnPool.Instance.ResetColumns();
            }
        }

        public void ComputeFitness(BirdAgent agent)
        {
            float fitness = 0;

            fitness += (Mathf.Pow(agent.columns, columnPowFitness) - 1) *
                columnFitness;

            fitness += (ColumnPool.Instance.visibilityPoint.position.x +
                        ColumnPool.Instance.distBetweenColumns -
                        agent.distToNextColOnDeath.x) * xDistFitness;

            //fitness += (Mathf.Abs(agent.distToNextColOnDeath.y -
            //groundPoint.position.y)) * yDistFitness;

            Debug.Log(fitness);
            agent.genome.Fitness = fitness;
        }

        protected override void InitAgents()
        {
            int i;

            if (agents.Count == 0)
            {
                for (i = 0; i < poplLen; i++)
                {
                    var newAgent = Instantiate(prefab, agentsBuf).transform;
                    var proxy = newAgent.GetComponent<GenomeProxy>();
                    agents.Add(proxy);
                }
            }

            i = 0;
            foreach (var agent in agents)
            {
                agent.Init(population.Genomes[i] as INeuralGenome);
                agent.transform.position = spawnPoint.position;
                agent.transform.rotation = spawnPoint.rotation;
                agent.gameObject.SetActive(true);
                i++;
            }
        }

        protected override IRandNeuralGenomeGenerator NewRandNeurGnmGenerator()
        {
            return new RandNeuralGenomeGeneratorBase(
                rand,
                new Sigmoid(),
                weightMutRange,
                inputCount,
                outputCount,
                hiddenLayers,
                createBias);
        }

        protected override void NextGeneration()
        {
            agents.ForEach(x => ComputeFitness(x as BirdAgent));
            generationStartTime = Time.time;
            population.Genomes = population.Genomes
                .OrderByDescending(x => x.Fitness)
                .ToList();

            //agents.OrderByDescending(x => x.genome.Fitness).First().gameObject.SetActive(true);
            //Debug.DebugBreak();
            base.NextGeneration();
        }

        protected override ISelection<Synapse> NewSelector(int toSelect = 2)
        {
            return new RouletteWheelSelection<Synapse>(rand, toSelect);
        }
    }
}