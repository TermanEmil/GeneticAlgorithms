using GA.Population;
using GA.GenerationGenerator;
using GA.Fitness;
using System;
using System.Linq;

using GA.GenerationGenerator.GenomeProducer.Reinsertion;
using GA.GenerationGenerator.GenomeProducer.Breeding;
using GA.GenerationGenerator.GenomeProducer.Breeding.Selection;
using GA.GenerationGenerator.GenomeProducer.Breeding.Crossover;
using GA.GenerationGenerator.GenomeProducer.Breeding.Mutation;

namespace GA_Tests.EvolveSum
{
    public class GAEvolveSum
    {
        public int GenomeNumbers { get; private set; }

        public PopulationBase<int> Population { get; private set; }
        public IFitnessEval<int> FitnessEval { get; set; }

        public int TargetSum { get; private set; }
        private Random RandomInst { get; set; }

        public double BestFitness
        {
            get { return Population.Genomes.Max(x => x.Fitness); }
        }

        public int BestSum
        {
            get
            {
                double bestFitness = BestFitness;
                return Population.Genomes.First(x => x.Fitness >= bestFitness)
                                 .Genes.Sum(x => x.Val);
            }
        }

        public bool MaxReached
        {
            get { return BestSum == TargetSum; }
        }

        public GAEvolveSum(
            Random rand,
            double partToReinsert,
            int targetSum,
            int poplLen,
            int genomeNumbers,
            int nbInterval,
            double geneMutChance,
            int geneMutRange)
        {
            ES_RandGenomeGenerator randGenomeGenerator;
            GenerationGeneratorBase<int> generationGenerator;
            IReinsertion<int> reinsertion;
            IBreeding<int> breeding;

            RandomInst = rand;
            TargetSum = targetSum;
            FitnessEval = new ES_FitnessEval(targetSum);

            randGenomeGenerator = new ES_RandGenomeGenerator(genomeNumbers,
                                                             nbInterval,
                                                             rand);
            reinsertion = new ReinsertBest<int>(
                (int)(poplLen * partToReinsert));
            
            breeding = NewBreeding(
                poplLen,
                partToReinsert,
                geneMutChance,
                geneMutRange);

            generationGenerator = new GenerationGeneratorBase<int>(
                poplLen,
                reinsertion,
                breeding);

            Population = new PopulationBase<int>(
                poplLen,
                randGenomeGenerator,
                generationGenerator);
        }

        public void PassGeneration()
        {
            //Population.Genomes = Population.Genomes.OrderByDescending(x => x.Fitness).ToArray();
            Population.Evolve();
        }

        public void EvaluateGenomes()
        {
            foreach (var genome in Population.Genomes)
                genome.Fitness = FitnessEval.Evaluate(genome);
        }

        private BreedingBase<int> NewBreeding(
            int poplLen,
            double partToReinsert,
            double geneMutChance,
            int mutRange)
        {
            BreedingBase<int> result;
            ISelection<int> selector;
            ICrossover<int> crossover;
            IMutation<int> mutator;

            selector = new EliteSelection<int>(2, partToBeElites: 0.5d);
            crossover = new SinglePointCrossover<int>(RandomInst, 1);
            mutator = new ES_Mutation(RandomInst, geneMutChance, mutRange);

            result = new BreedingBase<int>(
                poplLen - (int)(partToReinsert * poplLen),
                selector,
                crossover,
                mutator
            );
            return result;
        }
    }
}