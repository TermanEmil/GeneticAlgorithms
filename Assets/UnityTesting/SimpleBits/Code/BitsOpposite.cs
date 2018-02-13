using UnityEngine;
using System.Collections;

using GA.NeuralNet.NeuralGenome;

namespace GA_Tests.SimpleBits
{
    /// <summary>
    /// Learn to give the opposite bits.
    /// </summary>
    public class BitsOpposite : SimpleBits
    {
        public int MaxCount = 8;

        protected override double ComputeFitness(INeuralGenome genome)
        {
            double error = 0;

            for (int i = 0; i < MaxCount; i++)
            {
                var bits = ToBits(i, MaxCount);
                var expected = OppositeBits(bits);
                var outputs = genome.FeedNetwork(bits);

                for (int j = 0; j < bits.Length; j++)
                    error += Mathf.Abs((float)(expected[j] - outputs[j]));
            }

            genome.Fitness = -error;
            return error;
        }

        public override void ComputeUIButtonRequest()
        {
            string result = "";
            var bestGenome = GetBestGenome();

            for (int i = 0; i < MaxCount; i++)
            {
                var bits = ToBits(i, MaxCount);
                var expected = OppositeBits(bits);
                var outputs = bestGenome.FeedNetwork(bits);

                var line = "";

                for (int j = 0; j < bits.Length; j++)
                    line += string.Format("{0:0.00} ", expected[j]);
                line += " | ";
                for (int j = 0; j < bits.Length; j++)
                    line += string.Format("{0:0.00} ", outputs[j]);
                
                result += line + System.Environment.NewLine;
            }
            textField.text = result;
        }

        private double[] OppositeBits(double[] bits)
        {
            var result = new double[bits.Length];
            for (int i = 0; i < bits.Length; i++)
                result[i] = (bits[i] > 0.5) ? 0 : 1;
            return result;
        }
    }
}