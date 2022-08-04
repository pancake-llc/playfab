using System;
using Pancake.UI;
#if PANCAKE_TMP
using TMPro;
#endif
using UnityEngine.UI;

namespace Pancake.GameService
{
    using UnityEngine;

    public class CountryView : EnhancedScrollerCellView
    {
        [SerializeField] private Image countryIcon;
#if PANCAKE_TMP
        [SerializeField] private TextMeshProUGUI countryName;
#endif

        private CountryData _data;

        public Image CountryIcon => countryIcon;
#if PANCAKE_TMP
        public TextMeshProUGUI CountryName => countryName;
#endif
        public CountryData Data => _data;

        public void Init(CountryData data, Func<string, CountryCodeData> get)
        {
            _data = data;
            var result = get?.Invoke(((ECountryCode) _data.id).ToString());
            if (result != null)
            {
                countryIcon.sprite = result.icon;
#if PANCAKE_TMP
                countryName.text = result.name;
#endif
            }
        }
    }
}