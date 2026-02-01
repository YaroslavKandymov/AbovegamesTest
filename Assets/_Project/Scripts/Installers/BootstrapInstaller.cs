using Project.GameEntryPoints;
using Zenject;

namespace Project.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindBootstrapEntryPoint();
        }

        private void BindBootstrapEntryPoint()
        {
            Container
                .BindInterfacesAndSelfTo<BootstrapEntryPoint>()
                .AsSingle()
                .NonLazy();
        }
    }
}