using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Pancake.GameService
{
    /// <summary>
    /// bridge setting for PlayfabSharedSettings
    /// </summary>
    public class ServiceSettings : ScriptableObject
    {
        private static ServiceSettings instance;
        private static PlayFabSharedSettings sharedSettings;

        public static ServiceSettings Instance
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
                    instance = CreateInstance<ServiceSettings>();
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

        [SerializeField] private bool useCustomIdAsDefault = true;
        [SerializeField] private bool enableAdminApi;
        [SerializeField] private bool enableClientApi = true;
        [SerializeField] private bool enableEntityApi = true;
        [SerializeField] private bool enableServerApi;
        [SerializeField] private bool enableRequestTimesApi;
        [SerializeField] private GetPlayerCombinedInfoRequestParams infoRequestParams;

        public static bool EnableAdminApi => Instance.enableAdminApi;
        public static bool EnableClientApi => Instance.enableClientApi;
        public static bool EnableEntityApi => Instance.enableEntityApi;
        public static bool EnableServerApi => Instance.enableServerApi;
        public static bool EnableRequestTimesApi => Instance.enableRequestTimesApi;

        public static string TitleId => Instance.titleId;

        public static string SecretKey => Instance.secretKey;

        public static WebRequestType RequestType => Instance.requestType;

        public static GetPlayerCombinedInfoRequestParams InfoRequestParams => Instance.infoRequestParams;

        public static bool UseCustomIdAsDefault => Instance.useCustomIdAsDefault;

        public static ServiceSettings LoadSettings() { return Resources.Load<ServiceSettings>("GameServiceSettings"); }

        #region help

        private const string STORAGE_CURRENT_COUNTRY_KEY = "login_current_country_code";
        public static string GetCurrentCountryCode => PlayerPrefs.GetString(STORAGE_CURRENT_COUNTRY_KEY, "");
        public static void SetCurrentCountryCode(string value) => PlayerPrefs.SetString(STORAGE_CURRENT_COUNTRY_KEY, value.ToUpper());

        private const string STORAGE_CURRENT_NAME_KEY = "login_current_name_code";
        public static string GetCurrentName => PlayerPrefs.GetString(STORAGE_CURRENT_NAME_KEY, "");
        public static void SetCurrentName(string value) => PlayerPrefs.SetString(STORAGE_CURRENT_NAME_KEY, value);

        #endregion
    }
}