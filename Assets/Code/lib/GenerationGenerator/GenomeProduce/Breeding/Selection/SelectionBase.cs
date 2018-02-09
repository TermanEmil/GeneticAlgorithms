using System.Collections.Generic;
using System.Linq;
using GA.Genome;

namespace GA.GenerationGenerator.GenomeProducer.Breeding.Selection
{
    public abstract class SelectionBase<T> : ISelection<T>
    {
        public int CountToSelect { get; set; }
        public IList<IGenome<T>> AllGenomes { get; set; }

        public SelectionBase(int countToSelect)
        {
            CountToSelect = countToSelect;
        }

        public void BeforeAllSelections(IList<IGenome<T>> allGenomes)
        {
            if (allGenomes == null)
            {
                throw new System.Exception(
                    "Genomes can't be null or 0 length.");
            }

            AllGenomes = allGenomes;
            DoBeforeAllSelections();
        }

        protected abstract void DoBeforeAllSelections();
        public abstract IList<IGenome<T>> SelectNext();
    }
}