using System;
using System.Collections.Generic;
using System.Globalization;
using MEC;
using Pancake.Common;
using Pancake.Tween;
using Pancake.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

namespace Pancake.GameService
{
    public class PopupLeaderboard : UIPopup
    {
        public enum ELeaderboardTab
        {
            World = 0,
            Country = 1,
            Friend = 2
        }

        private const string LAST_TIME_FETCH_RANK_KEY = "last_time_fetch_rank";
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
        [SerializeField] private AnimationCurve displayRankCurve;
        
        private Data _worldData = new Data("world");
        private Data _countryData = new Data("country");
        private Data _friendData = new Data("friend");
        private Dictionary<string, InternalConfig> _userInternalConfig = new Dictionary<string, InternalConfig>();
        private ELeaderboardTab _currentTab = ELeaderboardTab.World;

        public int CountInOnePage => rankSlots.Length;

        private Color ColorDivision(int rank, string playerId)
        {
            switch (rank)
            {
                case 1: return colorRank1;
                case 2: return colorRank2;
                case 3: return colorRank3;
                default: return playerId.Equals(LoginResultModel.playerId) ? colorHightlight : colorOutRank;
            }
        }

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
                firstTime = true;
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

            if (_worldData.IsCanRefresh(ServiceSettings.delayFetchRank))
            {
                _worldData.firstTime = false;
                _worldData.players.Clear();
                _userInternalConfig.Clear();
                _worldData.LastTimeRefreshLeaderboard = DateTime.UtcNow;
                if (AuthService.Instance.isLoggedIn && AuthService.Instance.isRequestCompleted)
                {
                    // wait if need
                    block.SetActive(true);
                    AuthService.RequestLeaderboard(nameTableLeaderboard, RequestWorldLeaderboardSuccess, RequestWorldLeaderboardError);
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

        private void RequestWorldLeaderboardSuccess(GetLeaderboardResult result)
        {
            if (result == null && _worldData.players.Count == 0) return;

            txtWarning.gameObject.SetActive(false);
            if (result != null) _worldData.players = result.Leaderboard;
            _worldData.pageCount = M.CeilToInt(_worldData.players.Count / (float) CountInOnePage);
            Refresh(_worldData);
        }

        private void Refresh(Data data)
        {
            if (data.currentPage >= data.pageCount) // reach the end
            {
                btnNextPage.gameObject.SetActive(false);
                txtWarning.text = "Nothing to show\nYou have reached the end of the rankings";
                txtWarning.gameObject.SetActive(true);
                block.SetActive(false);
                btnBackPage.gameObject.SetActive(data.currentPage != 0);
                return;
            }

            block.SetActive(true);
            var pageData = new List<PlayerLeaderboardEntry>();
            for (int i = 0; i < CountInOnePage; i++)
            {
                int index = data.currentPage * CountInOnePage + i;
                if (data.players.Count <= index) break;

                pageData.Add(_worldData.players[index]);
            }

            btnBackPage.gameObject.SetActive(data.currentPage != 0);
            btnNextPage.gameObject.SetActive(data.currentPage < data.pageCount && !(data.players.Count < 100 && data.currentPage == data.pageCount - 1));

            content.SetActive(false);
            foreach (var element in rankSlots)
            {
                element.gameObject.SetActive(false);
            }

            FetchInternalConfig(pageData, OnOnePageFetchInternalConfigCompleted);
        }

        private IEnumerator<float> OnOnePageFetchInternalConfigCompleted(List<PlayerLeaderboardEntry> entries, InternalConfig[] internalConfigs)
        {
            block.SetActive(false);
            content.SetActive(true);

            for (int i = 0; i < internalConfigs.Length; i++)
            {
                rankSlots[i]
                .Init(internalConfigs[i],
                    entries[i].Position + 1,
                    countryCode.Get(internalConfigs[i].countryCode).icon,
                    entries[i].DisplayName,
                    entries[i].StatValue,
                    ColorDivision(entries[i].Position + 1, entries[i].PlayFabId),
                    entries[i].PlayFabId.Equals(LoginResultModel.playerId));
                rankSlots[i].gameObject.SetActive(true);
                var sequense = TweenManager.Sequence();
                sequense.Append(rankSlots[i].transform.TweenLocalScale(new Vector3(1.04f, 1.06f, 1), 0.15f).SetEase(Ease.OutQuad));
                sequense.Append(rankSlots[i].transform.TweenLocalScale(Vector3.one, 0.08f).SetEase(Ease.InQuad));
                sequense.Play();
                yield return Timing.WaitForSeconds(displayRankCurve.Evaluate(i / (float) internalConfigs.Length));
            }
        }

        private async void FetchInternalConfig(List<PlayerLeaderboardEntry> entries, Func<List<PlayerLeaderboardEntry>, InternalConfig[], IEnumerator<float>> onCompleted)
        {
            InternalConfig[] configs = new InternalConfig[entries.Count];
            for (int i = 0; i < entries.Count; i++)
            {
                if (!_userInternalConfig.ContainsKey(entries[i].PlayFabId))
                {
                    configs[i] = await AuthService.GetUserData<InternalConfig>(entries[i].PlayFabId,
                        ServiceSettings.INTERNAL_CONFIG_KEY,
                        errorCallback: error => Debug.Log(error.ErrorMessage));
                    _userInternalConfig.Add(entries[i].PlayFabId, configs[i]);
                }
                else
                {
                    configs[i] = _userInternalConfig[entries[i].PlayFabId];
                }
            }

            Timing.RunCoroutine(onCompleted?.Invoke(entries, configs));
        }

        private void OnFriendButtonClicked() { }

        private void OnCountryButtonClicked() { }

        private void OnWorldButtonClicked() { }

        /// <summary>
        /// next page
        /// </summary>
        private void OnNextPageButtonClicked()
        {
            btnNextPage.interactable = false;
            txtWarning.gameObject.SetActive(false);
            switch (_currentTab)
            {
                case ELeaderboardTab.World:
                    _worldData.currentPage++;
                    if (_worldData.currentPage == _worldData.pageCount)
                    {
                        if (_worldData.currentPage * CountInOnePage >= _worldData.players.Count && _worldData.players.Count > 0)
                        {
                            block.SetActive(true);
                            content.SetActive(false);
                            AuthService.RequestLeaderboard(nameTableLeaderboard,
                                NextPageRequestWorldLeaderboardSuccess,
                                NextPageRequestWorldLeaderboardError,
                                _worldData.currentPage * CountInOnePage);
                        }
                    }
                    else
                    {
                        btnNextPage.interactable = true;
                        Refresh(_worldData);
                    }

                    break;
                case ELeaderboardTab.Country:
                    _countryData.currentPage++;
                    break;
                case ELeaderboardTab.Friend:
                    _friendData.currentPage++;
                    break;
            }
        }

        /// <summary>
        /// previous page
        /// </summary>
        private void OnBackPageButtonClicked()
        {
            btnBackPage.interactable = false;
            txtWarning.gameObject.SetActive(false);
            switch (_currentTab)
            {
                case ELeaderboardTab.World:
                    if (_worldData.currentPage > 0)
                    {
                        _worldData.currentPage--;
                        Refresh(_worldData);
                    }

                    break;
                case ELeaderboardTab.Country:
                    if (_countryData.currentPage > 0)
                    {
                        _countryData.currentPage--;
                        Refresh(_countryData);
                    }

                    break;
                case ELeaderboardTab.Friend:
                    if (_friendData.currentPage > 0)
                    {
                        _friendData.currentPage--;
                        Refresh(_friendData);
                    }

                    break;
            }
        }


        #region world

        private void NextPageRequestWorldLeaderboardSuccess(GetLeaderboardResult result)
        {
            btnNextPage.interactable = true;
            if (result == null && _worldData.players.Count == 0) return;

            txtWarning.gameObject.SetActive(false);
            if (result != null) _worldData.players.AddRange(result.Leaderboard);
            _worldData.pageCount = M.CeilToInt(_worldData.players.Count / (float) CountInOnePage);
            Refresh(_worldData);
        }


        private void NextPageRequestWorldLeaderboardError(PlayFabError error) { btnNextPage.interactable = true; }

        #endregion

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
                    "Are you sure you wish to update aggregation for all leaderboard to Maximum?\nThis action cannot be reversed.",
                    "Update",
                    "Cancel"))
            {
                _internalIndex = 0;
                Call();
            }

            void Call()
            {
                var c = countryCode.countryCodeDatas[_internalIndex];
                UnityEditor.EditorUtility.DisplayProgressBar("Update Aggregation Highest Value",
                    $"Updating {c.code.ToString()}...",
                    _internalIndex / (float) countryCode.countryCodeDatas.Length);
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
                            UnityEditor.EditorUtility.ClearProgressBar();
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
                                        UnityEditor.EditorUtility.ClearProgressBar();
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