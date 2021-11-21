using System;
using System.Collections.Generic;
using AssetManagement;
using Cysharp.Threading.Tasks;
using LeaderBoard;
using Mechanics.Field;
using Mechanics.Player;
using UI;
using UnityEngine;
using Utils;
using Zenject;

namespace Mechanics.GameRules
{
  partial class GameController : MonoBehaviour, IGameSession
  {
    [Inject] private ILeaderBoardAdapter _leaderBoard;
    [Inject] private InvadersSceneManager _sceneManager;
    [Inject] private EnemyWave.EnemyWave _wave;
    [Inject] private IGameFieldConfig _field;
    [Inject] private IPlayerShip _player;
    [Inject] private IUiFacade _uiFacade;

    [Inject(Id = "Global")] private Metronome _gameMetronome;

    private IEnumerator<Vector3> _movePattern;

    public Utils.IObservable<int> PlayerLives => _playerLives;
    public Utils.IObservable<int> PlayerScore => _playerScore;
    public Utils.IObservable<int> WaveCount => _waveCount;

    private readonly Observable<int> _playerLives = new Observable<int>(3);
    private readonly Observable<int> _playerScore = new Observable<int>(0);
    private readonly Observable<int> _waveCount = new Observable<int>(0);

    partial void JumpToGameUiInEditor();

    private async void Start()
    {
      JumpToGameUiInEditor();

      _wave.WaveCleared += OnWaveCleared;
      _wave.PawnKilled += AddScore;
      _gameMetronome.Tick += OnEveryTick;
      _gameMetronome.OnEvery(10).Tick += OnEveryTenthTick;
      _player.Damaged += OnPlayerDamaged;
      _uiFacade.GameStopped += OnGameStop;

      await SpawnWave();
    }

    private void OnDestroy()
    {
      _wave.WaveCleared -= OnWaveCleared;
      _wave.PawnKilled -= AddScore;
      _gameMetronome.Tick -= OnEveryTick;
      _gameMetronome.OnEvery(10).Tick -= OnEveryTenthTick;
      _player.Damaged -= OnPlayerDamaged;
      _uiFacade.GameStopped -= OnGameStop;
    }

    IEnumerable<Vector3> MovePattern
    {
      get
      {
        Vector3 right = new Vector3(_field.CellWidth(), 0, 0);
        Vector3 down = new Vector3(0, 0, -_field.CellHeight());
        Vector3 left = new Vector3(-_field.CellWidth(), 0, 0);

        while (true)
        {
          yield return right;
          yield return right;
          yield return right;
          yield return right;
          yield return down;
          yield return left;
          yield return left;
          yield return left;
          yield return left;
          yield return down;
        }
      }
    }

    private void AddScore(int bounty)
    {
      _playerScore.Set(_playerScore+bounty);
    }

    private async void OnGameStop()
    {
      _gameMetronome.Stop(); //in case we run only the game scene
      await _sceneManager.UnloadGameScene();
    }

    private async void OnPlayerDamaged()
    {
      _playerLives.Set(_playerLives-1);
      if (_playerLives <= 0)
      {
        GameOver();
      }
      else
      {
        _player.IsInvincible = true;
        await UniTask.Delay(TimeSpan.FromSeconds(3)); //ToDo: Delay to config
        _player.IsInvincible = false;
      }
    }

    public async void GameOver()
    {
      _gameMetronome.Stop();
      _uiFacade.ShowNextView();
      await _leaderBoard.PublishScore(_playerScore);
      await _sceneManager.UnloadGameScene();
    }

    private async UniTask SpawnWave()
    {
      await _wave.Spawn();
      _gameMetronome.Bpm = 80;
      _movePattern = MovePattern.GetEnumerator();
      _gameMetronome.Start(0);
    }

    private async void OnWaveCleared()
    {
      _gameMetronome.Stop();
      _waveCount.Set(_waveCount+1);
      await SpawnWave();
    }

    //ToDo: parametrize

    private void OnEveryTenthTick()
    {
      _gameMetronome.Bpm += 20;
    }

    private async void OnEveryTick()
    {
      var MaxMoveTime = 0.25f;
      var moveTime = Mathf.Clamp(_gameMetronome.IntervalSeconds - 0.1f, 0.0f, MaxMoveTime);
      _movePattern.MoveNext();
      await _wave.MoveAll(_movePattern.Current, Mathf.Max(moveTime));
      if (_wave.AllowedToShoot)
      {
        _wave.Shoot();
      }
    }
  }
}
