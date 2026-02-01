using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.StaticData;
using Project.UI.Views;
using R3;
using UnityEngine;

namespace Project.UI.Presenters
{
    public class PremiumScreenPresenter : UIScreenPresenter<PremiumScreenView>
    {
        private readonly PremiumScreenPresenterData _data;
        
        public PremiumScreenPresenter(UIHub hub, PremiumScreenPresenterData data) : base(hub)
        {
            _data = data;
        }

        public override void Initialize()
        {
            View.CloseButtonClicked
                .Subscribe(OnCloseButtonClicked)
                .AddTo(Disposables);
            
            View.transform.localScale = Vector3.zero;
        }

        public override async UniTask Enable()
        {
            await base.Enable();
            
            await OpenWithBounce();
        }

        public override async UniTask Disable()
        {
            await CloseWithBounce();

            await base.Disable();
        }

        private void OnCloseButtonClicked(Unit _)
        {
            Disable().Forget();
        }
        
        private async UniTask OpenWithBounce()
        {
            View.transform.localScale = Vector3.zero;
        
            Sequence sequence = DOTween.Sequence();
        
            await sequence
                .Append(View.transform.DOScale(_data.Overshoot, _data.OpenDuration)
                .SetEase(_data.OvershootEase));
        
            await sequence
                .Append(View.transform.DOScale(1f, _data.OvershootDuration)
                .SetEase(_data.OpenEase));
        }
        
        private async UniTask CloseWithBounce()
        {
            await View.transform
                .DOScale(0f, _data.CloseDuration)
                .SetEase(_data.CloseEase);
        }
    }
}