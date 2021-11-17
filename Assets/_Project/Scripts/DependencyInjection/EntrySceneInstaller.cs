using Zenject;

namespace DependencyInjection
{
  class EntrySceneInstaller: MonoInstaller<UiInstaller>
  {
    public Starter Starter; 

    public override void InstallBindings()
    {
      Container.BindInstance(Starter);
    }
  }
}
