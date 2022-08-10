using System;
using System.Collections.Generic;
using System.Globalization;
using Pancake.Common;
using Pancake.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

namespace Pancake.GameService
{
    public class PopupLeaderboard : UIPopup
    {
        private const string LAST_TIME_FETCH_RANK_KEY = "last_time_fetch_rank";
        public static int delayFetchRank = 180;
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
        [SerializeField] private LeaderboardElement[] rankSlots;
        [SerializeField] private Color colorRank1 = new Color(1f, 0.8f, 0.25f, 1f);
        [SerializeField] private Color colorRank2 = new Color(0.65f, 0.75f, 0.84f, 1f);
        [SerializeField] private Color colorRank3 = new Color(0.75f, 0.58f, 0.38f, 1f);
        [SerializeField] private Color colorOutRank = new Color(0.86f, 0.84f, 0.69f, 1f);
        [SerializeField] private Color colorHightlight = new Color(0.57f, 0.85f, 0.63f, 1f);
        [SerializeField] private TextMeshProUGUI txtWarning;
        [SerializeField] private GameObject block;
        [SerializeField] private string nameTableLeaderboard;


        private Data _worldData = new Data("world");
        private Data _countryData = new Data("country");
        private Data _friendData = new Data("friend");

        public int CountInOnePage => rankSlots.Length;

        private sealed class Data
        {
            public int currentPage;
            public List<PlayerLeaderboardEntry> players;
            public bool firstTime;
            public int pageCount;
            private readonly string _key;

            public Data(string key)
            {
                _key = key;
                firstTime = false;
                players = new List<PlayerLeaderboardEntry>();
                currentPage = 0;
                pageCount = 0;
            }

            public DateTime LastTimeRefreshLeaderboard
            {
                get
                {
                    DateTime.TryParse(PlayerPrefs.GetString($"{LAST_TIME_FETCH_RANK_KEY}_{_key}", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                        out var result);
                    return result;
                }
                set => PlayerPrefs.SetString($"{LAST_TIME_FETCH_RANK_KEY}_{_key}", value.ToString(CultureInfo.InvariantCulture));
            }

            public bool IsCanRefresh(int limitTime) => (DateTime.UtcNow - LastTimeRefreshLeaderboard).TotalSeconds >= limitTime || firstTime;
        }

        private void OnEnable()
        {
            content.SetActive(false);
            btnBackPage.onClick.AddListener(OnBackPageButtonClicked);
            btnNextPage.onClick.AddListener(OnNextPageButtonClicked);
            btnWorld.onClick.AddListener(OnWorldButtonClicked);
            btnCountry.onClick.AddListener(OnCountryButtonClicked);
            btnFriend.onClick.AddListener(OnFriendButtonClicked);

            if (_worldData.IsCanRefresh(delayFetchRank))
            {
                _worldData.firstTime = false;
                _worldData.players.Clear();
                _worldData.LastTimeRefreshLeaderboard = DateTime.UtcNow;
                if (AuthService.Instance.isLoggedIn && AuthService.Instance.isRequestCompleted)
                {
                    AuthService.RequestLeaderboard(nameTableLeaderboard, RequestWorldLeaderboardCallback, RequestWorldLeaderboardError);
                }
                else
                {
                }
            }
            else
            {
                // display with old data
            }
        }

        private void RequestWorldLeaderboardError(PlayFabError error) { }

        private void RequestWorldLeaderboardCallback(GetLeaderboardResult result)
        {
            if (result == null && _worldData.players.Count == 0) return;

            txtWarning.gameObject.SetActive(false);
            if (result != null) _worldData.players = result.Leaderboard;
            _worldData.pageCount = M.CeilToInt(_worldData.players.Count / (float) CountInOnePage);
        }

        private void Refresh(Data data)
        {
            if (data.currentPage >= data.pageCount) // reach the end
            {
                btnNextPage.gameObject.SetActive(false);
                txtWarning.text = "Nothing to show\nYou have reached the end of the rankings";
                txtWarning.gameObject.SetActive(true);
                block.gameObject.SetActive(false);
                btnBackPage.gameObject.SetActive(data.currentPage != 0);
                return;
            }

            var pageData = new List<PlayerLeaderboardEntry>();
            for (int i = 0; i < CountInOnePage; i++)
            {
                int index = data.currentPage * CountInOnePage + i;
                if (data.players.Count <= index) break;

                pageData.Add(_worldData.players[index]);
            }

            btnBackPage.gameObject.SetActive(data.currentPage != 0);
            btnNextPage.gameObject.SetActive(data.currentPage < data.pageCount);

            content.SetActive(false);
            foreach (var element in rankSlots)
            {
                element.gameObject.SetActive(false);
            }

            content.SetActive(true);
            block.SetActive(true);
            for (int i = 0; i < pageData.Count; i++)
            {
                //pageData[i].Profile.Locations
                //rankSlots[i].Init(pageData[i].Position + 1, countryCode.Get());
            }
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

#if UNITY_EDITOR

        private int _internalIndex = 0;
        [ContextMenu("Update Aggregation")]
        public void CreateOrUpdateAggregationLeaderboard()
        {
            if (UnityEditor.EditorUtility.DisplayDialog("Update Aggregation All Leaderboard",
                    "Are you sure you wish to update aggregation for all leaderboard?\nThis action cannot be reversed.",
                    "Update",
                    "Cancel"))
            {
                _internalIndex = 0;
                Call();
            }

            void Call()
            {
                var c = countryCode.countryCodeDatas[_internalIndex];
                PlayFabAdminAPI.CreatePlayerStatisticDefinition(new PlayFab.AdminModels.CreatePlayerStatisticDefinitionRequest()
                    {
                        StatisticName = $"{nameTableLeaderboard}_{c.code.ToString()}",
                        AggregationMethod = PlayFab.AdminModels.StatisticAggregationMethod.Max,
                        VersionChangeInterval = PlayFab.AdminModels.StatisticResetIntervalOption.Never
                    },
                    _ =>
                    {
                        if (_internalIndex < countryCode.countryCodeDatas.Length - 1)
                        {
                            _internalIndex++;
                            Call();
                        }
                        else
                        {
                            Debug.Log("Update Aggregation Completed!");
                        }
                    },
                    error =>
                    {
                        if (error.Error == PlayFabErrorCode.StatisticNameConflict)
                        {
                            PlayFabAdminAPI.UpdatePlayerStatisticDefinition(new PlayFab.AdminModels.UpdatePlayerStatisticDefinitionRequest()
                                {
                                    StatisticName = $"{nameTableLeaderboard}_{c.code.ToString()}",
                                    AggregationMethod = PlayFab.AdminModels.StatisticAggregationMethod.Max,
                                    VersionChangeInterval = PlayFab.AdminModels.StatisticResetIntervalOption.Never
                                },
                                _ =>
                                {
                                    if (_internalIndex < countryCode.countryCodeDatas.Length - 1)
                                    {
                                        _internalIndex++;
                                        Call();
                                    }
                                    else
                                    {
                                        Debug.Log("Update Aggregation Completed!");
                                    }
                                },
                                fabError => { Debug.LogError(fabError.Error); });
                        }
                        else
                        {
                            Debug.LogError(error.Error);
                        }
                    });
            }
        }
#endif
    }
}