using GA.Population;
using GA.GenerationGenerator;
using GA.Fitness;
using System;
using System.Linq;

namespace GA_Tests.EvolveSum
{
    public class GAEvolveSum
    {
        public int GenomeNumbers { get; private set; }

        public PopulationBase<int> Population { get; private set; }
        public IFitnessEval<int> FitnessEval { get; set; }

        public int TargetSum { get; set; }
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
            int targetSum,
            int poplLen,
            int genomeNumbers,
            int nbInterval)
        {
            IGenerationGenerator<int> generationGenerator;
            EvolveSumGenomeGenerator genomeGenerator;

            GenomeNumbers = genomeNumbers;
            RandomInst = rand;
            TargetSum = targetSum;

            FitnessEval = new EvolveSumFitnessEval(targetSum);
            genomeGenerator = new EvolveSumGenomeGenerator(
                genomeNumbers,
                nbInterval,
                RandomInst);

            generationGenerator = new EvolveSumGenerationGenerator(RandomInst);
            Population = new PopulationBase<int>(
                poplLen,
                genomeGenerator,
                generationGenerator);
        }

        public void PassGeneration()
        {
            foreach (var genome in Population.Genomes)
                genome.Fitness = FitnessEval.Evaluate(genome);
            Population.Evolve();
        }
    }
}