using System.Collections.Generic;
using System.Linq;
using GA.Genome;
using System;

namespace GA.GenerationGenerator.Reinsertion
{
    public class ReinsertBest<T> : IReinsertion<T>
    {
        public int FixedNbOfGenomesToGet { get; set; } = -1;
        protected bool UseInt { get; set; } = false;

        public double PartOfGenomesToGet { get; set; } = -1;
        protected bool UsePart { get; set; } = false;

        /// <summary>
        /// The round function used to round the PartOfGenomes.
        /// By default, it uses this.IntTruncate.
        /// </summary>
        public Func<double, int> RounderF { get; set; }

        /// <summary>
        /// A function that is used to determine how many genomes will be get.
        /// </summary>
        public Func<IEnumerable<IGenome<T>>, int> GetNbOfGenomesF { get; set; } 

        private void ReinsertBestConstrBase()
        {
            GetNbOfGenomesF = TheActualNbOfGenomesToGet;
        }

        public ReinsertBest(int n)
        {
            UseInt = true;
            FixedNbOfGenomesToGet = n;
        }

        public ReinsertBest(double part)
        {
            UsePart = true;
            PartOfGenomesToGet = part;
            RounderF = IntTruncate;
        }

        public IList<IGenome<T>> GetGenomes(IEnumerable<IGenome<T>> genomes)
        {
            int n;

            n = GetNbOfGenomesF(genomes);
            if (n < genomes.Count())
            {
                throw new Exception(string.Format(
                    "Reinsertion: Invalid number: {0} / {1}",
                    n, genomes.Count()));
            }
            return genomes.Take(n).ToArray();
        }

        public int TheActualNbOfGenomesToGet(
            IEnumerable<IGenome<T>> genomes = null)
        {
            if (UseInt)
                return FixedNbOfGenomesToGet;
            else
                return RounderF(genomes.Count() * PartOfGenomesToGet);
        }

        public int IntTruncate(double value)
        {
            return (int)Math.Truncate(value);
        }
    }
}