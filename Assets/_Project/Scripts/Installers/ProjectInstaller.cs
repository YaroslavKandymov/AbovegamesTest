using Project.Insterfaces;
using Project.Services;
using Zenject;

namespace Project.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSceneLoader();
        }

        private void BindSceneLoader()
        {
            Container
                .Bind<ISceneLoaderService>()
                .To<SceneLoaderService>()
                .AsSingle();
        }
    }
}
