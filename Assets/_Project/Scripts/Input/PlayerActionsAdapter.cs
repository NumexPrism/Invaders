using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
  public class PlayerActionsAdapter : MonoBehaviour, IGameInput
  {
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