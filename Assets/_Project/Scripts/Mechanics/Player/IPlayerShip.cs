using System;

namespace Mechanics.Player
{
  public interface IPlayerShip
  {
    event Action Damaged;
    bool IsInvincible { get; set; }
  }
}