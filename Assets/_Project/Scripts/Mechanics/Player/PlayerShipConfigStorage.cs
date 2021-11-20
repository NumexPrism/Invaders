using System;
using UnityEngine;

namespace Mechanics.Player
{
  [Serializable]
  public class PlayerShipConfig : IPlayerShipConfig
  {
    public float speed = 1.0f;
    public float shootDelay = 1.0f;
    [Header("cosmetics")]
    public float blinkDuration = 1.0f;
    public Color blinkColor = Color.red;

    public float Speed => speed;
    public float ShootDelay => shootDelay;
    public float BlinkDuration => shootDelay;
    public Color BlinkColor => blinkColor;
  }
}