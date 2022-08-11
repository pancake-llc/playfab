using System;
using Pancake.Common;
using Pancake.Tween;
using Pancake.UI;
using Pancake.UIQuery;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pancake.GameService
{
    public class PopupEnterName : UIPopup, IEnhancedScrollerDelegate
    {
        private class PopupUiElements : IMappedObject
        {
            public IMapper Mapper { get; private set; }
            public UIButton BtnCountry { get; private set; }
            public UIButton BtnOk { get; private set; }
            public TMP_InputField IpfEnterName { get; private set; }
            public TextMeshProUGUI TxtWarning { get; private set; }
            public RectTransform SelectCountryPopup { get; private set; }
            public Image ImgCurrentCountryIcon { get; private set; }
            public TextMeshProUGUI TxtCurrentCountryName { get; private set; }
            public EnhancedScroller Scroller { get; private set; }
            public Transform Block { get; private set; }

            public PopupUiElements(IMapper mapper) { Initialize(mapper); }

            public void Initialize(IMapper mapper)
            {
                Mapper = mapper;
                BtnCountry = mapper.Get<UIButton>("BtnCountry");
                BtnOk = mapper.Get<UIButton>("BtnOk");
                IpfEnterName = mapper.Get<TMP_InputField>("IpfEnterName");
                TxtWarning = mapper.Get<TextMeshProUGUI>("TxtWarning");
                TxtCurrentCountryName = mapper.Get<TextMeshProUGUI>("TxtCurrentCountryName");
                SelectCountryPopup = mapper.Get<RectTransform>("SelectCountryPopup");
                ImgCurrentCountryIcon = mapper.Get<Image>("ImgCurrentCountryIcon");
                Scroller = mapper.Get<EnhancedScroller>("Scroller");
                Block = mapper.Get<RectTransform>("Block");
            }
        }

        [SerializeField] private CountryCode countryCode;
        [SerializeField] private CountryView elementPrefab;
        [SerializeField] private Sprite btnSpriteLocked;

        private SmallList<CountryData> _data;
        private PopupUiElements _uiElements;
        private bool _firstTime;
        private ITween _tween;
        private string _selectedCountry;
        private string _userName;
        private ISequence _sequenceTxtWarning;
        private Sprite _defaultSprite;

        public Func<bool> onAcceptName;

        private void Start()
        {
            _uiElements = new PopupUiElements(UIRoot);
            _uiElements.Scroller.Delegate = this;
            _uiElements.IpfEnterName.characterLimit = 17;
            _uiElements.IpfEnterName.onValueChanged.AddListener(OnInputNameCallback);
            _uiElements.IpfEnterName.text = "";
            _uiElements.IpfEnterName.ActivateInputField();
            _uiElements.IpfEnterName.Select();
            _uiElements.BtnCountry.onClick.RemoveListener(OnButtonShowPopupCountryClicked);
            _uiElements.BtnCountry.onClick.AddListener(OnButtonShowPopupCountryClicked);
            _uiElements.BtnOk.onClick.RemoveListener(OnButtonOkClicked);
            _uiElements.BtnOk.onClick.AddListener(OnButtonOkClicked);
            _uiElements.TxtWarning.gameObject.SetActive(false);
            var countryData = countryCode.Get(LoginResultModel.countryCode);
            _uiElements.ImgCurrentCountryIcon.sprite = countryData.icon;
            _uiElements.ImgCurrentCountryIcon.color = Color.white;
            _uiElements.TxtCurrentCountryName.text = countryData.name;
            _defaultSprite = _uiElements.BtnOk.image.sprite;
            _uiElements.Block.gameObject.SetActive(false);
            _selectedCountry = LoginResultModel.countryCode;
        }

        private void OnButtonOkClicked()
        {
            if (string.IsNullOrEmpty(_uiElements.IpfEnterName.text))
            {
                DisplayWarning("Name cannot be blank!");
                _uiElements.Block.gameObject.SetActive(false);
                _uiElements.IpfEnterName.Select();
                return;
            }

            _uiElements.BtnOk.interactable = false;
            _uiElements.Block.gameObject.SetActive(true);
            _uiElements.TxtWarning.gameObject.SetActive(false);
            var result = onAcceptName?.Invoke(); // validate name with server
            _uiElements.Block.gameObject.SetActive(false);
            if (result == null)
            {
                DisplayWarning("Error code O1: Result null, Invalid action");
                return;
            }

            if (result.Value)
            {
                // store user choose country code
                
                //ServiceSettings.SetCurrentName(_userName);
                AuthService.Instance.IsCompleteSetupName = true;
            }
        }

        private void OnInputNameCallback(string value)
        {
            if (value.Length >= 16)
            {
                _uiElements.BtnOk.interactable = false;
                _uiElements.BtnOk.image.sprite = btnSpriteLocked;
                if (!_uiElements.TxtWarning.gameObject.activeSelf) DisplayWarning("Name length cannot be longer than 16 characters!");
            }
            else
            {
                _uiElements.TxtWarning.gameObject.SetActive(false);
                _uiElements.BtnOk.interactable = true;
                _uiElements.BtnOk.image.sprite = _defaultSprite;
            }
        }

        private void DisplayWarning(string message)
        {
            _uiElements.TxtWarning.gameObject.SetActive(true);
            _uiElements.TxtWarning.text = message;
            _sequenceTxtWarning?.Kill();
            _sequenceTxtWarning = TweenManager.Sequence();
            _sequenceTxtWarning.Append(_uiElements.TxtWarning.transform.TweenLocalScale(new Vector3(1.1f, 1.1f, 1.1f), 0.12f).SetEase(Ease.Parabolic));
            _sequenceTxtWarning.Append(_uiElements.TxtWarning.transform.TweenLocalScale(new Vector3(1f, 1f, 1f), 0.12f).SetEase(Ease.Linear));
            _sequenceTxtWarning.Play();
        }

        private void OnButtonShowPopupCountryClicked()
        {
            if (_uiElements.BtnCountry.AffectObject.localEulerAngles.z.Equals(0))
            {
                _uiElements.BtnCountry.AffectObject.TweenLocalRotationZ(90, 0.3f, RotationMode.Beyond360).Play();
                // show

                InternalShowSelectCountry();
                if (!_firstTime)
                {
                    _firstTime = true;
                    InitData();
                }
            }
            else
            {
                _uiElements.BtnCountry.AffectObject.TweenLocalRotationZ(0, 0.3f, RotationMode.Beyond360).Play();
                // hide
                InternalHideSelectCountry();
            }
        }

        private void InitData()
        {
            _data = new SmallList<CountryData>();
            for (var i = 0; i < countryCode.countryCodeDatas.Length; i++)
            {
                _data.Add(new CountryData() {id = i});
            }

            _uiElements.Scroller.ReloadData();
        }

        private void InternalShowSelectCountry()
        {
            _uiElements.SelectCountryPopup.gameObject.SetActive(true);
            _uiElements.BtnOk.interactable = false;
            _uiElements.SelectCountryPopup.sizeDelta = _uiElements.SelectCountryPopup.sizeDelta.Change(y: 103);
            _tween?.Kill();
            _tween = _uiElements.SelectCountryPopup.TweenSizeDeltaY(666, 0.5f);
            _tween.SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    _uiElements.Scroller.ScrollbarVisibility = EnhancedScroller.ScrollbarVisibilityEnum.Always;
                    _uiElements.BtnOk.interactable = true;
                })
                .Play();
            ContainerTransform.TweenSizeDeltaY(1206f, 0.5f).Play();
        }

        private void InternalHideSelectCountry()
        {
            _uiElements.Scroller.ScrollbarVisibility = EnhancedScroller.ScrollbarVisibilityEnum.Never;
            _uiElements.BtnOk.interactable = false;
            _tween?.Kill();
            _tween = _uiElements.SelectCountryPopup.TweenSizeDeltaY(103f, 0.5f);
            _tween.SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    _uiElements.SelectCountryPopup.gameObject.SetActive(false);
                    _uiElements.BtnOk.interactable = true;
                })
                .Play();
            ContainerTransform.TweenSizeDeltaY(940f, 0.5f).Play();
        }

        #region implement

        public int GetNumberOfCells(EnhancedScroller scroller) { return _data.Count; }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex) { return 120; }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var element = scroller.GetCellView(elementPrefab) as CountryView;
            if (element != null)
            {
                var code = (ECountryCode) dataIndex;
                element.name = "Country_" + code;
                element.Init(_data[dataIndex], OnButtonElementCountryClicked, IsElementSelected, countryCode.Get);
                return element;
            }

            return null;
        }

        private void OnButtonElementCountryClicked(CountryView view)
        {
            _selectedCountry = view.Data.code.ToString();
            _userName = _uiElements.IpfEnterName.text;
            _uiElements.Scroller.RefreshActiveCellViews();
            _uiElements.ImgCurrentCountryIcon.sprite = view.Data.icon;
            _uiElements.ImgCurrentCountryIcon.color = Color.white;
            _uiElements.TxtCurrentCountryName.text = view.Data.name;
        }

        private bool IsElementSelected(string code) { return _selectedCountry.Equals(code); }

        #endregion
    }
}