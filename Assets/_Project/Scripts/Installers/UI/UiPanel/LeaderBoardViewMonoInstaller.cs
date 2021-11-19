using UnityEngine;
using UnityEngine.UI;

namespace Installers.UI.UiPanel
{
  class LeaderBoardViewMonoInstaller : UiViewMonoInstaller<LoadingViewMonoInstaller>
  {
    [SerializeField] private Button backButton;

    public override void InstallBindings()
    {
      base.InstallBindings();
      Container.BindInstance(backButton);
    }
  }
}