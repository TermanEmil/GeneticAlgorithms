using System.Linq;
using GA.Genome;
using System;
using GA.Fitness;
using System.Diagnostics;

namespace GA_Tests.Shakespeare
{
    public class Sh_FitnessEval : IFitnessEval<char>
    {
        public string TargetString { get; set; }
        public string Chars { get; set; }

        public Sh_FitnessEval(string targetStr, string chars)
        {
            TargetString = targetStr;
            Chars = chars;
        }

        public double Evaluate(IGenome<char> genome)
        {
            var genomeStr = string.Join("",
                genome.Genes.Select(x => x.Val.ToString()));

            Debug.Assert(genomeStr.Length == TargetString.Length,
                         "Invalid len");

            return Math.Pow(genomeStr.Zip(
                TargetString,
                (c1, c2) => (c1 == c2) ? 1 : 0).Sum(), 1.8d);
        }
    }
}