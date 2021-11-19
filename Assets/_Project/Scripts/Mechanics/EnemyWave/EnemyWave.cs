using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mechanics.Field;
using Mechanics.Projectiles;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Mechanics.Enemy
{
  internal class EnemyWave
  {
    private readonly SimpleEnemy.Factory _enemyFactory;
    private readonly Projectile.Factory _projectileFactory;
    private readonly IGameFieldConfig _fieldConfig;
    private readonly IProjectileConfig _projectileConfig;
    private readonly IEnemyWaveConfig _enemyConfig;

    [Inject]
    public EnemyWave(
      SimpleEnemy.Factory enemyFactory,
      Projectile.Factory projectileFactory,
      IGameFieldConfig fieldConfig,
      [Inject(Id = Party.Enemy)]IProjectileConfig projectileConfig,
      IEnemyWaveConfig enemyConfig)
    {
      _enemyFactory = enemyFactory;
      _projectileFactory = projectileFactory;
      _fieldConfig = fieldConfig;
      _projectileConfig = projectileConfig;
      _enemyConfig = enemyConfig;
      _alivePawns = new List<SimpleEnemy>();
    }

    private IProjectile _activeProjectile;

    private int RemainingEnemies => _alivePawns.Count;
    private readonly List<SimpleEnemy> _alivePawns;

    public int WaveNumber { get; private set; }
    public bool AllowedToShoot => _activeProjectile == null;

    public event Action<int> PawnKilled;
    public event Action<int> WaveSpawnCompleted;
    public event Action<int> WaveSpawnStarted;
    public event Action WaveCleared;

    //ToDo: it would be good to make it cached
    public void Restart()
    {
      WaveNumber = 0;
    }

    public void Shoot()
    {
      if (_alivePawns.Count == 0)
      {
        return;
      }

      var randomPawn = _alivePawns[Random.Range(0, _alivePawns.Count - 1)];//ToDo: make abstract selector
      var parameters = new ProjectileLaunchParameters(
        randomPawn.transform.position, //ToDo: add getter to shooter, to spawn projectiles not from ppivot
        randomPawn.Party,
        _projectileConfig);
      _activeProjectile = _projectileFactory.Create(parameters);
      _activeProjectile.DeSpawning += OnProjectileDeSpawning;
    }

    private void OnProjectileDeSpawning(IProjectile deadProjectile)
    {
      deadProjectile.DeSpawning -= OnProjectileDeSpawning;
      _activeProjectile = null;
    }

    public void OnPawnDestroyed(SimpleEnemy destroyedEnemy)
    {
      _alivePawns.Remove(destroyedEnemy);
      destroyedEnemy.Destroyed -= OnPawnDestroyed;
      PawnKilled?.Invoke(destroyedEnemy.PointsRewarded);
      if (RemainingEnemies == 0)
      {
        WaveCleared?.Invoke();
      }
    }

    public async UniTask Spawn()
    {
      foreach (var enemy in _alivePawns)
      {
        enemy.Dispose();
      }
      _alivePawns.Clear();

      int topCell = _fieldConfig.Rows - _enemyConfig.OffsetFromTop;

      Vector3 jumpInOffset = new Vector3(0, 0, _fieldConfig.Height);

      float jumpDuration = _enemyConfig.SpawnTime;

      for (int i = 0; i < _enemyConfig.EnemiesInRow; i++)
      {
        var rowStep = _enemyConfig.RowsSpacing+1;
        for (int j = 0; j < _enemyConfig.Rows*(_enemyConfig.RowsSpacing+1); j += rowStep)
        {
          var spawnParameters = new EnemySpawnParameters
          {
            Position = _fieldConfig.Cell(i, topCell - j) + jumpInOffset,
            PointsReward = _enemyConfig.ScorePerEnemy
          };
          var enemy = _enemyFactory.Create(spawnParameters);
          enemy.Destroyed += OnPawnDestroyed;
          _alivePawns.Add(enemy);
        }
      }

      WaveNumber++;
      WaveSpawnStarted?.Invoke(WaveNumber);
      await MoveAll(-jumpInOffset, jumpDuration);
      WaveSpawnCompleted?.Invoke(WaveNumber);
    }

    public async UniTask MoveAll(Vector3 moveVector, float moveTime)
    {
      await UniTask.WhenAll(ShipsMove(moveVector, moveTime));
    }

    private IEnumerable<UniTask> ShipsMove(Vector3 deltaMove, float time)
    {
      foreach (var enemy in _alivePawns)
      {
        yield return enemy.Move(deltaMove, time);
      }
    }
  }
}