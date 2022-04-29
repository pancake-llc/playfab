using UnityEngine;

namespace Pancake.Editor
{
    /// <summary>
    /// bridge setting for PlayfabSharedSettings
    /// </summary>
    public class PlayfabSettings : ScriptableObject
    {
        private static PlayfabSettings instance;

        public static PlayfabSettings Instance
        {
            get
            {
                if (instance != null) return instance;

                instance = LoadSettings();
                if (instance == null)
                {
#if !UNITY_EDITOR
                        Debug.LogError("Playfab settings not found! Please go to menu Tools > Pancake > Playfab to setup the plugin.");
#endif
                    instance = CreateInstance<PlayfabSettings>();
                }
                return instance;
            }
        }

        public static PlayfabSettings LoadSettings() { return Resources.Load<PlayfabSettings>("EditorPlayfabSettings"); }
    }
}