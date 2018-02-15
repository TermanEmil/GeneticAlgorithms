using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GA.UnityProxy.Genome;
using GA.UnityProxy.Population;
using GA.NeuralNet.RandomGenomeGenerator;
using GA.NeuralNet.Activation;
using GA.NeuralNet.NeuralGenome;
using System.Linq;

namespace GA_Tests.FlappyBird
{
    public class BirdPopulation : PopulationProxy
    {
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
            base.NextGeneration();
        }

        private void ComputeFitness(BirdAgent agent)
        {
            var fitness = Mathf.Pow(agent.columns * columnFitness,
                                    columnPowFitness);
            fitness += (ColumnPool.Instance.distBetweenColumns -
                        agent.distToNextColOnDeath.x) * xDistFitness;
            
            fitness += (agent.distToNextColOnDeath.y -
                        groundPoint.position.y) * yDistFitness;

            agent.genome.Fitness = fitness;
        }
    }
}