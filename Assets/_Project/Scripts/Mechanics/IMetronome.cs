using System;

namespace Mechanics
{
  public interface IMetronome
  {
    event Action Tick;
  }
}