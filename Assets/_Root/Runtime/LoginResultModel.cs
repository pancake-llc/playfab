using UnityEngine;

namespace Pancake.GameService
{
    public static class LoginResultModel
    {
        public static string playerId;
        public static string playerDisplayName;
        public static string countryCode = "US"; // set default country code is US

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Setup() { playerId = ""; }

        public static void Init(string playerId, string playerDisplayName, string countryCode)
        {
            LoginResultModel.playerId = playerId;
            LoginResultModel.playerDisplayName = playerDisplayName;
            LoginResultModel.countryCode = countryCode;
        }
    }
}