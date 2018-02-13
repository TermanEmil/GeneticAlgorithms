using UnityEngine;
using System.Collections;

using GA.NeuralNet.NeuralGenome;

namespace GA_Tests.SimpleBits
{
    public class SimpleBitsXOR : SimpleBits
    {
        protected override double ComputeFitness(INeuralGenome genome)
        {
            float error = 0;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    var targetOutput = i ^ j;
                    var output = genome.FeedNetwork(new double[2] { i, j })[0];
                    error += Mathf.Abs((float)(targetOutput - output));
                }
            }
            genome.Fitness = Mathf.Pow(4 - error + 1, fitenssPower);
            return error;
        }
    }
}