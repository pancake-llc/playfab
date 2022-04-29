#if UNITY_2017_1_OR_NEWER
using UnityEditor;
using UnityEngine;

public class MakeScriptableObject
{
    //[MenuItem("PlayFab/MakePlayFabSharedSettings")]
    public static void MakePlayFabSharedSettings()
    {
        PlayFabSharedSettings asset = ScriptableObject.CreateInstance<PlayFabSharedSettings>();
        AssetDatabase.CreateAsset(asset, "Assets/Resources/PlayFabSharedSettings.asset"); // TODO: Path should not be hard coded
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
#endif
