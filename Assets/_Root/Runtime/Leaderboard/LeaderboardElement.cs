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

        public void Init(int rank, Sprite icon, string userName, string score, Func<int, Color> get, bool self)
        {
            txtRank.text = $"{rank}";
            txtUserName.text = userName;

            switch (rank)
            {
                case 1:
                    imgForcegound.color = get(0);
                    break;
                case 2:
                    imgForcegound.color = get(1);
                    break;
                case 3:
                    imgForcegound.color = get(2);
                    break;
                default:
                    imgForcegound.color = get(self ? 4 : 3);
                    break;
            }

            txtScore.text = score;

            imgCountry.sprite = icon;
            imgCountry.gameObject.SetActive(true);
        }
    }
}