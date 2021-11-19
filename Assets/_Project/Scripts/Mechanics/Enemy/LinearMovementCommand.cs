using System;
using UnityEngine;

namespace Mechanics.Enemy
{
  class LinearMovementCommand
  {
    //that class looks really similar to command pattern. To be a fullFledged pattern it only lacks "revert" functionality

    private readonly Func<float> CurrentTime;
    private readonly float _moveEndTime;
    private readonly float _duration;
    private readonly AnimationCurve _easingCurve;
    private readonly Vector3 _moveVector;
    private readonly float _moveStartTime;

    private float _prevTickValue;
    private bool wasCancelled;

    public LinearMovementCommand(Func<float> currentTime, float duration, Vector3 moveVector) : this(
      currentTime,
      duration,
      moveVector,
      AnimationCurve.EaseInOut(0, 0, 1, 1)
    ){}

    public LinearMovementCommand(Func<float> currentTime, float duration, Vector3 moveVector, AnimationCurve normalizedEasing)
    {
      wasCancelled = false;
      CurrentTime = currentTime;
      _moveStartTime = CurrentTime();
      _moveEndTime = _moveStartTime + duration;
      _duration = duration;
      _prevTickValue = 0.0f;
      _easingCurve = normalizedEasing;
      _moveVector = moveVector;
    }

    public void Cancel()
    {
      wasCancelled = true;
    }

    public Vector3 CalculateMoveStep()
    {
      var currentTickTime = CurrentTime() - _moveStartTime;
      var thisTickValue = _easingCurve.Evaluate(currentTickTime/_duration);
      float delta = thisTickValue - _prevTickValue;
      _prevTickValue = thisTickValue;
      return delta * _moveVector;
    }

    public bool IsCompleted()
    {
      return wasCancelled || CurrentTime() > _moveEndTime;
    }
  }
}