using UnityEngine;

namespace Mechanics.Projectiles
{
  internal interface IProjectileFactory
  {
    void Spawn(Vector3 transformPosition);
  }
}