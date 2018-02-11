using GA.NeuralNet.NeuronClass;

namespace GA.NeuralNet.SynapseStruct
{
    public struct Synapse
    {
        public Neuron transmitter;
        public Neuron receiver;
        public double weight;
        public bool isEnabled;

        public Synapse(
            Neuron transmitter_,
            Neuron receiver_,
            double weight_)
        {
            transmitter = transmitter_;
            receiver = receiver_;
            weight = weight_;
            isEnabled = true;
        }
    }
}