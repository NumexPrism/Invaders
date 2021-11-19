using System;
using Cysharp.Threading.Tasks;
using Mechanics.Projectiles;
using UnityEngine;
using Zenject;

namespace Mechanics.Enemy
{
  class SimpleEnemy: MonoBehaviour, IPoolable<EnemySpawnParameters, IMemoryPool>, IDisposable, IHitByProjectile
  {
    internal class Factory : PlaceholderFactory<EnemySpawnParameters, SimpleEnemy> {}

    private IMemoryPool _pool;
    public Party Party => Party.Enemy;
    private LinearMovement _currentMovement;

    public event Action<SimpleEnemy> Destroyed;

    public void RespondToHit()
    {
      Destroyed?.Invoke(this);
      Dispose();
    }

    public void OnSpawned(EnemySpawnParameters spawnParameters, IMemoryPool pool)
    {
      transform.position = spawnParameters.position;
      _pool = pool;
    }

    public void OnDespawned()
    {
      _pool = null;
      transform.position = Vector3.zero;
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
      var movement = new LinearMovement(() => Time.time, duration, deltaMove);
      _currentMovement = movement;

      return UniTask.WaitUntil(() => movement.IsCompleted(), PlayerLoopTiming.LastUpdate);
    }
  }
}
