using System;
using Pancake.Editor;
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

        private void AuthServiceOnLoginSuccess(LoginResult success)
        {
            if (success.NewlyCreated)
            {
            }
            else
            {
            }
        }

        private void OnDisable() { AuthService.OnLoginSuccess -= AuthServiceOnLoginSuccess; }
    }
}