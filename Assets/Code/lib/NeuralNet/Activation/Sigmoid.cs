namespace GA.NeuralNet.Activation
{
    public class Sigmoid : IActivation
    {
        public double Activate(double x)
        {
            return 1.0d / (1 + System.Math.Exp(-x));
        }
    }
}