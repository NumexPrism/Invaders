using System;

namespace Mechanics.Player
{
  [Serializable]
  public class PlayerShipConfig : IPlayerShipConfig
  {
    public float speed = 1.0f;
    public float shootDelay = 1.0f;

    public float Speed => speed;
    public float ShootDelay => shootDelay;
  }
}