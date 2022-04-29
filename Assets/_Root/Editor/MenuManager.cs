using UnityEditor;

namespace Pancake.Editor
{
    public static class MenuManager
    {
        [MenuItem("Tools/Pancake/Playfab &4", false, 1)]
        public static void MenuOpenSettings()
        {
            // Load settings object or create a new one if it doesn't exist.
            var instance = PlayfabSettings.LoadSettings();
            if (instance == null) Creator.CreateSettingsAsset();
            var shared = PlayfabSettings.GetSharedSettingsObjectPrivate();
            if (shared == null) Creator.CreateSharedSettingsAsset();
            PlayfabWindow.ShowWindow();
        }
    }
}