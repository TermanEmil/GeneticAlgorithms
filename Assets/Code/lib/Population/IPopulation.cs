using System.Collections.Generic;
using GA.Genome;
using GA.Genome.RandGenerator;
using GA.GenerationGenerator;

namespace GA.Population
{
    public interface IPopulation<T>
    {
        int Generation {get; }
        IList<IGenome<T>> Genomes { get; set; }

        void Populate(int n);
        void Evolve();
    }
}