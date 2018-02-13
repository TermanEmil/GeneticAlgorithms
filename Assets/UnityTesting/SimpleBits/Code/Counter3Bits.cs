using UnityEngine;
using System.Collections;
using System.Linq;

using GA.NeuralNet.NeuralGenome;

namespace GA_Tests.SimpleBits
{
    /// <summary>
    /// Learn to count.
    /// For a given number, get the next number. The inputs and outputs are 
    /// represented in bits.
    /// 
    /// But it fails to learn...
    /// </summary>
    public class Counter3Bits : SimpleBits
    {
        protected override double ComputeFitness(INeuralGenome genome)
        {
            return CountingFitness(genome);
        }

        private double CountingFitness(INeuralGenome genome)
        {
            float error = 0;

            for (int i = 0; i < 7; i++)
            {
                var next3Bit = i + 1;
                if (next3Bit >= 8)
                    next3Bit = 0;
                error += Mathf.Abs(next3Bit - GetNetworkOutputAsNb(genome, i));
            }
            //genome.Fitness = Mathf.Pow(8 - error + 1, fitenssPower);
            genome.Fitness = 8 - error;
            return error;
        }

        public override void ComputeUIButtonRequest()
        {
            string result = "";
            var bestGenome = GetBestGenome();

            for (int i = 0; i < 8; i++)
            {
                result += string.Format("{0}) {1:0.00}",
                        i,
                        GetNetworkOutputAsNb(bestGenome, i));
                result += System.Environment.NewLine;
            }
            textField.text = result;
        }
    }
}