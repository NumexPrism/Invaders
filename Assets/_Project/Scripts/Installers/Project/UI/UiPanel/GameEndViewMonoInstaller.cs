using UnityEngine;
using UnityEngine.UI;

namespace Installers.Project.UI.UiPanel
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