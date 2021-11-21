using System;

namespace Input
{
  public class GameInputComposite: IGameInput
  {
    //I need the inputCompositor now to work with both inputs in editor
    
    public event Action<float> MoveStarted;
    public event Action MoveStopped;
    public event Action Shoot;

    public void Add(IGameInput input)
    {
      input.MoveStarted += InvokeMoveStarted;
      input.MoveStopped += InvokeMoveStopped;
      input.Shoot += InvokeShoot;
    }

    public void Remove(IGameInput input)
    {
      input.MoveStarted -= InvokeMoveStarted;
      input.MoveStopped -= InvokeMoveStopped;
      input.Shoot -= InvokeShoot;
    }

    private void InvokeShoot()
    {
      Shoot?.Invoke();
    }

    private void InvokeMoveStopped()
    {
      MoveStopped?.Invoke();
    }

    private void InvokeMoveStarted(float dir)
    {
      MoveStarted?.Invoke(dir);
    }
  }
}