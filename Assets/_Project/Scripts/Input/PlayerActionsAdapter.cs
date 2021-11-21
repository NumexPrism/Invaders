using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Input
{
  public class PlayerActionsAdapter : MonoBehaviour, IGameInput
  {
    [Inject] private DiContainer _container;

    private void OnEnable()
    {
      _container.Resolve<GameInputComposite>().Add(this);
    }

    private void OnDisable()
    {
      _container.Resolve<GameInputComposite>().Remove(this);
    }

    [UsedImplicitly]//called via unity events
    public void OnMove(InputAction.CallbackContext context)
    {
      if (context.valueType != typeof(float))
      {
        Debug.LogError($"valueType is {context.valueType}. but why?");
        return;
      }
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