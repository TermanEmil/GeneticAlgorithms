using System.Collections.Generic;
using System.Linq;
using System;

using GA.Gene;
using GA.Genome;
using GA.NeuralNet.SynapseStruct;
using GA.NeuralNet.NeuronClass;
using GA.NeuralNet.Activation;
using System.Threading.Tasks;

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
        public IDictionary<int, Neuron> Neurons
        {
            get;
            protected set;
        }

        public IList<Neuron> NeuronLst;
        public IActivation ActivationF { get; set; }

        public NeuralGenomeBase(
            IList<Neuron> neurons,
            IList<Gene<Synapse>> genes,
            IActivation activationF) : base(genes)
        {
            ActivationF = activationF;

            NeuronLst = neurons;
            ConnectInnovNbNetwork();
            ConnectNeuralNetwork();
        }

        public IList<double> FeedNetwork(double[] inputs)
        {
            foreach (var kv in Network)
                kv.Key.IsCalculated = false;

            for (int i = 0; i < inputs.Length; i++)
            {
                Neurons[i].Val = inputs[i];
                Neurons[i].IsCalculated = true;
            }

            var outputs = GetNeuronsOfType(ENeurType.output);
            foreach (var output in outputs)
                ComputNeuronVal(output);

            return outputs.Select(x => x.Val).ToArray();
        }   

        public override IGenome<Synapse> CreateNew(bool copyGenes = true)
        {
            IList<Gene<Synapse>> genes;
            IList<Neuron> neurons;

            if (copyGenes)
                genes = Genes.Select(x => new Gene<Synapse>(x)).ToArray();
            else
                genes = Genes;

            neurons = NeuronLst.Select(x => new Neuron(x.innovNb,
                                                       x.neurType,
                                                       x.Val))
                               .ToArray();
            return new NeuralGenomeBase(neurons, genes, ActivationF);
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
        protected void ConnectNeuralNetwork()
        {
            if (Network == null)
                Network = new Dictionary<Neuron, IList<Synapse>>();
            else
                Network.Clear();

            foreach (var gene in Genes)
            {
                if (!Network.ContainsKey(Neurons[gene.Val.transmitter]))
                {
                    Network.Add(Neurons[gene.Val.transmitter],
                                new List<Synapse>(1));
                }
                if (!Network.ContainsKey(Neurons[gene.Val.receiver]))
                {
                    Network.Add(Neurons[gene.Val.receiver],
                                new List<Synapse>(1));
                }

                Network[Neurons[gene.Val.transmitter]].Add(gene.Val);
                Network[Neurons[gene.Val.receiver]].Add(gene.Val);
            }
        }

        protected void ConnectInnovNbNetwork()
        {
            if (Neurons == null)
                Neurons = new Dictionary<int, Neuron>();
            else
                Neurons.Clear();

            foreach (var neuron in NeuronLst)
            {
                if (!Neurons.ContainsKey(neuron.innovNb))
                    Neurons.Add(neuron.innovNb, neuron);
                else if (Neurons[neuron.innovNb] != neuron)
                    throw new Exception("AAAAaAAAaaaaa: " + neuron.innovNb);
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
                if (synapse.receiver != target.innovNb)
                    continue;
                
                if (!synapse.isEnabled)
                    continue;

                if (!Neurons[synapse.transmitter].IsCalculated)
                    ComputNeuronVal(Neurons[synapse.transmitter]);
                sum += synapse.weight * Neurons[synapse.transmitter].Val;
            }
            target.Val = ActivationF.Activate(sum);
        }
    }
}