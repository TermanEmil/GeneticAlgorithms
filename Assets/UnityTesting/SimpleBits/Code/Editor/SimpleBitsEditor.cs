using UnityEditor;
using UnityEngine;

using GA.Graph;

namespace GA_Tests.SimpleBits
{
    [CustomEditor(typeof(SimpleBits), true)]
    public class SimpleBitsEditor : Editor
    {
        private SimpleBits instance = null;
        private SimpleBits Instance
        {
            get
            {
                return (instance == null) ? (SimpleBits)target : instance;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!Application.isPlaying)
                return;

            if (GUILayout.Button("Draw best"))
            {
                GAGraph.Instance.givenNeuralNet = Instance.GetBestGenome();
                GAGraph.Instance.DrawGivenGenome();
            }
        }
    }
}