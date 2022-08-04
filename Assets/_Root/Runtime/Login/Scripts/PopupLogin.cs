using Pancake.Common;
using Pancake.Tween;
using Pancake.UI;
using Pancake.UIQuery;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pancake.GameService
{
    public class PopupLogin : UIPopup, IEnhancedScrollerDelegate
    {
        private class PopupUiElements : IMappedObject
        {
            public IMapper Mapper { get; private set; }
            public UIButton BtnCountry { get; private set; }
            public TMP_InputField IpfEnterName { get; private set; }
            public TextMeshProUGUI TxtWarning { get; private set; }
            public RectTransform SelectCountryPopup { get; private set; }
            public Image ImgCurrentCountryIcon { get; private set; }
            public TextMeshProUGUI TxtCurrentCountryName { get; private set; }
            public EnhancedScroller Scroller { get; private set; }

            public PopupUiElements() { }
            public PopupUiElements(IMapper mapper) { Initialize(mapper); }

            public void Initialize(IMapper mapper)
            {
                Mapper = mapper;
                BtnCountry = mapper.Get<UIButton>("BtnCountry");
                IpfEnterName = mapper.Get<TMP_InputField>("IpfEnterName");
                TxtWarning = mapper.Get<TextMeshProUGUI>("TxtWarning");
                TxtCurrentCountryName = mapper.Get<TextMeshProUGUI>("TxtCurrentCountryName");
                SelectCountryPopup = mapper.Get<RectTransform>("SelectCountryPopup");
                ImgCurrentCountryIcon = mapper.Get<Image>("ImgCurrentCountryIcon");
                Scroller = mapper.Get<EnhancedScroller>("Scroller");
            }
        }

        [SerializeField] private CountryCode countryCode;
        [SerializeField] private CountryView elementPrefab;

        private SmallList<CountryData> _data;
        private PopupUiElements _uiElements;

        private void Start()
        {
            _uiElements = new PopupUiElements(UIRoot);
            // _uiElements.Scroller.Delegate = this;
            // _uiElements.IpfEnterName.characterLimit = 16;
            // _uiElements.IpfEnterName.onValueChanged.AddListener(OnInputNameCallback);
            // _uiElements.IpfEnterName.text = "";
            // _uiElements.IpfEnterName.ActivateInputField();
            // _uiElements.IpfEnterName.Select();
            // _uiElements.BtnCountry.onClick.RemoveListener(OnButtonShowPopupCountryClicked);
            // _uiElements.BtnCountry.onClick.AddListener(OnButtonShowPopupCountryClicked);
            // _uiElements.TxtWarning.gameObject.SetActive(false);
        }

        private void OnInputNameCallback(string value)
        {
            if (value.Length >= 16)
            {
                if (!_uiElements.TxtWarning.gameObject.activeSelf)
                {
                    _uiElements.TxtWarning.gameObject.SetActive(true);
                    _uiElements.TxtWarning.text = "Name length cannot be longer than 16 characters";
                }
            }
            else
            {
                _uiElements.TxtWarning.gameObject.SetActive(false);
            }
        }

        private void OnButtonShowPopupCountryClicked()
        {
            if (_uiElements.BtnCountry.AffectObject.localEulerAngles.z.Equals(0))
            {
                _uiElements.BtnCountry.AffectObject.TweenLocalRotationZ(90, 0.3f, RotationMode.Beyond360).Play();
            }
            else
            {
                _uiElements.BtnCountry.AffectObject.TweenLocalRotationZ(0, 0.3f, RotationMode.Beyond360).Play();
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
            _uiElements.SelectCountryPopup.sizeDelta = _uiElements.SelectCountryPopup.sizeDelta.Change(y: 103);
            _uiElements.SelectCountryPopup.TweenSizeDeltaY(666, 0.5f).SetEase(Ease.OutQuad).Play();
        }

        public int GetNumberOfCells(EnhancedScroller scroller) { return _data.Count; }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex) { return 120; }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var element = scroller.GetCellView(elementPrefab) as CountryView;
            if (element != null)
            {
                var code = (ECountryCode) dataIndex;
                element.name = "Country_" + code;
                element.Init(_data[dataIndex], countryCode.Get);
                return element;
            }

            return null;
        }
    }
}