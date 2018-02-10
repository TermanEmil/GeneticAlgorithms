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

namespace GA_Tests.Shakespeare
{
    public class ShakespeareSolver
    {
        const string chars = "$%#@!*abcdefghijklmnopqrstuvwxy" +
                "z1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^& ";

        public PopulationBase<char> Population { get; set; }
        private string TargetStr { get; set; }

        private Sh_FitnessEval FitnessEval { get; set; }

        public double BestFitness
        {
            get { return Population.Genomes.Max(x => x.Fitness); }
        }

        public string BestStr
        {
            get
            {
                var bestFitness = BestFitness;
                return string.Join("",
                    Population.Genomes.First(x => x.Fitness == bestFitness)
                                   .Genes.Select(x => x.Val.ToString()));
            }
        }

        public bool MaxReached
        {
            get { return TargetStr.Equals(BestStr); }
        }

        public ShakespeareSolver(
            string targetStr,
            Random rand,
            double partToReinsert,
            int popLen,
            double geneMutChance,
            int geneMutRange)
        {
            TargetStr = targetStr;
            FitnessEval = new Sh_FitnessEval(TargetStr, chars);

            var randGenomeGenerator = new Sh_RandGenomeGenerator(
                rand,
                targetStr.Length,
                geneMutRange,
                chars);

            var reinsertion = new ReinsertBest<char>(
                (int)(popLen * partToReinsert));

            var breeding = NewBreeding(
                rand,
                popLen,
                partToReinsert,
                geneMutChance,
                geneMutRange);

            var generationGenerator = new GenerationGeneratorBase<char>(
                popLen,
                reinsertion,
                breeding);

            Population = new PopulationBase<char>(
                popLen,
                randGenomeGenerator,
                generationGenerator);
        }

        public void PassGeneration()
        {
            Population.Evolve();
        }

        public void EvaluateGenomes()
        {
            foreach (var genome in Population.Genomes)
                genome.Fitness = FitnessEval.Evaluate(genome);
        }

        private IBreeding<char> NewBreeding(
            Random random,
            int poplLen,
            double partToReinsert,
            double geneMutChance,
            int mutRange)
        {
            var selector = new EliteSelection<char>(2);
            //var selector = new RouletteWheelSelection<char>(random, 2);

            var crossover = new SinglePointCrossover<char>(random);
            var mutator = new Sh_Mutation(
                random,
                geneMutChance,
                mutRange,
                chars);
            
            return new BreedingBase<char>(
                poplLen - (int)(partToReinsert * poplLen),
                selector,
                crossover,
                mutator);
        }
    }
}