using System;
using Mechanics.Field;
using UnityEngine;
using Zenject;

namespace Mechanics.Projectiles
{
  class Projectile : MonoBehaviour, IProjectile, IPoolable<ProjectileLaunchParameters, IMemoryPool>, IDisposable
  {
    private Vector3 _velocity;
    private Party _party;
    IMemoryPool _pool;

    [Inject] private IGameFieldConfig _gameField;

    internal class Pool: MonoMemoryPool<ProjectileLaunchParameters, Projectile> {}
    public class Factory:PlaceholderFactory<ProjectileLaunchParameters, Projectile>{}

    public event Action<IProjectile> DeSpawning;

    public void OnSpawned(ProjectileLaunchParameters projectileLaunchParameters, IMemoryPool pool)
    {
      Debug.Log("Projectile.CONSTRUCT");
      _pool = pool;
      transform.position = projectileLaunchParameters.ShootPoint;
      transform.rotation = Quaternion.LookRotation(projectileLaunchParameters.Velocity);
      _velocity = projectileLaunchParameters.Velocity;
      _party = projectileLaunchParameters.Party;
    }

    public void OnDespawned()
    {
      DeSpawning?.Invoke(this);
      _pool = null;
      _velocity = Vector3.zero;
      _party = Party.None;
    }

    private void Update()
    {
      if (_gameField.IsOutsideBounds(transform.position))
      {
        Dispose();
        return;
      }
      transform.position += _velocity * Time.deltaTime;
    }

    public void Dispose()
    {
      _pool.Despawn(this);
    }

    private void OnTriggerEnter(Collider other)
    {
      Debug.Log("HIT SMTHING");
      var canBeHit = other.GetComponent<IHitByProjectile>();
      if (canBeHit == null || canBeHit.Party == Party.None || canBeHit.Party == _party)
      {
        return;
      }
      canBeHit.RespondToHit();
      Dispose();
    }
  }
} 