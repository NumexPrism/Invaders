using System;
using Cysharp.Threading.Tasks;
using Mechanics.Field;
using Mechanics.GameRules;
using Mechanics.Projectiles;
using UnityEngine;
using Zenject;

namespace Mechanics.Enemy
{
  class SimpleEnemy: MonoBehaviour, IPoolable<EnemySpawnParameters, IMemoryPool>, IDisposable, IHitByProjectile
  {
    internal class Factory : PlaceholderFactory<EnemySpawnParameters, SimpleEnemy> {}

    [Inject] private IGameFieldConfig _fieldConfig;
    [Inject] private GameController _GameSession;

    private IMemoryPool _pool;
    public Party Party => Party.Enemy;
    public int PointsRewarded { get; private set; }

    private LinearMovementCommand _currentMovement;

    public event Action<SimpleEnemy> Destroyed;

    public void RespondToHit()
    {
      Destroyed?.Invoke(this);
      Dispose();
    }

    public void OnSpawned(EnemySpawnParameters spawnParameters, IMemoryPool pool)
    {
      transform.position = spawnParameters.Position;
      PointsRewarded = spawnParameters.PointsReward;
      _pool = pool;
    }

    public void OnDespawned()
    {
      _pool = null;
      transform.position = Vector3.zero;
      PointsRewarded = 0;
    }

    public void Dispose()
    {
      _currentMovement?.Cancel();
      _currentMovement = null;
      _pool.Despawn(this);
    }

    private bool IsMoving => _currentMovement != null && !_currentMovement.IsCompleted();

    private void Update()
    {
      if (IsMoving)
      {
        transform.position += _currentMovement.CalculateMoveStep();
      }
      if (transform.position.z < _fieldConfig.Bottom())
      {
        _GameSession.GameOver();
      }
    }

    public UniTask Move(Vector3 deltaMove, float duration)
    {
      if (duration < -Mathf.Epsilon)
      {
        throw new ArgumentException($"Duration should be 0 or more seconds, but was {duration}", nameof(duration));
      }

      if (duration <= Mathf.Epsilon)
      {
        //zero duration - instant move
        transform.position += deltaMove;
        return UniTask.CompletedTask;
      }

      _currentMovement?.Cancel();

      //ToDo: Inject linearMovement
      var movement = new LinearMovementCommand(() => Time.time, duration, deltaMove);
      _currentMovement = movement;

      return UniTask.WaitUntil(() => movement.IsCompleted(), PlayerLoopTiming.LastUpdate);
    }
  }
}
