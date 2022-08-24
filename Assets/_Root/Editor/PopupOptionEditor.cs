using Pancake.GameService;
using Pancake.UI;
using Pancake.UI.Editor;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Pancake.Editor
{
    [CustomEditor(typeof(PopupOption))]
    public class PopupOptionEditor : UIPopupEditor
    {
        private SerializedProperty _txtMessage;
        private SerializedProperty _btnOk;
        private SerializedProperty _btnCancel;

        protected override void OnEnable()
        {
            base.OnEnable();
            _txtMessage = serializedObject.FindProperty("txtMessage");
            _btnOk = serializedObject.FindProperty("btnOk");
            _btnCancel = serializedObject.FindProperty("btnCancel");
        }

        protected override void OnDrawExtraSetting()
        {
            Uniform.SpaceOneLine();
            Uniform.DrawUppercaseSection("UIPOPUP_OPTION", "POPUP SETTING", DrawSetting);
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
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Cancel", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _btnCancel.objectReferenceValue = EditorGUILayout.ObjectField(_btnCancel.objectReferenceValue, typeof(UIButton), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();
        }
    }
}