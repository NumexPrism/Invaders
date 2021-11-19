using TMPro;
using UI.Views.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Installers.UI.UiPanel
{
  class GameViewMonoInstaller : UiViewMonoInstaller<LoadingViewMonoInstaller>
  {
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public override void InstallBindings()
    {
      base.InstallBindings();
      Container.BindInstance(backButton);
      Container.BindInstance(waveText).WithId(UiLabelId.Wave);
      Container.BindInstance(livesText).WithId(UiLabelId.Lives);
      Container.BindInstance(scoreText).WithId(UiLabelId.Score);
    }
  }
}