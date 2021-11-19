using DependencyInjection.UI;
using Mechanics;
using Mechanics.Enemy;
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
    [Inject] private GameController _enemyWave;

    private void OnEnable()
    {
      _backButton.onClick.AddListener(BackButtonClicked);
      _enemyWave.WaveCount.Observe(ShowWave);
      _enemyWave.PlayerLives.Observe(ShowLives);
      _enemyWave.PlayerScore.Observe(ShowScore);
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
      _enemyWave.WaveCount.StopObserving(ShowWave);
      _enemyWave.PlayerLives.StopObserving(ShowLives);
      _enemyWave.PlayerScore.StopObserving(ShowScore);
    }

    private void BackButtonClicked()
    {
      UiFacade.ShowPreviousView();
    }
  }
}