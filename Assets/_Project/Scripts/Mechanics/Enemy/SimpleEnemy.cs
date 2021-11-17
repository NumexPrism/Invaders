using System;
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

    [Inject] private EnemyWave _enemyWave;

    public void ReceiveDamage()
    {
      _enemyWave.NotifyKilled();
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
      _pool.Despawn(this);
    }
  }
}
