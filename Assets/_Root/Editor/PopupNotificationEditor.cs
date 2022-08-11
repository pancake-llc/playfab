using Pancake.GameService;
using Pancake.UI.Editor;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Pancake.Editor
{
    [CustomEditor(typeof(PopupNotification))]
    public class PopupNotificationEditor : UIPopupEditor
    {
        private SerializedProperty _txtMessage;

        protected override void OnEnable()
        {
            base.OnEnable();
            _txtMessage = serializedObject.FindProperty("txtMessage");
        }

        protected override void OnDrawExtraSetting()
        {
            Uniform.SpaceOneLine();
            Uniform.DrawUppercaseSection("UIPOPUP_NOTIFICATION", "NOTIFICATION SETTING", DrawSetting);
        }

        private void DrawSetting()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Message", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _txtMessage.objectReferenceValue = EditorGUILayout.ObjectField(_txtMessage.objectReferenceValue, typeof(TextMeshProUGUI), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();
        }
    }
}