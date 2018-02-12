using GA.NeuralNet.NeuronClass;

namespace GA.NeuralNet.SynapseStruct
{
    public struct Synapse
    {
        public int transmitter;
        public int receiver;
        public double weight;
        public bool isEnabled;

        public Synapse(
            int transmitter_,
            int receiver_,
            double weight_)
        {
            transmitter = transmitter_;
            receiver = receiver_;
            weight = weight_;
            isEnabled = true;
        }

        public override string ToString()
        {
            return string.Format("{0} __{1:0.00}__> {2}",
                                 transmitter,
                                 weight,
                                 receiver);
        }
    }
}