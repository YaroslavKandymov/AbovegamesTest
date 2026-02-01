using Cysharp.Threading.Tasks;
using Project.UI.Views;

namespace Project.UI.Presenters
{
    public class SplashScreenPresenter : UIScreenPresenter<SplashScreenView>
    {
        private readonly MenuScreenPresenter _menuScreenPresenter;
        
        public SplashScreenPresenter(UIHub hub, MenuScreenPresenter menuScreenPresenter) : base(hub)
        {
            _menuScreenPresenter = menuScreenPresenter;
        }

        public override void Initialize()
        {
            Enable().Forget();
            
            Work().Forget();
        }

        private async UniTaskVoid Work()
        {
            await UniTask.WaitForSeconds(View.ExistTime);

            await Disable();
            
            _menuScreenPresenter.Enable().Forget();
        }
    }
}