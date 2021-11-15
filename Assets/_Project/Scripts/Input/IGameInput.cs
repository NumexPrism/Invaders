using System;

namespace Input
{
  public interface IGameInput
  {
    event Action<float> MoveStarted;
    event Action MoveStopped;
    event Action Shoot;
  }
}