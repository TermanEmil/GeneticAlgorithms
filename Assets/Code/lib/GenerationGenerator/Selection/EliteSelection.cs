using GA.Genome;
using System.Collections.Generic;
using System.Linq;

namespace GA.GenerationGenerator.Selection
{
    public class EliteSelection<T> : SelectionBase<T>
    {
        public IList<IGenome<T>> Elites { get; protected set; }
        public int CurrentIndex { get; set; } = 0;

        protected override void DoBeforeAllSelections()
        {
            Elites = AllGenomes.OrderByDescending(x => x.Fitness)
                               .Take(TotalRequiredNb)
                               .ToArray();
            CurrentIndex = 0;
        }

        public override void BeforeSelection(int iter)
        {
            // Do nothing.
        }

        public override IGenome<T> SelectNext()
        {
            IGenome<T> result;

            result = Elites[CurrentIndex];
            CurrentIndex++;
            if (CurrentIndex >= Elites.Count())
                CurrentIndex = 0;
            return result;
        }
    }
}