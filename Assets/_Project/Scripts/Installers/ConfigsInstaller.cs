using Project.StaticData;
using UnityEngine;
using Zenject;

namespace Project.Installers
{
    [CreateAssetMenu(fileName = "new ConfigsInstaller", menuName = "StaticData/ConfigsInstaller", order = 0)]
    public class ConfigsInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private ImageDownloaderData _imageDownloaderData;
        [SerializeField] private MenuScreenPresenterData _menuScreenPresenterData;
        [SerializeField] private PremiumScreenPresenterData _premiumScreenPresenterData;

        public override void InstallBindings()
        {
            RegisterImageDownloaderData();
            RegisterMenuScreenPresenterData();
            RegisterPremiumScreenPresenterData();
        }

        private void RegisterImageDownloaderData()
        {
            Container
                .BindInstance(_imageDownloaderData)
                .AsSingle();
        } 
        
        private void RegisterMenuScreenPresenterData()
        {
            Container
                .BindInstance(_menuScreenPresenterData)
                .AsSingle();
        }
        
        private void RegisterPremiumScreenPresenterData()
        {
            Container
                .BindInstance(_premiumScreenPresenterData)
                .AsSingle();
        }
    }
}