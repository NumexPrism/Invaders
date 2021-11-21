using TMPro;
using UI;
using UI.Views.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Installers.Project.UI.UiPanel
{
  class GameViewMonoInstaller : UiViewMonoInstaller<LoadingViewMonoInstaller>
  {
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private Button shoot;
    [SerializeField] private HoldButton moveLeft;
    [SerializeField] private HoldButton moveRight;

    public override void InstallBindings()
    {
      base.InstallBindings();

      Container.Bind<GameButtonsHandler>().AsTransient();

      Container.BindInstance(backButton).WithId(GameUiButtonId.Back);
      Container.BindInstance(shoot).WithId(GameUiButtonId.Shoot);
      Container.BindInstance(moveLeft).WithId(GameUiButtonId.MoveLeft);
      Container.BindInstance(moveRight).WithId(GameUiButtonId.MoveRight);

      Container.BindInstance(waveText).WithId(UiLabelId.Wave);
      Container.BindInstance(livesText).WithId(UiLabelId.Lives);
      Container.BindInstance(scoreText).WithId(UiLabelId.Score);
    }
  }
}