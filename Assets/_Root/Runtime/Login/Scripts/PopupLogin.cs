using Pancake.Common;
using Pancake.Tween;
using Pancake.UI;
using TMPro;
using UnityEngine;

namespace Pancake.GameService
{
    public class PopupLogin : UIPopup, IEnhancedScrollerDelegate
    {
        [SerializeField] private CountryCode countryCode;
        [SerializeField] private CountryView elementPrefab;

        private SmallList<CountryData> _data;

        private const string IPF_ENTER_NAME = "IpfEnterName";
        private const string TXT_WARNING = "TxtWarning";
        private const string BTN_COUNTRY = "BtnCountry";
        private const string SELECT_COUNTRY_POPUP = "SelectCountryPopup";
        private const string SCROLLER = "Scroller";


        private void Start()
        {
            UIRoot.Get<EnhancedScroller>(SCROLLER).Delegate = this;
            var ipf = UIRoot.Get<TMP_InputField>(IPF_ENTER_NAME);
            ipf.characterLimit = 16;
            ipf.onValueChanged.AddListener(OnInputNameCallback);
            ipf.text = "";
            ipf.ActivateInputField();
            ipf.Select();
            var btn = UIRoot.Get<UIButton>(BTN_COUNTRY);
            btn.onClick.RemoveListener(OnButtonShowPopupCountryClicked);
            btn.onClick.AddListener(OnButtonShowPopupCountryClicked);
            var warning = UIRoot.Get<TextMeshProUGUI>(TXT_WARNING);
            warning.gameObject.SetActive(false);
        }

        private void OnInputNameCallback(string value)
        {
            var warning = UIRoot.Get<TextMeshProUGUI>(TXT_WARNING);
            if (value.Length >= 16)
            {
                if (!warning.gameObject.activeSelf)
                {
                    warning.gameObject.SetActive(true);
                    warning.text = "Name length cannot be longer than 16 characters";
                }
            }
            else
            {
                warning.gameObject.SetActive(false);
            }
        }

        private void OnButtonShowPopupCountryClicked()
        {
            var btn = UIRoot.Get<UIButton>(BTN_COUNTRY);
            if (btn.AffectObject.localEulerAngles.z.Equals(0))
            {
                btn.AffectObject.TweenLocalRotationZ(90, 0.3f, RotationMode.Beyond360).Play();
            }
            else
            {
                btn.AffectObject.TweenLocalRotationZ(0, 0.3f, RotationMode.Beyond360).Play();
            }
        }

        private void InitData()
        {
            _data = new SmallList<CountryData>();
            for (var i = 0; i < countryCode.countryCodeDatas.Length; i++)
            {
                _data.Add(new CountryData() {id = i});
            }

            UIRoot.Get<EnhancedScroller>(SCROLLER).ReloadData();
        }

        private void InternalShowSelectCountry()
        {
            var t = UIRoot.Get<RectTransform>(SELECT_COUNTRY_POPUP);
            t.gameObject.SetActive(true);
            t.sizeDelta = t.sizeDelta.Change(y: 103);
            t.TweenSizeDeltaY(666, 0.5f).SetEase(Ease.OutQuad).Play();
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