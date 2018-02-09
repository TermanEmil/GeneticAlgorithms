using GA.Genome;
using System.Collections.Generic;
using System.Linq;

namespace GA.GenerationGenerator.GenomeProducer.Breeding.Selection
{
    public class EliteSelection<T> : SelectionBase<T>
    {
        public IList<IGenome<T>> Elites { get; protected set; }
        public int CurrentIndex { get; set; } = 0;

        /// <summary>
        /// Part of genomes chosen to be elites.
        /// </summary>
        public double PartToBeElites { get; set; } = 0.5d;

        /// <summary>
        /// This number should be modified after instantiation.
        /// </summary>
        public int MinElites { get; set; } = 2;

        public EliteSelection(
            int countToSelect,
            double partToBeElites = 0.5d) : base(countToSelect)
        {
            PartToBeElites = partToBeElites;
        }

        protected override void DoBeforeAllSelections()
        {
            Elites = GetElites(AllGenomes);
            CurrentIndex = 0;
        }

        public override IList<IGenome<T>> SelectNext()
        {
            List<IGenome<T>> result;

            result = new List<IGenome<T>>(CountToSelect);
            for (int i = 0; i < CountToSelect; i++)
            {
                result.Add(Elites[CurrentIndex]);
                CurrentIndex++;
                if (CurrentIndex >= Elites.Count())
                    CurrentIndex = 0;
            }

            if (result.Count() != CountToSelect)
                ThrowInvalidSelected(result.Count());

            return result;
        }

        /// <summary>
        /// This method should be overriden for any more precisese Elite
        /// selection.
        /// </summary>
        protected IList<IGenome<T>> GetElites(IList<IGenome<T>> genomes)
        {
            int n;

            n = (int)(genomes.Count * PartToBeElites);
            n = System.Math.Max(MinElites, n);
            return genomes.OrderByDescending(x => x.Fitness)
                          .Take(n)
                          .ToArray();
        }

        private void ThrowInvalidSelected(int resultCount)
        {
            throw new System.Exception(
                string.Format("Elite selection: invalid number of " +
                              "genomes selected: {0}/{1}",
                              resultCount, CountToSelect));
        }
    }
}