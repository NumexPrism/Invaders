using System.ComponentModel;
using UnityEngine;

namespace Mechanics.Projectiles
{
  public readonly struct ProjectileLaunchParameters
  {
    public readonly Vector3 ShootPoint;
    public readonly Vector3 Velocity;
    public readonly Party Party;

    public ProjectileLaunchParameters(Vector3 shootPoint, Party party, IProjectileConfig config)
    {
      ShootPoint = shootPoint;
      Velocity = SelectDirection(party) * config.Speed;
      Party = party;
    }

    private static Vector3 SelectDirection(Party party)
    {
      switch (party)
      {
        case Party.Player: return Vector3.forward;
        case Party.Enemy: return Vector3.back;
        default: throw new InvalidEnumArgumentException($"{party} is not supported. We don't know which way tou shoot.");
      }
    }
  }
}
