using Pancake.GameService;
using Pancake.UI;
using Pancake.UI.Editor;
using TMPro;
using UnityEditor;
using UnityEngine;

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
        private SerializedProperty _rankSlot1;
        private SerializedProperty _rankSlot2;
        private SerializedProperty _rankSlot3;
        private SerializedProperty _rankSlot4;
        private SerializedProperty _rankSlot5;
        private SerializedProperty _rankSlot6;
        private SerializedProperty _rankSlot7;
        private SerializedProperty _rankSlot8;
        private SerializedProperty _rankSlot9;
        private SerializedProperty _rankSlot10;
        private SerializedProperty _colorRank1;
        private SerializedProperty _colorRank2;
        private SerializedProperty _colorRank3;
        private SerializedProperty _colorOutRank;
        private SerializedProperty _colorHightlight;
        private SerializedProperty _txtWarning;
        private SerializedProperty _block;
        private SerializedProperty _content;

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
            _rankSlot1 = serializedObject.FindProperty("rankSlot1");
            _rankSlot2 = serializedObject.FindProperty("rankSlot2");
            _rankSlot3 = serializedObject.FindProperty("rankSlot3");
            _rankSlot4 = serializedObject.FindProperty("rankSlot4");
            _rankSlot5 = serializedObject.FindProperty("rankSlot5");
            _rankSlot6 = serializedObject.FindProperty("rankSlot6");
            _rankSlot7 = serializedObject.FindProperty("rankSlot7");
            _rankSlot8 = serializedObject.FindProperty("rankSlot8");
            _rankSlot9 = serializedObject.FindProperty("rankSlot9");
            _rankSlot10 = serializedObject.FindProperty("rankSlot10");
            _colorRank1 = serializedObject.FindProperty("colorRank1");
            _colorRank2 = serializedObject.FindProperty("colorRank2");
            _colorRank3 = serializedObject.FindProperty("colorRank3");
            _colorOutRank = serializedObject.FindProperty("colorOutRank");
            _colorHightlight = serializedObject.FindProperty("colorHightlight");
            _txtWarning = serializedObject.FindProperty("txtWarning");
            _block = serializedObject.FindProperty("block");
            _content = serializedObject.FindProperty("content");
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

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Rank 1", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _rankSlot1.objectReferenceValue = EditorGUILayout.ObjectField(_rankSlot1.objectReferenceValue, typeof(LeaderboardElement), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Rank 2", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _rankSlot2.objectReferenceValue = EditorGUILayout.ObjectField(_rankSlot2.objectReferenceValue, typeof(LeaderboardElement), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Rank 3", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _rankSlot3.objectReferenceValue = EditorGUILayout.ObjectField(_rankSlot3.objectReferenceValue, typeof(LeaderboardElement), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Rank 4", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _rankSlot4.objectReferenceValue = EditorGUILayout.ObjectField(_rankSlot4.objectReferenceValue, typeof(LeaderboardElement), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Rank 5", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _rankSlot5.objectReferenceValue = EditorGUILayout.ObjectField(_rankSlot5.objectReferenceValue, typeof(LeaderboardElement), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Rank 6", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _rankSlot6.objectReferenceValue = EditorGUILayout.ObjectField(_rankSlot6.objectReferenceValue, typeof(LeaderboardElement), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Rank 7", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _rankSlot7.objectReferenceValue = EditorGUILayout.ObjectField(_rankSlot7.objectReferenceValue, typeof(LeaderboardElement), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Rank 8", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _rankSlot8.objectReferenceValue = EditorGUILayout.ObjectField(_rankSlot8.objectReferenceValue, typeof(LeaderboardElement), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Rank 9", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _rankSlot9.objectReferenceValue = EditorGUILayout.ObjectField(_rankSlot9.objectReferenceValue, typeof(LeaderboardElement), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Rank 10", GUILayout.Width(DEFAULT_LABEL_WIDTH));
            _rankSlot10.objectReferenceValue = EditorGUILayout.ObjectField(_rankSlot10.objectReferenceValue, typeof(LeaderboardElement), allowSceneObjects: true);
            EditorGUILayout.EndHorizontal();

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
        }
    }
}