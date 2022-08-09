using System;
using System.Collections.Generic;
using Pancake.UI;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

namespace Pancake.GameService
{
    public class PopupLeaderboard : UIPopup
    {
        [SerializeField] private CountryCode countryCode;
        [SerializeField] private UIButton btnNextPage;
        [SerializeField] private UIButton btnBackPage;
        [SerializeField] private UIButton btnWorld;
        [SerializeField] private UIButton btnCountry;
        [SerializeField] private UIButton btnFriend;
        [SerializeField] private TextMeshProUGUI txtName;
        [SerializeField] private TextMeshProUGUI txtRank;
        [SerializeField] private TextMeshProUGUI txtCurrentPage;
        [SerializeField] private GameObject content;
        [SerializeField] private LeaderboardElement rankSlot1;
        [SerializeField] private LeaderboardElement rankSlot2;
        [SerializeField] private LeaderboardElement rankSlot3;
        [SerializeField] private LeaderboardElement rankSlot4;
        [SerializeField] private LeaderboardElement rankSlot5;
        [SerializeField] private LeaderboardElement rankSlot6;
        [SerializeField] private LeaderboardElement rankSlot7;
        [SerializeField] private LeaderboardElement rankSlot8;
        [SerializeField] private LeaderboardElement rankSlot9;
        [SerializeField] private LeaderboardElement rankSlot10;
        [SerializeField] private Color colorRank1 = new Color(1f, 0.8f, 0.25f, 1f);
        [SerializeField] private Color colorRank2 = new Color(0.65f, 0.75f, 0.84f, 1f);
        [SerializeField] private Color colorRank3 = new Color(0.75f, 0.58f, 0.38f, 1f);
        [SerializeField] private Color colorOutRank = new Color(0.86f, 0.84f, 0.69f, 1f);
        [SerializeField] private Color colorHightlight = new Color(0.57f, 0.85f, 0.63f, 1f);
        [SerializeField] private TextMeshProUGUI txtWarning;
        [SerializeField] private GameObject block;

        [SerializeField] private DateTime _lastTimeRefreshLeaderboard;

        private Data _worldData;

        private class Data
        {
            public int currentPage;
            public List<PlayerLeaderboardEntry> players;
        }

        private void OnEnable()
        {
            content.SetActive(false);
            btnBackPage.onClick.AddListener(OnBackPageButtonClicked);
            btnNextPage.onClick.AddListener(OnNextPageButtonClicked);
            btnWorld.onClick.AddListener(OnWorldButtonClicked);
            btnCountry.onClick.AddListener(OnCountryButtonClicked);
            btnFriend.onClick.AddListener(OnFriendButtonClicked);
        }

        private void OnFriendButtonClicked() { }

        private void OnCountryButtonClicked() { }

        private void OnWorldButtonClicked() { }

        private void OnNextPageButtonClicked() { }

        private void OnBackPageButtonClicked() { }

        private void OnDisable()
        {
            btnBackPage.onClick.RemoveListener(OnBackPageButtonClicked);
            btnNextPage.onClick.RemoveListener(OnNextPageButtonClicked);
            btnWorld.onClick.RemoveListener(OnWorldButtonClicked);
            btnCountry.onClick.RemoveListener(OnCountryButtonClicked);
            btnFriend.onClick.RemoveListener(OnFriendButtonClicked);
        }
    }
}