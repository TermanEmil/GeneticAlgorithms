#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

using GA.Gene;
using GA.NeuralNet.SynapseStruct;
using GA.NeuralNet.NeuronClass;
using GA.NeuralNet.NeuralGenome;
using GA.NeuralNet;
using GA.NeuralNet.RandomGenomeGenerator;

namespace GA.Graph
{
    public class GAGraph : MonoBehaviour
    {
        private static GAGraph instance = null;
        public static GAGraph Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<GAGraph>();
                return instance;
            }
        }

        private System.Random random;
        private System.Random RandomInst
        {
            get
            {
                return random = random ?? new System.Random();
            }
        }
        [Header("General config")]
        [SerializeField] private float weightSize = 1;
        [SerializeField] private float maxWeight = 20;
        [SerializeField] private bool autoAdjustMaxWeight = true;
        [SerializeField] private float connectionWidth = 0.5f;
        [SerializeField] private Color positiveWeightColor = Color.green;
        [SerializeField] private Color negativeWeightColor = Color.red;
        [SerializeField] private Color disabledColor = Color.black;
        [SerializeField] private float distBetweenNodes = 2;

        [Header("Obj references")]
        [SerializeField] GameObject nodePrefab = null;
        [SerializeField] Transform nodesParent = null;

        [SerializeField] private Transform inputNodesPos = null;
        [SerializeField] private Transform outputNodesPos = null;
        [SerializeField] private Transform bottom = null;

        [Header("Live Draw Parameters")]
        public int newGenomeInputCount = 2;
        public int newGenomeOutputCout = 1;
        public int hiddenNodes = 10;
        public int connectionMutations = 5;
        public float newGenomeRandomWeight = 10;

        [Header("Draw from given Genome")]
        public INeuralGenome givenNeuralNet = null;

        private Dictionary<Neuron, RectTransform> nodes
            = new Dictionary<Neuron, RectTransform>();

        public void DrawGenome(INeuralGenome genome)
        {
            RemoveAllNodes();

            if (autoAdjustMaxWeight)
                maxWeight = GetAverageWeight(genome.Genes);

            int count = 0;
            foreach (var neuron in genome.NeuronLst)
            {
                var newNode = Instantiate(nodePrefab, nodesParent)
                    .GetComponent<RectTransform>();
                newNode.name = "Node " + count;

                var nodeText = neuron.innovNb.ToString();
                if (neuron.neurType == ENeurType.bias)
                    nodeText = "bias" + nodeText;
                else if (neuron.neurType == ENeurType.input)
                    nodeText = "in" + nodeText;
                else if (neuron.neurType == ENeurType.output)
                    nodeText = "out" + nodeText;

                newNode.GetChild(0).GetComponent<Text>().text = nodeText;

                nodes.Add(neuron, newNode);
                count++;
            }

            var inputNeurons = NeuralUtils.GetNeurons(
                genome, ENeurType.input);
            var outputNeurons = NeuralUtils.GetNeurons(
                genome, ENeurType.output);
            var bias = NeuralUtils.GetBias(genome);

            foreach (var kv in nodes)
            {
                ConnectNode(kv.Value, kv.Key, genome);
                if (inputNeurons.Contains(kv.Key))
                {
                    int i = inputNeurons.IndexOf(kv.Key) + 1;
                    kv.Value.localPosition =
                            inputNodesPos.localPosition +
                            Vector3.down * distBetweenNodes * i;
                }
                else if (outputNeurons.Contains(kv.Key))
                {
                    int i = outputNeurons.IndexOf(kv.Key);
                    kv.Value.localPosition =
                            outputNodesPos.localPosition +
                            Vector3.down * distBetweenNodes * i;
                }
                else if (bias == kv.Key)
                    kv.Value.localPosition = inputNodesPos.localPosition;
                else
                    kv.Value.localPosition = GetPosOfNewNode();
            }
        }

        public void DrawRandomFromParameters()
        {
            DrawGenome(RandomGenome(
                newGenomeInputCount,
                newGenomeOutputCout,
                hiddenNodes,
                newGenomeRandomWeight
            ));
        }

        public void DrawGivenGenome()
        {
            if (givenNeuralNet == null)
            {
                Debug.LogError("No genome proxy was given");
                return;
            }
            RemoveAllNodes();
            DrawGenome(givenNeuralNet);
        }

        public void RemoveAllNodes()
        {
            var children = new List<GameObject>();
            foreach (Transform child in nodesParent)
                children.Add(child.gameObject);
            children.ForEach(child =>
            {
                if (Application.isEditor)
                    DestroyImmediate(child.gameObject);
                else
                    Destroy(child);
            });

            nodes.Clear();
            ConnectionManager.CleanConnections();
            nodes.Clear();
        }

        // Create a ne random genome from the given parameters.
        public INeuralGenome RandomGenome(
            int inputs = 2,
            int outputs = 2,
            int hiddenNodes = 5,
            float weight = 10)
        {
            var randGenomeGenerator = new RandNeuralGenomeGeneratorBase(
                RandomInst,
                null,
                weight,
                inputs,
                outputs,
                new int[1] { hiddenNodes },
                true);

            var result = randGenomeGenerator.NewRandomGenome();
            return result as INeuralGenome;
        }

        private void ConnectNode(
            RectTransform target,
            Neuron targetNeuron,
            INeuralGenome genome)
        {
            var outNeurons = genome.Network[targetNeuron]
                                   .Select(x => x.receiver);
            
            foreach (var neuronInnov in outNeurons)
            {
                var neuron = genome.Neurons[neuronInnov];
                if (neuron == targetNeuron)
                    continue;

                var conn = ConnectionManager.CreateConnection(
                    target,
                    nodes[neuron]);

                conn.points[0].weight = 0.6f;
                conn.points[1].weight = 0.6f;
                conn.line.widthMultiplier = connectionWidth;
                conn.points[0].direction =
                        ConnectionPoint.ConnectionDirection.East;
                conn.points[1].direction =
                        ConnectionPoint.ConnectionDirection.West;

                var synapse = genome.Network[targetNeuron]
                                    .First(x => x.receiver == neuronInnov);
                var gene = NeuralUtils.FindGene(genome.Genes, synapse);
                
                var color = GetColor(gene);
                conn.points[0].color = color;
                conn.points[1].color = color;

                if (!synapse.isEnabled)
                    conn.line.widthMultiplier *= 0.3f;
            }
        }

        private Color GetColor(Gene<Synapse> gene)
        {
            Color result;

            if (gene.Val.isEnabled)
            {
                float gradient = Mathf.InverseLerp(-maxWeight,
                                                   maxWeight,
                                                   (float)gene.Val.weight);
                //Debug.Log(string.Format("{0:0.00} -> {1:0.00}", gene.Val.weight, gradient));
                result = Color.Lerp(negativeWeightColor,
                                    positiveWeightColor,
                                    gradient);

                if (gene.Val.weight >= 0)
                    result.a = (float)gene.Val.weight / maxWeight;
                else
                    result.a = (float)-gene.Val.weight / maxWeight;
            }
            else
            {
                result = disabledColor;
            }

            return result;
        }

        private Vector3 GetPosOfNewNode(int tries = 5)
        {
            var result = Vector3.zero;

            do
            {
                tries--;
                result.x = Random.Range(inputNodesPos.localPosition.x,
                                        outputNodesPos.localPosition.x);
                result.y = Random.Range(inputNodesPos.localPosition.y,
                                        bottom.localPosition.y);
            } while (PosIsTooNearToAnything(result) && tries > 0);

            return result;
        }

        private bool PosIsTooNearToAnything(Vector3 pos)
        {
            foreach (var node in nodes)
            {
                var dist = Vector3.Distance(pos, node.Value.localPosition);
                if (dist <= distBetweenNodes)
                    return true;
            }
            return false;
        }

        private float GetAverageWeight(IList<Gene<Synapse>> genes)
        {
            return genes.Average(x => Mathf.Abs((float)x.Val.weight));
        }
    }
}