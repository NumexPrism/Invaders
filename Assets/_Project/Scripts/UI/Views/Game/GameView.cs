using DependencyInjection.UI;
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
    [Inject] private EnemyWave _enemyWave;

    private void OnEnable()
    {
      _backButton.onClick.AddListener(BackButtonClicked);
      _enemyWave.WaveSpawned += ShowWave;
    }

    private void ShowWave(int number)
    {
      _waveText.text = Mathf.Min(number, 99).ToString("D2");;
    }

    private void OnDisable()
    {
      _backButton.onClick.RemoveListener(BackButtonClicked);
    }

    private void BackButtonClicked()
    {
      UiFacade.ShowPreviousView();
    }
  }
}