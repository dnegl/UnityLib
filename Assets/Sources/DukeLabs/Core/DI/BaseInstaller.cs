using DukeLabs.Core.UI;
using Zenject;

namespace DukeLabs.Core.DI
{
    public class BaseInstaller : MonoInstaller<BaseInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<UpdateManager>().FromNewComponentOnNewGameObject().AsSingle();
        }
    }
}