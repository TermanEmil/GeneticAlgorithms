using UnityEngine;
using System.Collections;

using GA.NeuralNet.NeuralGenome;

namespace GA_Tests.SimpleBits
{
    /// <summary>
    /// Test the algorithm if it learns to determine if the input is an odd or
    /// even number.
    /// </summary>
    public class BitsParity : SimpleBits
    {
        protected override double ComputeFitness(INeuralGenome genome)
        {
            return ParityFitness(genome);
        }

        private double ParityFitness(INeuralGenome genome)
        {
            float error = 0;

            for (int i = 0; i < 8; i++)
            {
                var expected = (i % 2 == 0) ? 1 : 0;
                var output = (float)genome.FeedNetwork(ToBits(i))[0];
                error += Mathf.Abs(expected - output);
            }
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
                                        bestGenome.FeedNetwork(ToBits(i))[0]);
                result += System.Environment.NewLine;
            }
            textField.text = result;
        }
    }
}