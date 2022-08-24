using System;
using System.Collections.Generic;
using System.Globalization;
using MEC;
using Pancake.Common;
using Pancake.Tween;
using Pancake.UI;
using PlayFab;
using PlayFab.ServerModels;
using TMPro;
using UnityEngine;
using GetLeaderboardResult = PlayFab.ClientModels.GetLeaderboardResult;
using PlayerLeaderboardEntry = PlayFab.ClientModels.PlayerLeaderboardEntry;

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

        [Serializable]
        public class ElementColor
        {
            public Color colorBackground;
            public Color colorOverlay;
            public Color colorBoder;
            public Color colorHeader;
            public Color colorText;

            public ElementColor(Color colorBackground, Color colorOverlay, Color colorBoder, Color colorHeader, Color colorText)
            {
                this.colorBackground = colorBackground;
                this.colorOverlay = colorOverlay;
                this.colorBoder = colorBoder;
                this.colorHeader = colorHeader;
                this.colorText = colorText;
            }

            public ElementColor()
            {
                colorBackground = new Color(0.99f, 0.96f, 0.82f);
                colorOverlay = new Color(0.8f, 0.66f, 0.33f);
                colorBoder = new Color(0.99f, 0.96f, 0.82f);
                colorHeader = new Color(1f, 0.67f, 0.26f);
                colorText = new Color(0.68f, 0.3f, 0.01f);
            }
        }

        private const string LAST_TIME_FETCH_RANK_KEY = "last_time_fetch_rank";
        [SerializeField] private CountryCode countryCode;
        [SerializeField] private UIButton btnNextPage;
        [SerializeField] private UIButton btnBackPage;
        [SerializeField] private UIButtonTMP btnWorld;
        [SerializeField] private UIButtonTMP btnCountry;
        [SerializeField] private UIButtonTMP btnFriend;
        [SerializeField] private TextMeshProUGUI txtName;
        [SerializeField] private TextMeshProUGUI txtRank;
        [SerializeField] private TextMeshProUGUI txtCurrentPage;
        [SerializeField] private GameObject content;
        [SerializeField] private LeaderboardElement[] rankSlots;

        [SerializeField] private ElementColor colorRank1 = new ElementColor(new Color(1f, 0.82f, 0f),
            new Color(0.44f, 0.33f, 0f),
            new Color(0.99f, 0.96f, 0.82f),
            new Color(1f, 0.55f, 0.01f),
            new Color(0.47f, 0.31f, 0f));

        [SerializeField] private ElementColor colorRank2 = new ElementColor(new Color(0.79f, 0.84f, 0.91f),
            new Color(0.29f, 0.4f, 0.6f),
            new Color(0.94f, 0.94f, 0.94f),
            new Color(0.45f, 0.54f, 0.56f),
            new Color(0.18f, 0.31f, 0.48f));

        [SerializeField] private ElementColor colorRank3 = new ElementColor(new Color(0.8f, 0.59f, 0.31f),
            new Color(0.34f, 0.23f, 0.09f),
            new Color(1f, 0.82f, 0.57f),
            new Color(0.3f, 0.22f, 0.12f),
            new Color(0.4f, 0.25f, 0.1f));

        [SerializeField] private ElementColor colorRankYou = new ElementColor(new Color(0.47f, 0.76f, 0.92f),
            new Color(0.08f, 0.53f, 0.71f),
            new Color(0.09f, 0.53f, 0.71f),
            new Color(0.22f, 0.58f, 0.85f),
            new Color(0.08f, 0.27f, 0.42f));

        [SerializeField] private ElementColor colorOutRank = new ElementColor();

        [SerializeField] private TextMeshProUGUI txtWarning;
        [SerializeField] private GameObject block;
        [SerializeField] private string nameTableLeaderboard;
        [SerializeField] private AnimationCurve displayRankCurve;
        [SerializeField] private Sprite spriteTabSelected;
        [SerializeField] private Sprite spriteTabNormal;
        [SerializeField] private Color colorTabTextSelected;
        [SerializeField] private Color colorTabTextNormal;

        private Data _worldData = new Data("world");
        private Data _countryData = new Data("country");
        private Data _friendData = new Data("friend");
        private Dictionary<string, InternalConfig> _userInternalConfig = new Dictionary<string, InternalConfig>();
        private ELeaderboardTab _currentTab = ELeaderboardTab.World;
        private bool _sessionFirstTime;
        private HandleIconFacebook _handleIconFacebook;

        public HandleIconFacebook HandleIconFacebook
        {
            get
            {
                if (_handleIconFacebook == null) _handleIconFacebook = btnFriend.GetComponent<HandleIconFacebook>();
                return _handleIconFacebook;
            }
        }

        public int CountInOnePage => rankSlots.Length;

        private ElementColor ColorDivision(int rank, string playerId)
        {
            switch (rank)
            {
                case 1: return colorRank1;
                case 2: return colorRank2;
                case 3: return colorRank3;
                default: return playerId.Equals(LoginResultModel.playerId) ? colorRankYou : colorOutRank;
            }
        }

        private sealed class Data
        {
            public int currentPage;
            public List<PlayerLeaderboardEntry> players;
            public bool firstTime;
            public int pageCount;
            public int myPosition;
            private readonly string _key;

            public Data(string key)
            {
                _key = key;
                firstTime = true;
                players = new List<PlayerLeaderboardEntry>();
                currentPage = 0;
                pageCount = 0;
                myPosition = -1;
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
            btnBackPage.onClick.AddListener(OnBackPageButtonClicked);
            btnNextPage.onClick.AddListener(OnNextPageButtonClicked);
            btnWorld.onClick.AddListener(OnWorldButtonClicked);
            btnCountry.onClick.AddListener(OnCountryButtonClicked);
            btnFriend.onClick.AddListener(OnFriendButtonClicked);
            txtName.text = LoginResultModel.playerDisplayName;

            if (!_sessionFirstTime)
            {
                _sessionFirstTime = true;
                _userInternalConfig.Clear();
            }

            _currentTab = ELeaderboardTab.World;
            WorldButtonInvokeImpl();
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

                pageData.Add(data.players[index]);
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
                    Canvas,
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

        private void OnFriendButtonClicked()
        {
            if (_currentTab == ELeaderboardTab.Friend) return;
            _currentTab = ELeaderboardTab.Friend;
            UpdateDisplayTab();
            content.SetActive(false);
        }

        private void OnCountryButtonClicked()
        {
            if (_currentTab == ELeaderboardTab.Country) return;
            _currentTab = ELeaderboardTab.Country;
            UpdateDisplayTab();
            content.SetActive(false);
            if (_countryData.IsCanRefresh(ServiceSettings.delayFetchRank))
            {
                _countryData.firstTime = false;
                _countryData.players.Clear();
                _countryData.LastTimeRefreshLeaderboard = DateTime.UtcNow;
                if (AuthService.Instance.isLoggedIn && AuthService.Instance.isRequestCompleted)
                {
                    block.SetActive(true);
                    AuthService.GetMyPosition(LoginResultModel.playerId,
                        $"{nameTableLeaderboard}_{LoginResultModel.countryCode}",
                        OnGetLeaderboardAroundUserCountrySuccess,
                        OnGetLeaderboardAroundUserCountryError);
                }
                else
                {
                    LogError();
                }
            }
            else
            {
                txtRank.text = $"Country Rank: {_countryData.myPosition + 1}";
                Refresh(_countryData);
            }
        }

        private void OnWorldButtonClicked()
        {
            if (_currentTab == ELeaderboardTab.World) return;
            _currentTab = ELeaderboardTab.World;
            UpdateDisplayTab();
            WorldButtonInvokeImpl();
        }

        private void WorldButtonInvokeImpl()
        {
            content.SetActive(false);

            if (_worldData.IsCanRefresh(ServiceSettings.delayFetchRank))
            {
                _worldData.firstTime = false;
                _worldData.players.Clear();
                _worldData.LastTimeRefreshLeaderboard = DateTime.UtcNow;
                if (AuthService.Instance.isLoggedIn && AuthService.Instance.isRequestCompleted)
                {
                    // wait if need
                    block.SetActive(true);
                    AuthService.GetMyPosition(LoginResultModel.playerId,
                        nameTableLeaderboard,
                        OnGetLeaderboardAroundUserWorldSuccess,
                        OnGetLeaderboardAroundUserWorldError);
                }
                else
                {
                    LogError();
                }
            }
            else
            {
                // display with old data
                txtRank.text = $"World Rank: {_worldData.myPosition + 1}";
                Refresh(_worldData);
            }
        }

        private void LogError()
        {
            if (AuthService.Instance.isLoggedIn)
            {
                Popup.Show<PopupNotification>(_ => _.Message("An error occurred,\nYou seem to have not completed entering your name and selecting your country"));
            }
            else
            {
                Popup.Show<PopupNotification>(_ => _.Message("Login failed for unknown reason!"));
            }
        }

        private void UpdateDisplayTab()
        {
            switch (_currentTab)
            {
                case ELeaderboardTab.World:
                    btnWorld.image.sprite = spriteTabSelected;
                    btnWorld.Label.color = colorTabTextSelected;

                    btnCountry.image.sprite = spriteTabNormal;
                    btnCountry.Label.color = colorTabTextNormal;

                    btnFriend.image.sprite = spriteTabNormal;
                    btnFriend.Label.color = colorTabTextNormal;
                    HandleIconFacebook.DeSelect();
                    break;
                case ELeaderboardTab.Country:
                    btnWorld.image.sprite = spriteTabNormal;
                    btnWorld.Label.color = colorTabTextNormal;

                    btnCountry.image.sprite = spriteTabSelected;
                    btnCountry.Label.color = colorTabTextSelected;

                    btnFriend.image.sprite = spriteTabNormal;
                    btnFriend.Label.color = colorTabTextNormal;
                    HandleIconFacebook.DeSelect();
                    break;
                case ELeaderboardTab.Friend:
                    btnWorld.image.sprite = spriteTabNormal;
                    btnWorld.Label.color = colorTabTextNormal;

                    btnCountry.image.sprite = spriteTabNormal;
                    btnCountry.Label.color = colorTabTextNormal;

                    btnFriend.image.sprite = spriteTabSelected;
                    btnFriend.Label.color = colorTabTextSelected;
                    HandleIconFacebook.Select();
                    break;
            }
        }

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
                    if (_countryData.currentPage == _countryData.pageCount)
                    {
                        if (_countryData.currentPage * CountInOnePage >= _countryData.players.Count && _worldData.players.Count > 0)
                        {
                            block.SetActive(true);
                            content.SetActive(false);
                            AuthService.RequestLeaderboard($"{nameTableLeaderboard}_{LoginResultModel.countryCode}",
                                NextPageRequestCountryLeaderboardSuccess,
                                NextPageRequestCountryLeaderboardError,
                                _countryData.currentPage * CountInOnePage);
                        }
                    }
                    else
                    {
                        btnNextPage.interactable = true;
                        Refresh(_countryData);
                    }

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

        private void OnGetLeaderboardAroundUserWorldError(PlayFabError error)
        {
            Popup.Show<PopupNotification>(_ => _.Message($"Retrieve your position in the failed world ranking!\nError code: {error.Error}"));
        }

        private void OnGetLeaderboardAroundUserWorldSuccess(GetLeaderboardAroundUserResult success)
        {
            _worldData.myPosition = success.Leaderboard[0].Position;
            txtRank.text = $"World Rank: {_worldData.myPosition + 1}";
            AuthService.RequestLeaderboard(nameTableLeaderboard, RequestWorldLeaderboardSuccess, RequestWorldLeaderboardError);
        }

        private void RequestWorldLeaderboardError(PlayFabError error)
        {
            Popup.Show<PopupNotification>(_ => _.Message($"Retrieve world ranking information failed!\nError code: {error.Error}"));
        }

        private void RequestWorldLeaderboardSuccess(GetLeaderboardResult result)
        {
            if (result == null && _worldData.players.Count == 0) return;

            txtWarning.gameObject.SetActive(false);
            if (result != null) _worldData.players = result.Leaderboard;
            _worldData.pageCount = M.CeilToInt(_worldData.players.Count / (float) CountInOnePage);
            Refresh(_worldData);
        }

        #endregion
        
        #region country

        private void NextPageRequestCountryLeaderboardSuccess(GetLeaderboardResult result)
        {
            btnNextPage.interactable = true;
            if (result == null && _countryData.players.Count == 0) return;

            txtWarning.gameObject.SetActive(false);
            if (result != null) _countryData.players.AddRange(result.Leaderboard);
            _countryData.pageCount = M.CeilToInt(_countryData.players.Count / (float) CountInOnePage);
            Refresh(_countryData);
        }

        private void NextPageRequestCountryLeaderboardError(PlayFabError error) { btnNextPage.interactable = true; }

        private void OnGetLeaderboardAroundUserCountryError(PlayFabError error)
        {
            Popup.Show<PopupNotification>(_ => _.Message($"Retrieve your position in the failed country ranking!\nError code: {error.Error}"));
        }

        private void OnGetLeaderboardAroundUserCountrySuccess(GetLeaderboardAroundUserResult success)
        {
            _countryData.myPosition = success.Leaderboard[0].Position;
            txtRank.text = $"Country Rank: {_countryData.myPosition + 1}";
            AuthService.RequestLeaderboard($"{nameTableLeaderboard}_{LoginResultModel.countryCode}", RequestCountryLeaderboardSuccess, RequestCountryLeaderboardError);
        }

        private void RequestCountryLeaderboardError(PlayFabError error)
        {
            Popup.Show<PopupNotification>(_ => _.Message($"Retrieve country ranking information failed!\nError code: {error.Error}"));
        }

        private void RequestCountryLeaderboardSuccess(GetLeaderboardResult result)
        {
            if (result == null && _countryData.players.Count == 0) return;

            txtWarning.gameObject.SetActive(false);
            if (result != null) _countryData.players = result.Leaderboard;
            _countryData.pageCount = M.CeilToInt(_countryData.players.Count / (float) CountInOnePage);
            Refresh(_countryData);
        }

        #endregion

        #region friend

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