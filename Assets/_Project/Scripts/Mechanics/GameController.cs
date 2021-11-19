using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mechanics.Enemy;
using Mechanics.Field;
using Mechanics.Player;
using UnityEngine;
using Zenject;

namespace Mechanics
{
  partial class GameController : MonoBehaviour
  {
    [Inject] private EnemyWave _wave;
    [Inject] private IGameFieldConfig _field;
    [Inject] private IPlayerShip _player;

    private Metronome _metronome;
    private IEnumerator<Vector3> _movePattern;

    public IObservable<int> PlayerLives => _playerLives;
    public IObservable<int> PlayerScore => _playerScore;
    public IObservable<int> WaveCount => _waveCount;

    private readonly Observable<int> _playerLives = new Observable<int>(3);
    private readonly Observable<int> _playerScore = new Observable<int>(0);
    private readonly Observable<int> _waveCount = new Observable<int>(0);

    partial void JumpToGameUiInEditor();

    private void Start()
    {
      JumpToGameUiInEditor();
      _wave.WaveCleared += OnWaveCleared;
      _metronome = new Metronome();

      _metronome.Tick += OnEveryTick;
      _metronome.OnEvery(10).Tick += OnEveryTenthTick;

      _player.Damaged += OnPlayerDamaged;

      //ToDo: move away from start, wait input instead
      SpawnWave();
    }

    private async void OnPlayerDamaged()
    {
      _playerLives.Set(_playerLives-1); 
      _player.IsInvincible = true;
      await UniTask.Delay(TimeSpan.FromSeconds(3)); //ToDo: Delay to config
      _player.IsInvincible = false;
    }

    private async UniTask SpawnWave()
    {
      Debug.Log("ToDo: DisableInputs");
      await _wave.Spawn();
      Debug.Log("ToDo: EnableInputs");
      _metronome.Bpm = 80;
      _movePattern = MovePattern.GetEnumerator();
      _metronome.Start(0);
    }

    private async void OnWaveCleared()
    {
      _metronome.Stop();
      _waveCount.Set(_waveCount+1);
      await SpawnWave();
    }

    //ToDo: parametrize
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

    private void OnEveryTenthTick()
    {
      _metronome.Bpm += 20;
    }

    private async void OnEveryTick()
    {
      var MaxMoveTime = 0.25f;
      var moveTime = Mathf.Clamp(_metronome.IntervalSeconds - 0.1f, 0.0f, MaxMoveTime);
      _movePattern.MoveNext();
      await _wave.MoveAll(_movePattern.Current, Mathf.Max(moveTime));
      if (_wave.AllowedToShoot)
      {
        _wave.Shoot();
      }
    }
  }
}
