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
    private readonly IGameFieldConfig _field;
    [Inject(Id = Party.Enemy)] private IProjectileConfig _projectileConfig;

    [Inject]public EnemyWave(
      SimpleEnemy.Factory enemyFactory,
      Projectile.Factory projectileFactory,
      IGameFieldConfig field)
    {
      _enemyFactory = enemyFactory;
      _projectileFactory = projectileFactory;
      _field = field;
      _alivePawns = new List<SimpleEnemy>();
    }

    private IProjectile activeProjectile;

    private int RemainingEnemies => _alivePawns.Count;
    private readonly List<SimpleEnemy> _alivePawns;

    public int WaveNumber { get; private set; }
    public bool AllowedToShoot => activeProjectile == null;

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
      activeProjectile = _projectileFactory.Create(parameters);
      activeProjectile.DeSpawning += OnProjectileDeSpawning;
    }

    private void OnProjectileDeSpawning(IProjectile deadProjectile)
    {
      deadProjectile.DeSpawning -= OnProjectileDeSpawning;
      activeProjectile = null;
    }

    public void OnPawnDestroyed(SimpleEnemy destroyedEnemy)
    {
      _alivePawns.Remove(destroyedEnemy);
      destroyedEnemy.Destroyed -= OnPawnDestroyed;
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

      //ToDo: ExtractParameters
      int w = 4;
      int h = 4;

      int topCell = _field.Rows - 4;

      Vector3 jumpInOffset = new Vector3(0, 0, _field.Height);

      float jumpDuration = 1.0f;

      for (int i = 0; i < w; i++)
      {
        for (int j = 0; j < h; j+=2)
        {
          var spawnParameters = new EnemySpawnParameters
          {
            position = _field.Cell(i, topCell - j) + jumpInOffset
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