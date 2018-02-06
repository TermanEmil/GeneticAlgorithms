using System.Collections.Generic;
using System.Linq;
using GA.Genome;
using GA.Genome.Generator;
using GA.GenerationGenerator;

namespace GA.Population
{
    public class PopulationBase<T> : IPopulation<T>
    {
        public int Generation { get; set; }
        public IList<IGenome<T>> Genomes { get; set; }

        protected IGenomeGenerator<T> GenomeGenerator { get; set; }
        protected IGenerationGenerator<T> GenerationGenerator { get; set; }

        public PopulationBase(int populationLength,
                              IGenomeGenerator<T> genomeGenerator,
                              IGenerationGenerator<T> generationGenerator)
        {
            GenomeGenerator = genomeGenerator;
            GenerationGenerator = generationGenerator;
            Populate(populationLength);
        }

        public void Populate(int n)
        {
            Genomes = Enumerable.Range(0, n)
                                .Select(x => GenomeGenerator.NewRandomGenome())
                                .ToArray();
        }

        public void Evolve()
        {
            int prevPoplLen;

            prevPoplLen = Genomes.Count();

            Genomes = GenerationGenerator.Generate(Genomes);
            if (!Genomes.Any())
                Populate(prevPoplLen);
            
            Generation++;
        }
    }
}