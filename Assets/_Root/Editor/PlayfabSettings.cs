using PlayFab;
using UnityEngine;

namespace Pancake.Editor
{
    /// <summary>
    /// bridge setting for PlayfabSharedSettings
    /// </summary>
    public class PlayfabSettings : ScriptableObject
    {
        private static PlayfabSettings instance;
        private static PlayFabSharedSettings sharedSettings;

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

        public static PlayFabSharedSettings SharedSettings
        {
            get
            {
                if (sharedSettings != null) return sharedSettings;
                sharedSettings = GetSharedSettingsObjectPrivate();
                if (sharedSettings == null)
                {
#if !UNITY_EDITOR
                        Debug.LogError("Playfab settings not found! Please go to menu Tools > Pancake > Playfab to setup the plugin.");
#endif
                    sharedSettings = CreateInstance<PlayFabSharedSettings>();
                }
                return sharedSettings;
            }
        }

        public static PlayFabSharedSettings GetSharedSettingsObjectPrivate() { return Resources.Load<PlayFabSharedSettings>("PlayFabSharedSettings"); }


        [SerializeField] private string titleId;
        [SerializeField] private string secretKey;
        [SerializeField] private WebRequestType requestType = WebRequestType.UnityWebRequest;

        [SerializeField] private bool enableAdminApi;
        [SerializeField] private bool enableClientApi = true;
        [SerializeField] private bool enableEntityApi = true;
        [SerializeField] private bool enableServerApi;
        [SerializeField] private bool enableRequestTimesApi;

        public static bool EnableAdminApi => Instance.enableAdminApi;
        public static bool EnableClientApi => Instance.enableClientApi;
        public static bool EnableEntityApi => Instance.enableEntityApi;
        public static bool EnableServerApi => Instance.enableServerApi;
        public static bool EnableRequestTimesApi => Instance.enableRequestTimesApi;

        public static string TitleId => Instance.titleId;

        public static string SecretKey => Instance.secretKey;

        public static WebRequestType RequestType => Instance.requestType;

        public static PlayfabSettings LoadSettings() { return Resources.Load<PlayfabSettings>("EditorPlayfabSettings"); }
    }
}