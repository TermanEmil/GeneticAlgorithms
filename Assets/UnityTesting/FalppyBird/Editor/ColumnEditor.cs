using UnityEngine;
using UnityEditor;

namespace GA_Tests.FlappyBird
{
    [CustomEditor(typeof(Column))]
    public class ColumnEditor : Editor
    {
        private Column Target { get { return (Column)target; } }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Target.size = EditorGUILayout.Slider(Target.size, 0, 5);
            Target.SetColumnSize(Target.size);
        }
    }
}