using Pancake.UI;
using PlayFab.ClientModels;
using UnityEngine;

namespace Pancake.GameService
{
    [RequireComponent(typeof(PlayfabSilentLogin))]
    [DisallowMultipleComponent]
    public class SilentLogin : MonoBehaviour
    {
        private PlayfabSilentLogin _playfabSilentLogin;

        private void Awake()
        {
            _playfabSilentLogin = GetComponent<PlayfabSilentLogin>();
            _playfabSilentLogin.onLoginSuccess.RemoveAllListeners();
            _playfabSilentLogin.onLoginSuccess.AddListener(OnLoginSuccess);
        }

        private void OnLoginSuccess(LoginResult result)
        {
            if (result.NewlyCreated || !AuthService.Instance.IsCompleteSetupName)
            {
                Popup.Show<PopupEnterName>();
            }
            else
            {
                
            }
        }
    }
}