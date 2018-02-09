using System.Collections.Generic;
using System.Linq;
using GA.Genome;
using GA.Genome.RandGenerator;
using GA.GenerationGenerator;

namespace GA.Population
{
    public class PopulationBase<T> : IPopulation<T>
    {
        public int Generation { get; set; }
        public IList<IGenome<T>> Genomes { get; set; }

        public IRandGenomeGenerator<T> RandGnmGenerator { get; set; }
        public IGenerationGenerator<T> GenerationGenerator { get; set; }

        public PopulationBase(int populationLength,
                              IRandGenomeGenerator<T> genomeGenerator,
                              IGenerationGenerator<T> generationGenerator)
        {
            RandGnmGenerator = genomeGenerator;
            GenerationGenerator = generationGenerator;
            Populate(populationLength);
        }

        public void Populate(int n)
        {
            Genomes = Enumerable.Range(0, n)
                                .Select(x => RandGnmGenerator.NewRandomGenome())
                                .ToArray();
        }

        public void Evolve()
        {
            int prevPoplLen;

            prevPoplLen = Genomes.Count();

            Genomes = GenerationGenerator.Generate(Genomes);
            if (!Genomes.Any())
            {
                UnityEngine.Debug.Log("repopulate!");
                Populate(prevPoplLen);
            }
            
            Generation++;
        }

        public override string ToString()
        {
            int i;
            var rs = "{" + System.Environment.NewLine;

            i = 0;
            foreach (var genome in Genomes)
            {
                rs += i + ") " + genome + System.Environment.NewLine;
                i++;
            }
            rs += "}";
            return rs;
        }
    }
}