using System;
using UnityEngine;
using Zenject;

namespace Mechanics.Enemy
{
  class SimpleEnemy: MonoBehaviour, IPoolable<EnemySpawnParameters, IMemoryPool>, IDisposable
  {
    private IMemoryPool _pool;

    internal class Factory : PlaceholderFactory<EnemySpawnParameters, SimpleEnemy> {}

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
