using System.IO;
using Pancake.GameService;
using UnityEditor;
using UnityEngine;

namespace Pancake.Editor
{
    public static class Creator
    {
        internal static ServiceSettings CreateSettingsAsset()
        {
            // Stop if the asset is already created.
            var instance = ServiceSettings.LoadSettings();
            if (instance != null) return instance;

            // Now create the asset inside the Resources folder.
            instance = UnityEngine.ScriptableObject.CreateInstance<ServiceSettings>();
            AssetDatabase.CreateAsset(instance,  $"{InEditor.DefaultResourcesPath()}/GameServiceSettings.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Settings was created at {InEditor.DefaultResourcesPath()}/GameServiceSettings.asset");

            return instance;
        }

        internal static PlayFabSharedSettings CreateSharedSettingsAsset()
        {
            // Stop if the asset is already created.
            var instance = ServiceSettings.GetSharedSettingsObjectPrivate();
            if (instance != null) return instance;

            // Now create the asset inside the Resources folder.
            instance = UnityEngine.ScriptableObject.CreateInstance<PlayFabSharedSettings>();
            AssetDatabase.CreateAsset(instance, $"{InEditor.DefaultResourcesPath()}/PlayFabSharedSettings.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Settings was created at {InEditor.DefaultResourcesPath()}/PlayFabSharedSettings.asset");

            return instance;
        }

        /// <summary>
        /// Creates the folder if it doesn't exist.
        /// </summary>
        /// <param name="path">Path - the slashes will be corrected.</param>
        private static void EnsureFolderExists(string path)
        {
            path = SlashesToPlatformSeparator(path);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Replaces / in file path to be the os specific separator.
        /// </summary>
        /// <returns>The path.</returns>
        /// <param name="path">Path with correct separators.</param>
        public static string SlashesToPlatformSeparator(string path) { return path.Replace("/", Path.DirectorySeparatorChar.ToString()); }
    }
}