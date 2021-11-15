using UnityEngine;

namespace Mechanics.Projectiles
{
  class ProjectileFactory : IProjectileFactory
  {
    public void Spawn(Vector3 transformPosition)
    {
      Debug.Log("PEW PEW PEW");
    }
  }
}