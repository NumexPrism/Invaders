using System;

namespace Mechanics.Projectiles
{
  interface IProjectile
  {
    event Action<IProjectile> DeSpawning;
  }
}