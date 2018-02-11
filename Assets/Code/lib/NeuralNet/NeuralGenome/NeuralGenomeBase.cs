using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using GA.Gene;
using GA.Genome;
using GA.NeuralNet.SynapseStruct;
using GA.NeuralNet.NeuronClass;
using GA.NeuralNet.Activation;

namespace GA.NeuralNet.NeuralGenome
{
    public class NeuralGenomeBase : GenomeBase<Synapse>, INeuralGenome
    {
        public IDictionary<Neuron, IList<Synapse>> Network
        {
            get;
            protected set;
        }

        /// <summary>
        /// The same network, but it defines connections between innovation nb
        /// and Neurons.
        /// </summary>
        public IDictionary<int, Neuron> InnovNbNetwork
        {
            get;
            protected set;
        }

        public IActivation ActivationF { get; set; }

        public NeuralGenomeBase(
            IList<Gene<Synapse>> genes,
            IActivation activationF) : base(genes)
        {
            ActivationF = activationF;

            ReconnectNeuralNetwork();
            ReconnectInnovNbNetwork();
        }

        public IList<double> FeedNetwork(IDictionary<int, double> inputs)
        {
            foreach (var kv in Network)
                kv.Key.IsCalculated = false;

            foreach (var kv in inputs)
            {
                InnovNbNetwork[kv.Key].Val = kv.Value;
                InnovNbNetwork[kv.Key].IsCalculated = true;
            }

            var outputs = GetNeuronsOfType(ENeurType.output);
            foreach (var output in outputs)
                ComputNeuronVal(output);

            return outputs.Select(x => x.Val).ToArray();
        }   

        protected Neuron[] GetNeuronsOfType(ENeurType neurType)
        {
            return Network.Where(kv => kv.Key.neurType == neurType)
                          .Select(kv => kv.Key)
                          .ToArray();
        }

        /// <summary>
        /// Reconnectes the Network, remembering the connections
        /// between neurons, making FeedNeuralNetwork() faster.
        /// </summary>
        protected void ReconnectNeuralNetwork()
        {
            if (Network == null)
                Network = new Dictionary<Neuron, IList<Synapse>>();
            else
                Network.Clear();

            foreach (var gene in Genes)
            {
                if (!Network.ContainsKey(gene.Val.transmitter))
                    Network.Add(gene.Val.transmitter, new List<Synapse>(1));
                if (!Network.ContainsKey(gene.Val.receiver))
                    Network.Add(gene.Val.receiver, new List<Synapse>(1));

                Network[gene.Val.transmitter].Add(gene.Val);
                Network[gene.Val.receiver].Add(gene.Val);
            }
        }

        protected void ReconnectInnovNbNetwork()
        {
            if (InnovNbNetwork == null)
                InnovNbNetwork = new Dictionary<int, Neuron>();
            else
                InnovNbNetwork.Clear();

            foreach (var kv in Network)
            {
                if (!InnovNbNetwork.ContainsKey(kv.Key.innovNb))
                    InnovNbNetwork.Add(kv.Key.innovNb, kv.Key);
                else
                    Debug.Assert(InnovNbNetwork[kv.Key.innovNb] == kv.Key);
            }
        }

        /// <summary>
        /// The target neuron is set as calculated, so that it works for
        /// recurrent neural networks.
        /// </summary>
        private void ComputNeuronVal(Neuron target)
        {
            double sum;

            target.IsCalculated = true;
            sum = 0d;
            foreach (var synapse in Network[target])
            {
                Debug.Assert(synapse.receiver == target);
                if (!synapse.isEnabled)
                    continue;

                if (!synapse.transmitter.IsCalculated)
                    ComputNeuronVal(synapse.transmitter);
                sum += synapse.weight * synapse.transmitter.Val;
            }
            target.Val = ActivationF.Activate(sum);
        }
    }
}