using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Events;

namespace Pancake.GameService
{
    public class PlayfabSilentLogin : MonoBehaviour
    {
        public UnityEvent<LoginResult> onLoginSuccess;
        private void OnEnable()
        {
            AuthService.Instance.infoRequestParams = ServiceSettings.InfoRequestParams;
            AuthService.OnLoginSuccess += AuthServiceOnLoginSuccess;
        }

        private void Start() { AuthService.Instance.Authenticate(EAuthType.Silent); }

        private void AuthServiceOnLoginSuccess(LoginResult success)
        {
            // if (success.NewlyCreated)
            // {
            //     // enter name
            // }
            // else
            // {
            //     // goto menu
            // }
            
            onLoginSuccess?.Invoke(success);
        }

        private void OnDisable() { AuthService.OnLoginSuccess -= AuthServiceOnLoginSuccess; }
    }
}