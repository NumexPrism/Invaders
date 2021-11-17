using UnityEngine;
using UnityEngine.UI;

namespace DependencyInjection.UI
{
  class GameEndViewMonoInstaller : UiViewMonoInstaller<LoadingViewMonoInstaller>
  {
    [SerializeField] private Button nextButton;

    public override void InstallBindings()
    {
      base.InstallBindings();
      Container.BindInstance(nextButton);
    }
  }
}