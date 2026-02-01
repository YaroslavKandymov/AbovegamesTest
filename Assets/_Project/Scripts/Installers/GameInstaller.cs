using Project.GameEntryPoints;
using Project.Services;
using Project.UI;
using Project.UI.Presenters;
using UnityEngine;
using Zenject;

namespace Project.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private UIHub _uiHub;
        [SerializeField] private SoundCollection _soundCollection;

        public override void InstallBindings()
        {
            RegisterUI();
            RegisterDeviceChecker();
            RegisterImageDownloaderService();
            RegisterPresenters();
            RegisterSound();
            RegisterGameplayEntryPoint();
        }

        private void RegisterUI()
        {
            var ui = Container.InstantiatePrefabForComponent<UIHub>(_uiHub);

            Container
                .Bind<UIHub>()
                .FromInstance(ui)
                .AsSingle();
        }
        
        private void RegisterDeviceChecker()
        {
            Container
                .Bind<DeviceChecker>()
                .AsSingle();
        }
        
        private void RegisterImageDownloaderService()
        {
            Container
                .Bind<ImageDownloaderService>()
                .AsSingle();
        }

        private void RegisterPresenters()
        {
            Container
                .Bind<PremiumScreenPresenter>()
                .AsSingle();
            
            Container
                .Bind<MenuScreenPresenter>()
                .AsSingle();
            
            Container
                .Bind<SplashScreenPresenter>()
                .AsSingle();
        }

        private void RegisterSound()
        {
            var sound = Container.InstantiatePrefabForComponent<SoundCollection>(_soundCollection);

            Container
                .Bind<SoundCollection>()
                .FromInstance(sound)
                .AsSingle();
            
            Container
                .Bind<SoundService>()
                .AsSingle();
        }

        private void RegisterGameplayEntryPoint()
        {
            Container
                .BindInterfacesAndSelfTo<GameplayEntryPoint>()
                .AsSingle()
                .NonLazy();
        }
    }
}