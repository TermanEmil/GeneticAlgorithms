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
                totalFitness = genomesAndFitnessBuf.Sum(kv => kv.Value);
                tmp = RandomSelectElement(
                    totalFitness, genomesAndFitnessBuf, RandomInst);

                genomesAndFitnessBuf.Remove(tmp);
                result.Add(tmp);
            }
            return result;
        }

        protected static TKey RandomSelectElement<TKey>(
            double limit,
            Dictionary<TKey, double> dict,
            Random rand)
        {
            double randVal;

            randVal = rand.NextDouble() * limit;
            foreach (var kv in dict)
            {
                if (randVal <= kv.Value)
                    return kv.Key;
                else
                    randVal -= kv.Value;
            }

            if (randVal <= 0.01)
                return dict.Last().Key;

            throw new Exception("Random select failed.");
        }

        /// <summary>
        /// A test to proove that the random select works fine
        /// </summary>
        public static int[] TestRandomness(
            int tests,
            double maxVal,
            int dataCount = 10)
        {
            double totalSum;
            Random rand;
            Dictionary<int, double> data;
            int[] buf;

            rand = new Random();
            data = new Dictionary<int, double>(dataCount);
            for (int i = 0; i < dataCount; i++)
                data.Add(dataCount - i - 1, maxVal / (i + 1));
            totalSum = data.Sum(kv => kv.Value);
            buf = new int[data.Count()];

            for (int i = 0; i < tests; i++)
                buf[RandomSelectElement(totalSum, data, rand)]++;

            return buf;
        }
    }
}