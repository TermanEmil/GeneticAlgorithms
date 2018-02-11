namespace GA.NeuralNet.NeuronClass
{
    public class Neuron
    {
        public int innovNb;
        public ENeurType neurType;

        private double val;
        public double Val
        {
            get { return (neurType == ENeurType.bias) ? 1.0d : val; }
            set { val = value; }
        }

        private bool isCalculated;
        public bool IsCalculated
        {
            get { return (neurType == ENeurType.bias) ? true : isCalculated; }
            set { isCalculated = value; }
        }

        public Neuron(
            int innovNb_,
            ENeurType neurType_,
            double val_ = 0d)
        {
            innovNb = innovNb_;
            neurType = neurType_;
            Val = val_;
            IsCalculated = false;
        }
    }
}