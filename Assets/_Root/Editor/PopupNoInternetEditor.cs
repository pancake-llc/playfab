using Pancake.GameService;
using Pancake.UI;
using Pancake.UI.Editor;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Pancake.Editor
{
    [CustomEditor(typeof(PopupNoInternet))]
    public class PopupNoInternetEditor : UIPopupEditor
    {
        private SerializedProperty _btnOk;

        protected override void OnEnable()
        {
            base.OnEnable();
            _btnOk = serializedObject.FindProperty("btnOk");
        }

        protected override void OnDrawExtraSetting()
        {
            Uniform.SpaceOneLine();
            Uniform.DrawUppercaseSection("UIPOPUP_NOINTERNET", "NO INTERNET SETTING", DrawSetting);
        }

        private void DrawSetting()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Ok", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _btnOk.objectReferenceValue = EditorGUILayout.ObjectField(_btnOk.objectReferenceValue, typeof(UIButton), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();
        } 
    }
}
