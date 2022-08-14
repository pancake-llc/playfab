using Pancake.GameService;
using Pancake.UI;
using Pancake.UI.Editor;
using TMPro;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using CountryCode = Pancake.GameService.CountryCode;

namespace Pancake.Editor
{
    [CustomEditor(typeof(PopupLeaderboard))]
    public class PopupLeaderboardEditor : UIPopupEditor
    {
        private SerializedProperty _countryCode;
        private SerializedProperty _btnNextPage;
        private SerializedProperty _btnBackPage;
        private SerializedProperty _btnWorld;
        private SerializedProperty _btnCountry;
        private SerializedProperty _btnFriend;
        private SerializedProperty _txtName;
        private SerializedProperty _txtRank;
        private SerializedProperty _txtCurrentPage;
        private SerializedProperty _rankSlots;
        private SerializedProperty _colorRank1;
        private SerializedProperty _colorRank2;
        private SerializedProperty _colorRank3;
        private SerializedProperty _colorOutRank;
        private SerializedProperty _colorHightlight;
        private SerializedProperty _txtWarning;
        private SerializedProperty _block;
        private SerializedProperty _content;
        private SerializedProperty _nameTableLeaderboard;
        private SerializedProperty _displayRankCurve;
        private ReorderableList _rankSlotList;

        protected override void OnEnable()
        {
            base.OnEnable();
            _btnNextPage = serializedObject.FindProperty("btnNextPage");
            _btnBackPage = serializedObject.FindProperty("btnBackPage");
            _countryCode = serializedObject.FindProperty("countryCode");
            _btnWorld = serializedObject.FindProperty("btnWorld");
            _btnCountry = serializedObject.FindProperty("btnCountry");
            _btnFriend = serializedObject.FindProperty("btnFriend");
            _txtName = serializedObject.FindProperty("txtName");
            _txtRank = serializedObject.FindProperty("txtRank");
            _txtCurrentPage = serializedObject.FindProperty("txtCurrentPage");
            _rankSlots = serializedObject.FindProperty("rankSlots");
            _colorRank1 = serializedObject.FindProperty("colorRank1");
            _colorRank2 = serializedObject.FindProperty("colorRank2");
            _colorRank3 = serializedObject.FindProperty("colorRank3");
            _colorOutRank = serializedObject.FindProperty("colorOutRank");
            _colorHightlight = serializedObject.FindProperty("colorHightlight");
            _txtWarning = serializedObject.FindProperty("txtWarning");
            _block = serializedObject.FindProperty("block");
            _content = serializedObject.FindProperty("content");
            _nameTableLeaderboard = serializedObject.FindProperty("nameTableLeaderboard");
            _displayRankCurve = serializedObject.FindProperty("displayRankCurve");

            _rankSlotList = new ReorderableList(serializedObject,
                _rankSlots,
                true,
                true,
                true,
                true);
            _rankSlotList.drawElementCallback = DrawListRankItem;
            _rankSlotList.drawHeaderCallback = DrawRankHeader;
        }

        private void DrawRankHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Rank Slots");
        }

        private void DrawListRankItem(Rect rect, int index, bool isactive, bool isfocused)
        {
            SerializedProperty element = _rankSlotList.serializedProperty.GetArrayElementAtIndex(index); //The element in the list
            EditorGUI.PropertyField(rect, element, new GUIContent(element.displayName), element.isExpanded);
        }

        protected override void OnDrawExtraSetting()
        {
            Uniform.SpaceOneLine();
            Uniform.DrawUppercaseSection("UIPOPUP_LEADERBOARD", "LEADERBOARD SETTING", DrawSetting);
        }

        private void DrawSetting()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Country Code", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _countryCode.objectReferenceValue = EditorGUILayout.ObjectField(_countryCode.objectReferenceValue, typeof(CountryCode), allowSceneObjects: false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Next Page", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _btnNextPage.objectReferenceValue = EditorGUILayout.ObjectField(_btnNextPage.objectReferenceValue, typeof(UIButton), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Back Page", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _btnBackPage.objectReferenceValue = EditorGUILayout.ObjectField(_btnBackPage.objectReferenceValue, typeof(UIButton), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Button World", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _btnWorld.objectReferenceValue = EditorGUILayout.ObjectField(_btnWorld.objectReferenceValue, typeof(UIButton), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Button Country", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _btnCountry.objectReferenceValue = EditorGUILayout.ObjectField(_btnCountry.objectReferenceValue, typeof(UIButton), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Button Friend", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _btnFriend.objectReferenceValue = EditorGUILayout.ObjectField(_btnFriend.objectReferenceValue, typeof(UIButton), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Name Text", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _txtName.objectReferenceValue = EditorGUILayout.ObjectField(_txtName.objectReferenceValue, typeof(TextMeshProUGUI), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Rank Text", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _txtRank.objectReferenceValue = EditorGUILayout.ObjectField(_txtRank.objectReferenceValue, typeof(TextMeshProUGUI), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Page Text", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _txtCurrentPage.objectReferenceValue = EditorGUILayout.ObjectField(_txtCurrentPage.objectReferenceValue, typeof(TextMeshProUGUI), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

            _rankSlotList.DoLayoutList();
            
            _colorRank1 = serializedObject.FindProperty("colorRank1");
            _colorRank2 = serializedObject.FindProperty("colorRank2");
            _colorRank3 = serializedObject.FindProperty("colorRank3");
            _colorOutRank = serializedObject.FindProperty("colorOutRank");
            _colorHightlight = serializedObject.FindProperty("colorHightlight");
            _txtWarning = serializedObject.FindProperty("txtWarning");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Color Rank 1", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _colorRank1.colorValue = EditorGUILayout.ColorField(_colorRank1.colorValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Color Rank 2", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _colorRank2.colorValue = EditorGUILayout.ColorField(_colorRank2.colorValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Color Rank 3", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _colorRank3.colorValue = EditorGUILayout.ColorField(_colorRank3.colorValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Color Out Rank", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _colorOutRank.colorValue = EditorGUILayout.ColorField(_colorOutRank.colorValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Color Hightlight", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _colorHightlight.colorValue = EditorGUILayout.ColorField(_colorHightlight.colorValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Warning Text", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _txtWarning.objectReferenceValue = EditorGUILayout.ObjectField(_txtWarning.objectReferenceValue, typeof(TextMeshProUGUI), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Block", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _block.objectReferenceValue = EditorGUILayout.ObjectField(_block.objectReferenceValue, typeof(GameObject), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Content", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _content.objectReferenceValue = EditorGUILayout.ObjectField(_content.objectReferenceValue, typeof(GameObject), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Table Name", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _nameTableLeaderboard.stringValue = EditorGUILayout.TextField(_nameTableLeaderboard.stringValue);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Curve", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _displayRankCurve.animationCurveValue = EditorGUILayout.CurveField(_displayRankCurve.animationCurveValue);
            EditorGUILayout.EndHorizontal();
        }
    }
}