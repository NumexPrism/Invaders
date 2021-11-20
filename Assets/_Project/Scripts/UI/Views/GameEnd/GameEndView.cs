using Cysharp.Threading.Tasks;
using Installers.Project.UI.UiPanel;
using Mechanics.GameRules;
using TMPro;
using UI.Views.Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views.GameEnd
{
  [RequireComponent(typeof(GameEndViewMonoInstaller))]
  internal class GameEndView: BaseUIView
  {
    //it's not some global value, it's just a max value that can be displayed in UI
    //so not moving to any configs or global constants.
    private const int MaxScore = 999999;
    private const int MaxWaves = 99;

    [Inject] private DiContainer _container;

    [Inject] private Button _nextButton;

    [Inject(Id = UiLabelId.Wave)] private TextMeshProUGUI _waveText;
    [Inject(Id = UiLabelId.Score)] private TextMeshProUGUI _scoreText;

    public override async UniTask Show()
    {
      await base.Show();
      _nextButton.onClick.AddListener(NextButtonClicked);
      var gameSession = _container.Resolve<IGameSession>();
      _waveText.text = Mathf.Min(gameSession.WaveCount.Value, MaxWaves).ToString("D2");
      _scoreText.text = Mathf.Min(gameSession.PlayerScore.Value, MaxScore).ToString("D6");
    }

    public override async UniTask Hide()
    {
      await base.Hide();
      _nextButton.onClick.RemoveListener(NextButtonClicked);
    }

    private void NextButtonClicked()
    {
      UiFacade.ShowNextView();
    }
  }
}