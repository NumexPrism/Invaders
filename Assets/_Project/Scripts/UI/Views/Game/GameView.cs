using System;
using Cysharp.Threading.Tasks;
using Installers.Project.UI.UiPanel;
using Mechanics.GameRules;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views.Game
{
  [RequireComponent(typeof(GameViewMonoInstaller))]
  internal class GameView : BaseUIView
  {
    //it's not some global value, it's just a max value that can be displayed in UI
    //so not moving to any configs or global constants.
    private const int MaxScore = 999999;
    private const int MaxWaves = 99;

    [Inject] private DiContainer _container;

    [Inject(Id = GameUiButtonId.Back)] private Button _backButton;
    [Inject(Id = UiLabelId.Wave)] private TextMeshProUGUI _waveText;
    [Inject(Id = UiLabelId.Lives)] private TextMeshProUGUI _livesText;
    [Inject(Id = UiLabelId.Score)] private TextMeshProUGUI _scoreText;

    [Inject(Id = GameUiButtonId.MoveLeft)] private HoldButton _moveLeftButton;
    [Inject(Id = GameUiButtonId.MoveRight)] private HoldButton _moveRightButton;
    [Inject(Id = GameUiButtonId.Shoot)] private Button _shootButton;

    [Inject] private GameButtonsHandler _gameButtonsHandler;

    private IGameSession _subscribedGameSession;

    public Action ShootCallback
    {
      set => _gameButtonsHandler.ShootCallback = value;
    }

    public Action<float> MoveStartCallback
    {
      set => _gameButtonsHandler.MoveStartCallback = value;
    }

    public Action MoveEndCallback
    {
      set => _gameButtonsHandler.MoveEndCallback = value;
    }

    public override async UniTask Show()
    {
      await base.Show();

      //GameController is injected ad hoc when GameScene opens
      this._subscribedGameSession = _container.Resolve<IGameSession>();
      _backButton.onClick.AddListener(BackButtonClicked);
      _subscribedGameSession.WaveCount.Observe(ShowWave);
      _subscribedGameSession.PlayerLives.Observe(ShowLives);
      _subscribedGameSession.PlayerScore.Observe(ShowScore);

      // ok, so at the last moment it turned out that OnScreenButtons in the new InputSystem are crashing on android.
      // since I didn't have any more time to investigate, I just did a quick and dirty solution. Because who needs a cool nice code, that just doesn't work
      // If you already know, what I did wrong with the new InputSystem - i would gladly learn
      _shootButton.onClick.AddListener(_gameButtonsHandler.ShootClicked);
      _moveLeftButton.OnDown += _gameButtonsHandler.LeftMovePressed;
      _moveLeftButton.OnUp += _gameButtonsHandler.LeftMoveReleased;
      _moveRightButton.OnDown += _gameButtonsHandler.RightMovePressed;
      _moveRightButton.OnUp += _gameButtonsHandler.RightMoveReleased;
    }

    

    public override async UniTask Hide()
    {
      await base.Hide();

      _shootButton.onClick.RemoveListener(_gameButtonsHandler.ShootClicked);
      _moveLeftButton.OnDown -= _gameButtonsHandler.LeftMovePressed;
      _moveLeftButton.OnUp -= _gameButtonsHandler.LeftMoveReleased;
      _moveRightButton.OnDown -= _gameButtonsHandler.RightMovePressed;
      _moveRightButton.OnUp -= _gameButtonsHandler.RightMoveReleased;

      _backButton.onClick.RemoveListener(BackButtonClicked);
      _subscribedGameSession.WaveCount.StopObserving(ShowWave);
      _subscribedGameSession.PlayerLives.StopObserving(ShowLives);
      _subscribedGameSession.PlayerScore.StopObserving(ShowScore);
    }

    private void ShowWave(int number)
    {
      _waveText.text = Mathf.Min(number, MaxWaves).ToString("D2"); ;
    }

    private void ShowScore(int number)
    {
      _scoreText.text = Mathf.Min(number, MaxScore).ToString("D6"); ;
    }

    private void ShowLives(int number)
    {
      char Symbol = 'M';
      _livesText.text = new string(Symbol, number);
    }

    private void BackButtonClicked()
    {
      UiFacade.ShowPreviousView();
    }
  }
}