using UnityEngine;
using System.Collections.Generic;

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
using GA.UnityProxy.Genome;
using GA.GenerationGenerator.GenomeProducer;

namespace GA.UnityProxy.Population
{
    /// <summary>
    /// A proxy between the IPopulation<Synapse> and Monobehaviour.
    /// </summary>
    public abstract class PopulationProxy : MonoBehaviour
    {
        [Header("Random")]
        [SerializeField] protected bool seedRandom = false;
        [SerializeField] protected int seedValue = 1;
        protected System.Random rand;

        [Header("General config")]
        [SerializeField] protected int poplLen = 50;
        [SerializeField] protected IPopulation<Synapse> population;
        [SerializeField] protected double partToReinsert = 0.1d;

        [Header("Nerual net")]
        [SerializeField] protected double initWeightRange = 1;
        [SerializeField] protected int inputCount = 6;
        [SerializeField] protected int outputCount = 2;
        [SerializeField] protected bool createBias = true;

        [Header("Mutation")]
        [SerializeField] protected double geneMutChance = 0.1d;
        [SerializeField] protected double weightMutRange = 1d;

        [HideInInspector]
        public List<GenomeProxy> agents = new List<GenomeProxy>();

        /// <summary>
        /// Initialize the agents, adding them into <see cref="agents"/>.
        /// </summary>
        protected abstract void InitAgents();
        protected abstract IRandNeuralGenomeGenerator NewRandNeurGnmGenerator();

        protected virtual void Awake()
        {
            if (seedRandom)
                rand = new System.Random(seedValue);
            else
                rand = new System.Random();
        }

        public virtual void Init()
        {
            InitAlgorithm();
            InitAgents();
        }

        protected virtual void InitAlgorithm()
        {
            var randomGenomeGenerator = NewRandNeurGnmGenerator();
            var generationGenerator = NewGenerationGenerator(NewProducers());

            population = new PopulationBase<Synapse>(
                poplLen,
                randomGenomeGenerator,
                generationGenerator);
        }

        protected virtual IBreeding<Synapse> NewBreeding()
        {
            var selector = NewSelector(2);
            var crossover = NewCrossover();
            var mutator = NewMutator();

            return new BreedingBase<Synapse>(
                poplLen - (int)(partToReinsert * poplLen),
                selector,
                crossover,
                mutator);
        }

        protected virtual void NextGeneration()
        {
            if (agents.Count == 0)
                return;

            population.Evolve();
            InitAgents();
        }

        protected virtual ISelection<Synapse> NewSelector(int toSelect = 2)
        {
            return new EliteSelection<Synapse>(toSelect);
        }

        protected virtual ICrossover<Synapse> NewCrossover()
        {
            return new SinglePointCrossover<Synapse>(rand);
        }

        protected virtual IMutation<Synapse> NewMutator()
        {
            return new FixedNeuralNetMutation(
                rand,
                geneMutChance,
                weightMutRange);
        }

        protected virtual IReinsertion<Synapse> NewReinsertor()
        {
            return new ReinsertBest<Synapse>((int)(poplLen * partToReinsert));   
        }

        protected virtual IList<IGenomeProducer<Synapse>> NewProducers()
        {
            return new IGenomeProducer<Synapse>[]
            {
                NewReinsertor(),
                NewBreeding()
            };
        }

        protected virtual IGenerationGenerator<Synapse> NewGenerationGenerator(
            IList<IGenomeProducer<Synapse>> producers)
        {
            return new GenerationGeneratorBase<Synapse>(poplLen, producers);
        }
    }
}