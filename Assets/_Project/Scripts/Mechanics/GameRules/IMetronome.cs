using System;

namespace Mechanics.GameRules
{
  public interface IMetronome
  {
    event Action Tick;
  }
}