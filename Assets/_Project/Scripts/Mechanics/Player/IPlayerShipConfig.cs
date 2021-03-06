using UnityEngine;

namespace Mechanics.Player
{
  internal interface IPlayerShipConfig
  {
    float Speed { get; }
    float ShootDelay { get; }

    float BlinkDuration { get; }
    Color BlinkColor { get;}
  }
}