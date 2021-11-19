using Installers.Project.UI.UiPanel;
using Mechanics.GameRules;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views.Game
{
  [RequireComponent(typeof(GameViewMonoInstaller))]
  internal class GameView: BaseUIView
  {
    //it's not some global value, it's just a max value that can be displayed in UI
    //so not moving to any configs or global constants.
    private const int MaxScore = 999999;
    private const int MaxWaves = 99;

    //rule of three at work: _backButton is just copied from the LeaderBoard View. If I have a third occurrence, I will introduce a base class, or move to composite
    [Inject] private Button _backButton;
    [Inject(Id = UiLabelId.Wave)] private TextMeshProUGUI _waveText;
    [Inject(Id = UiLabelId.Lives)] private TextMeshProUGUI _livesText;
    [Inject(Id = UiLabelId.Score)] private TextMeshProUGUI _scoreText;

    [Inject] private DiContainer container;

    private IGameSession _subscribedGameSession;

    private void OnEnable()
    {
      //GameController is injected dynamically when GameScene opens
      //when
      SubscribeToGameSession(container.Resolve<IGameSession>());
    }

    private void SubscribeToGameSession(IGameSession gameSession)
    {
      this._subscribedGameSession = gameSession;
      _backButton.onClick.AddListener(BackButtonClicked);
      _subscribedGameSession.WaveCount.Observe(ShowWave);
      _subscribedGameSession.PlayerLives.Observe(ShowLives);
      _subscribedGameSession.PlayerScore.Observe(ShowScore);
    }

    private void ShowWave(int number)
    {
      _waveText.text = Mathf.Min(number, MaxWaves).ToString("D2");;
    }

    private void ShowScore(int number)
    {
      _scoreText.text = Mathf.Min(number, MaxScore).ToString("D6");;
    }

    private void ShowLives(int number)
    {
      char Symbol = 'M';
      _livesText.text = new string(Symbol, number);
    }

    private void OnDisable()
    {
      _backButton.onClick.RemoveListener(BackButtonClicked);
      _subscribedGameSession.WaveCount.StopObserving(ShowWave);
      _subscribedGameSession.PlayerLives.StopObserving(ShowLives);
      _subscribedGameSession.PlayerScore.StopObserving(ShowScore);
    }

    private void BackButtonClicked()
    {
      UiFacade.ShowPreviousView();
    }
  }
}