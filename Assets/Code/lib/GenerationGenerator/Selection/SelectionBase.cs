using System.Collections.Generic;
using System.Linq;
using GA.Genome;

namespace GA.GenerationGenerator.Selection
{
    public abstract class SelectionBase<T> : ISelection<T>
    {
        public int TotalRequiredNb { get; set; }
        public IList<IGenome<T>> AllGenomes { get; set; }

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
        public abstract void BeforeSelection(int iter);
        public abstract IGenome<T> SelectNext();
    }
}