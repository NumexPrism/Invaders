using System;

namespace UI.Views.Game
{
  public class GameButtonsHandler
  {
    // I wanted to use the new InputSystem. it was all fine until I found out it just crashes on android.
    // since I don't have time to investigate, I have to manually wire UI elements
    //and manually process the inputs

    private bool _moveLeft = false;
    private bool _moveRight = false;

    public Action ShootCallback;
    public Action<float> MoveStartCallback;
    public Action MoveEndCallback;

    private float MoveSum {
      get
      {
        if(_moveLeft && _moveRight)
        {
          return 0.0f;
        }
        if(_moveLeft)
        {
          return -1.0f;
        }
        if(_moveRight)
        {
          return 1.0f;
        } 
        return 0.0f;
      }
    }

    public void LeftMoveReleased()
    {
      _moveLeft = false;
      MoveEndCallback();
    }

    public void LeftMovePressed()
    {
      _moveLeft = true;
      MoveStartCallback?.Invoke(MoveSum);
    }

    public void RightMoveReleased()
    {
      _moveRight = false;
      MoveEndCallback();
    }

    public void RightMovePressed()
    {
      _moveRight = true;
      MoveStartCallback?.Invoke(MoveSum);
    }

    public void ShootClicked()
    {
      ShootCallback?.Invoke();
    }
  }
}