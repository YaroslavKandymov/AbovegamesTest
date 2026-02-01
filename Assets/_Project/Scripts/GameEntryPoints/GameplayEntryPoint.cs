using Project.Services;
using Project.UI.Presenters;
using Zenject;

namespace Project.GameEntryPoints
{
    public class GameplayEntryPoint : IInitializable
    {
        private readonly SplashScreenPresenter _splashScreenPresenter;
        private readonly MenuScreenPresenter _menuScreenPresenter;
        private readonly PremiumScreenPresenter _premiumScreenPresenter;
        private readonly SoundService _soundService;

        public GameplayEntryPoint(
            SplashScreenPresenter splashScreenPresenter,
            MenuScreenPresenter menuScreenPresenter,
            PremiumScreenPresenter premiumScreenPresenter,
            SoundService soundService)
        {
            _splashScreenPresenter = splashScreenPresenter;
            _menuScreenPresenter = menuScreenPresenter;
            _premiumScreenPresenter = premiumScreenPresenter;
            _soundService = soundService;
        }

        void IInitializable.Initialize()
        {
            _splashScreenPresenter.Initialize();
            _menuScreenPresenter.Initialize();
            _premiumScreenPresenter.Initialize();
            _soundService.Initialize();
        }
    }
}