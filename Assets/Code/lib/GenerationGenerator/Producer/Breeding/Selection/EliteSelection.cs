using GA.Genome;
using System.Collections.Generic;
using System.Linq;

namespace GA.GenerationGenerator.Breeding.Selection
{
    public class EliteSelection<T> : SelectionBase<T>
    {
        public IList<IGenome<T>> Elites { get; protected set; }
        public int CurrentIndex { get; set; } = 0;

        public EliteSelection(int countToSelect) : base(countToSelect)
        {
            // Do nothing.
        }

        protected override void DoBeforeAllSelections()
        {
            Elites = AllGenomes.OrderByDescending(x => x.Fitness)
                               .Take(TotalRequiredNb)
                               .ToArray();
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
            return result;
        }
    }
}