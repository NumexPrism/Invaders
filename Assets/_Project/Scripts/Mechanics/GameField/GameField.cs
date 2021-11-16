using UnityEngine;
using Zenject;

namespace Mechanics.GameField
{
  class GameField : IGameField
  {
    readonly IGameFieldConfig _config;

    public GameField([Inject]IGameFieldConfig config)
    {
      _config = config;
    }

    public float ClampPlayerPosition(float position)
    {
      return Mathf.Clamp(position, LeftBorder, RightBorder);
    }

    private float RightBorder => _config.Width / 2;
    private float LeftBorder => -_config.Width / 2;
    public float TopBorder => _config.Height;
    public float BottomBorder => 0.0f;

    public bool IsOutsideBounds(Vector3 position)
    {
      return position.z < BottomBorder || position.z > TopBorder || position.x < LeftBorder || position.x > RightBorder;
    }
  }
}