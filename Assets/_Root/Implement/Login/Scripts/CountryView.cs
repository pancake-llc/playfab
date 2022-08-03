using Pancake.UI;
using TMPro;
using UnityEngine.UI;

namespace Pancake.GameService
{
    using UnityEngine;

    public class CountryView : EnhancedScrollerCellView
    {
        [SerializeField] private Image countryIcon;
        [SerializeField] private TextMeshProUGUI countryName;
        private CountryData _data;
        
        public Image CountryIcon => countryIcon;
        public TextMeshProUGUI CountryName => countryName;
        public CountryData Data => _data;

        public void SetData(CountryData data)
        {
            _data = data;
            
        }
    }

}