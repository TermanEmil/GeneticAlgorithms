using UnityEditor;
using UnityEngine;

namespace GA.Graph
{
    [CustomEditor(typeof(GAGraph))]
    public class GAGraphEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var myTarget = (GAGraph)target;

            bool drawRandom;
            bool reset;
            bool drawGivenGenome;

            GUILayout.BeginHorizontal();
            drawRandom = GUILayout.Button("Draw random");
            drawGivenGenome = GUILayout.Button("Draw given");
            reset = GUILayout.Button("Reset");
            GUILayout.EndHorizontal();

            if (drawRandom)
                myTarget.DrawRandomFromParameters();
            else if (drawGivenGenome)
                myTarget.DrawGivenGenome();
            else if (reset)
                myTarget.RemoveAllNodes();
        }
    }
}