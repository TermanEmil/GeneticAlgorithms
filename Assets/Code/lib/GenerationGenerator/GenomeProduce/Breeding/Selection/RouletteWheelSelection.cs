using GA.Genome;
using System.Collections.Generic;
using System.Linq;
using System;

namespace GA.GenerationGenerator.GenomeProducer.Breeding.Selection
{
    public class RouletteWheelSelection<T> : SelectionBase<T>
    {
        public Random RandomInst { get; set; }

        /// <summary>
        /// A storage used to remember the initial state.
        /// </summary>
        protected Dictionary<IGenome<T>, double> GenomesAndFitness { get; set; }

        /// <summary>
        /// Buffer used for opperations. Once a genome is picked, it's removed
        /// from here. Every time a set of parrents is picked, this buffer
        /// is first copied from GenomesAndFitness.
        /// In this way, there can't be selected the same genome twice.
        /// </summary>
        protected Dictionary<IGenome<T>, double> GenomesAndFitnessBuf
        {
            get;
            set;
        }

        public RouletteWheelSelection(
            Random random,
            int countToSelect) : base(countToSelect)
        {
            RandomInst = random;
        }

        protected override void DoBeforeAllSelections()
        {
            double minFitness;

            minFitness = AllGenomes.Min(x => x.Fitness);
            if (minFitness < 0)
                minFitness = Math.Abs(minFitness);
            else
                minFitness = 0;

            GenomesAndFitness =
                AllGenomes.OrderByDescending(x => x.Fitness)
                          .ToDictionary(k => k,v => v.Fitness + minFitness);
        }

        public override IList<IGenome<T>> SelectNext()
        {
            IGenome<T>                      tmp;
            List<IGenome<T>>                result;
            double                          totalFitness;
            Dictionary<IGenome<T>, double>  genomesAndFitnessBuf;

            genomesAndFitnessBuf =
                GenomesAndFitness.ToDictionary(kv => kv.Key, kv => kv.Value);

            result = new List<IGenome<T>>(CountToSelect);
            for (int i = 0; i < CountToSelect; i++)
            {
                totalFitness = GenomesAndFitnessBuf.Sum(kv => kv.Value);
                tmp = RandomSelectElement(totalFitness, GenomesAndFitnessBuf);

                GenomesAndFitnessBuf.Remove(tmp);
                result.Add(tmp);
            }
            return result;
        }

        protected TKey RandomSelectElement<TKey>(
            double limit,
            Dictionary<TKey, double> dict)
        {
            double rnd;

            rnd = RandomInst.NextDouble() * limit;
            foreach (var kv in dict)
            {
                if (rnd <= kv.Value)
                    return kv.Key;
                else
                    rnd -= kv.Value;
            }

            if (rnd <= 0.001f)
                return dict.Last().Key;

            throw new Exception("Random select failed.");
        }
    }
}