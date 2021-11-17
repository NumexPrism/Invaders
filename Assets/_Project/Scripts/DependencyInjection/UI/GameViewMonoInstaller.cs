using UnityEngine;
using UnityEngine.UI;

namespace DependencyInjection.UI
{
  class GameViewMonoInstaller : UiViewMonoInstaller<LoadingViewMonoInstaller>
  {
    [SerializeField] private Button backButton;

    public override void InstallBindings()
    {
      base.InstallBindings();
      Container.BindInstance(backButton);
    }
  }
}