using Installers.UI.UiPanel;
using Mechanics;
using Mechanics.Enemy;
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
      _waveText.text = Mathf.Min(number, 99).ToString("D2");;
    }

    private void ShowScore(int number)
    {
      _scoreText.text = Mathf.Min(number, 99).ToString("D6");;
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