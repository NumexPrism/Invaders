using Zenject;

namespace DependencyInjection
{
  class EntrySceneInstaller: MonoInstaller<UiInstaller>
  {
    public Starter Starter; 
    //public UiFacade Ui; 

    public override void InstallBindings()
    {
     // Container.BindInstance<IUiFacade>(Ui);
      Container.BindInstance(Starter);
    }
  }
}
