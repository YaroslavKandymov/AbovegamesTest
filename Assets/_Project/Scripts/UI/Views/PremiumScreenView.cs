using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Views
{
    public class PremiumScreenView : UIScreenView
    {
        public Observable<Unit> CloseButtonClicked => _closeButton.onClick.AsObservable();
        
        [SerializeField] private Button _closeButton;
    }
}