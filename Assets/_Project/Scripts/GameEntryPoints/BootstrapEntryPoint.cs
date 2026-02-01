using Cysharp.Threading.Tasks;
using Project.Enums;
using Project.Insterfaces;
using UnityEngine;
using Zenject;

namespace Project.GameEntryPoints
{
    public class BootstrapEntryPoint : IInitializable
    {
        private readonly ISceneLoaderService _sceneLoaderService;

        public BootstrapEntryPoint(ISceneLoaderService sceneLoaderService)
        {
            _sceneLoaderService = sceneLoaderService;
        }
        
        void IInitializable.Initialize()
        {
            StartAsync().Forget();
        }

        private async UniTask StartAsync()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            await _sceneLoaderService.LoadScene(CurrentScene.Game);
        }
    }
}