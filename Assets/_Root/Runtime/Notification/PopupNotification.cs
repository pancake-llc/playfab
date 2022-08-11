using Pancake.UI;
using TMPro;
using UnityEngine;

namespace Pancake.GameService
{
    public class PopupNotification : UIPopup
    {
        [SerializeField] private TextMeshProUGUI txtMessage;
        public void Message(string message) { txtMessage.text = message; }
    }
}