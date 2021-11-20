using TMPro;
using UI.Views.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Installers.Project.UI.UiPanel
{
  class GameEndViewMonoInstaller : UiViewMonoInstaller<LoadingViewMonoInstaller>
  {
    [SerializeField] private Button nextButton;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public override void InstallBindings()
    {
      base.InstallBindings();
      Container.BindInstance(nextButton);
      Container.BindInstance(waveText).WithId(UiLabelId.Wave);
      Container.BindInstance(scoreText).WithId(UiLabelId.Score);
    }
  }
}