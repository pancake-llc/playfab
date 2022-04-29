using UnityEditor;
using UnityEngine;

namespace Pancake.Editor
{
    [CustomEditor(typeof(PlayfabSettings))]
    public class PlayfabEditor : UnityEditor.Editor
    {
        public static bool callFromEditorWindow = false;
        private void Init() { }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Init();
            if (!callFromEditorWindow)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox(
                    "This ScriptableObject holds all the settings of Playfab. Please go to menu Tools > Pancake > Playfab or click the button below to edit it.",
                    MessageType.Info);
                if (GUILayout.Button("Edit")) PlayfabWindow.ShowWindow();
                return;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}