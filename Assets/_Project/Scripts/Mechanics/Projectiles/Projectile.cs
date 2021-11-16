﻿using System;
using Mechanics.GameField;
using UnityEngine;
using Zenject;

namespace Mechanics.Projectiles
{
  class Projectile : MonoBehaviour, IProjectile, IPoolable<ProjectileLaunchParameters, IMemoryPool>, IDisposable
  {
    private Vector3 velocity;
    private Party party;
    IMemoryPool _pool;

    [Inject] private IGameField _gameField;

    internal class Pool: MonoMemoryPool<ProjectileLaunchParameters, Projectile> {}
    public class Factory:PlaceholderFactory<ProjectileLaunchParameters, Projectile>{}

    public void OnSpawned(ProjectileLaunchParameters projectileLaunchParameters, IMemoryPool pool)
    {
      Debug.Log("Projectile.CONSTRUCT");
      _pool = pool;
      transform.position = projectileLaunchParameters.ShootPoint;
      transform.rotation = Quaternion.LookRotation(projectileLaunchParameters.Velocity);
      velocity = projectileLaunchParameters.Velocity;
      party = projectileLaunchParameters.Party;
    }

    public void OnDespawned()
    {
      _pool = null;
      velocity = Vector3.zero;
      party = Party.None;
    }

    private void Update()
    {
      if (_gameField.IsOutsideBounds(transform.position))
      {
        Dispose();
        return;
      }
      transform.position += velocity * Time.deltaTime;
    }

    public void Dispose()
    {
      _pool.Despawn(this);
    }
  }
} 