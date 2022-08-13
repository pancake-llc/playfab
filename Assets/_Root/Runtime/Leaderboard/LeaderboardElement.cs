using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pancake.GameService
{
    public class LeaderboardElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtRank;
        [SerializeField] private Image imgCountry;
        [SerializeField] private TextMeshProUGUI txtUserName;
        [SerializeField] private TextMeshProUGUI txtScore;
        [SerializeField] private Image imgForcegound;

        protected InternalConfig userInternalConfig;

        public virtual void Init(InternalConfig userInternalConfig, int rank, Sprite icon, string userName, int score, Color color)
        {
            this.userInternalConfig = userInternalConfig;
            txtRank.text = $"{rank}";
            txtUserName.text = userName;
            imgForcegound.color = color;
            txtScore.text = score.ToString();
            imgCountry.sprite = icon;
            imgCountry.gameObject.SetActive(true);
        }
    }
}