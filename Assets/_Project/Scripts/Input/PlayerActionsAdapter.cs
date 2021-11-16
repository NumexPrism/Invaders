using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
  public class PlayerActionsAdapter : MonoBehaviour, IGameInput
  {
    [UsedImplicitly]//called via unity events
    public void OnMove(InputAction.CallbackContext context)
    {
      var dir = context.action.ReadValue<float>();
      if (context.started)
      {
        MoveStarted?.Invoke(dir);
      }

      if (context.canceled)
      {
        MoveStopped?.Invoke();
      }
    }

    [UsedImplicitly]//called via unity events
    public void OnFire(InputAction.CallbackContext context)
    {
      if (context.performed)
      {
        Shoot?.Invoke();
      }
    }

    public event Action <float> MoveStarted;
    public event Action MoveStopped;
    public event Action Shoot;
  }
}