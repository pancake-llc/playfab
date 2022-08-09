using Pancake.GameService;
using Pancake.UI;
using Pancake.UI.Editor;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Pancake.Editor
{
    [CustomEditor(typeof(PopupEnterName))]
    public class PopupEnterNameEditor : UIPopupEditor
    {
        private SerializedProperty _elementPrefab;
        private SerializedProperty _countryCode;
        private SerializedProperty _btnSpriteLocked;

        protected override void OnEnable()
        {
            base.OnEnable();
            _elementPrefab = serializedObject.FindProperty("elementPrefab");
            _countryCode = serializedObject.FindProperty("countryCode");
            _btnSpriteLocked = serializedObject.FindProperty("btnSpriteLocked");
        }

        protected override void OnDrawExtraSetting()
        {
            Uniform.SpaceOneLine();
            Uniform.DrawUppercaseSection("UIPOPUP_LOGIN", "LOGIN SETTING", DrawSetting);
        }

        private void DrawSetting()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Prefab", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _elementPrefab.objectReferenceValue = EditorGUILayout.ObjectField(_elementPrefab.objectReferenceValue, typeof(CountryView), allowSceneObjects: false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Country Code", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _countryCode.objectReferenceValue = EditorGUILayout.ObjectField(_countryCode.objectReferenceValue, typeof(CountryCode), allowSceneObjects: false);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Sprite Lock", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _btnSpriteLocked.objectReferenceValue = EditorGUILayout.ObjectField(_btnSpriteLocked.objectReferenceValue, typeof(Sprite), allowSceneObjects: false);
            EditorGUILayout.EndHorizontal();
        }
    }
}