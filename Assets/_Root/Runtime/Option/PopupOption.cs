using System;
using Pancake.UI;
using TMPro;
using UnityEngine;

namespace Pancake.GameService
{
    public class PopupOption : UIPopup
    {
        [SerializeField] private TextMeshProUGUI txtMessage;
        [SerializeField] private UIButton btnOk;
        [SerializeField] private UIButton btnCancel;

        private Action _actionOk;
        private Action _actionCancel;

        public void Message(string message) { txtMessage.text = message; }

        public void Ok(Action actionOk)
        {
            _actionOk = actionOk;
            btnOk.onClick.RemoveListener(InvokeActionOk);
            btnOk.onClick.AddListener(InvokeActionOk);
        }

        public void Cancel(Action actionCancel)
        {
            _actionCancel = actionCancel;
            btnCancel.onClick.RemoveListener(InvokeActionCancel);
            btnCancel.onClick.AddListener(InvokeActionCancel);
        }

        private void InvokeActionOk() { _actionOk?.Invoke(); }
        private void InvokeActionCancel() { _actionCancel?.Invoke(); }
    }
}