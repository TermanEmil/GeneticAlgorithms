using UnityEngine;
using System.Collections.Generic;
using GA.NeuralNet.NeuralGenome;

namespace GA.Graph
{
    public class GenomeToDraw : MonoBehaviour
    {
        public IList<INeuralGenome> Genomes { get; set; }
    }
}