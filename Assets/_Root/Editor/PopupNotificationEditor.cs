using Pancake.GameService;
using Pancake.UI;
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
        private SerializedProperty _btnOk;

        protected override void OnEnable()
        {
            base.OnEnable();
            _txtMessage = serializedObject.FindProperty("txtMessage");
            _btnOk = serializedObject.FindProperty("btnOk");
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
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Ok", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _btnOk.objectReferenceValue = EditorGUILayout.ObjectField(_btnOk.objectReferenceValue, typeof(UIButton), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();
        }
    }
}