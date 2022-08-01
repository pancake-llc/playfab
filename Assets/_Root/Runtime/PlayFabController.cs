using System;
using PlayFab.ClientModels;
using UnityEngine;

namespace Pancake.GameService
{
    public class PlayFabController : MonoBehaviour
    {
        private void OnEnable()
        {
            AuthService.Instance.infoRequestParams = ServiceSettings.InfoRequestParams;
            AuthService.OnLoginSuccess += AuthServiceOnLoginSuccess;
        }

        private void Start() { AuthService.Instance.Authenticate(EAuthType.Silent); }

        private void AuthServiceOnLoginSuccess(LoginResult success)
        {
            if (success.NewlyCreated)
            {
                // enter name
            }
            else
            {
                // goto menu
            }
        }

        private void OnDisable() { AuthService.OnLoginSuccess -= AuthServiceOnLoginSuccess; }
    }
}