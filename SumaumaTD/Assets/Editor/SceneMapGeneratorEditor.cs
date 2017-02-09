using Assets.Scripts;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomEditor(typeof(SceneMapGenerator))]
    public class SceneMapGeneratorEditor :UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SceneMapGenerator script = (SceneMapGenerator)target;
            if (GUILayout.Button("Generate Map"))
            {
                script.GenerateMap();
            }
        }


   
    }
}
