using TMPro;
using UI.Views.Game;
using UnityEngine;
using UnityEngine.UI;

namespace DependencyInjection.UI
{
  class GameViewMonoInstaller : UiViewMonoInstaller<LoadingViewMonoInstaller>
  {
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI waveText;

    public override void InstallBindings()
    {
      base.InstallBindings();
      Container.BindInstance(backButton);
      Container.BindInstance(waveText).WithId(UiLabelId.Wave);
    }
  }
}