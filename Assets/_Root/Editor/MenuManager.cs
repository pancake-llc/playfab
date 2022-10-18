﻿using Pancake.GameService;
using UnityEditor;

namespace Pancake.Editor
{
    public static class MenuManager
    {
        [MenuItem("Tools/Pancake/Playfab", false, 1)]
        public static void MenuOpenSettings()
        {
            InvokeCreate();
            PlayfabWindow.ShowWindow();
        }

        internal static void InvokeCreate()
        {
            // Load settings object or create a new one if it doesn't exist.
            var instance = ServiceSettings.LoadSettings();
            if (instance == null) Creator.CreateSettingsAsset();
            var shared = ServiceSettings.GetSharedSettingsObjectPrivate();
            if (shared == null) Creator.CreateSharedSettingsAsset();
        }
    }
}