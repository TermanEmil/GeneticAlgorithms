using System.Collections.Generic;
using System.Linq;
using GA.Genome;

namespace GA.GenerationGenerator.Breeding.Selection
{
    public abstract class SelectionBase<T> : ISelection<T>
    {
        public int TotalRequiredNb { get; set; }
        public int CountToSelect { get; set; }
        public IList<IGenome<T>> AllGenomes { get; set; }

        public SelectionBase(int countToSelect)
        {
            CountToSelect = countToSelect;
        }

        public void BeforeAllSelections(
            int requiredNb,
            IList<IGenome<T>> allGenomes)
        {
            if (allGenomes == null || allGenomes.Count() < requiredNb)
            {
                throw new System.Exception(
                    "Genomes can't be null or 0 length.");
            }

            TotalRequiredNb = requiredNb;
            AllGenomes = allGenomes;
            DoBeforeAllSelections();
        }

        protected abstract void DoBeforeAllSelections();
        public abstract IList<IGenome<T>> SelectNext();
    }
}