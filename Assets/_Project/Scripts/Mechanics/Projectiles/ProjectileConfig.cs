using System;

namespace Mechanics.Projectiles
{
  [Serializable]
  public class ProjectileConfig : IProjectileConfig
  {
    public float speed;
    public float Speed => speed;
  }
}