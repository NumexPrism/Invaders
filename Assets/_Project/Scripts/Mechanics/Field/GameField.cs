using Mechanics.GameField;
using UnityEngine;
using Zenject;

namespace Mechanics.Field
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
      return Mathf.Clamp(position, _config.Left(), _config.Right());
    }

    public bool IsOutsideBounds(Vector3 position)
    {
      return position.z < _config.Bottom() || position.z > _config.Top() || position.x < -_config.Left() || position.x > _config.Right();
    }

    public Vector3 GetGridPosition(int x, int y)
    {
      return _config.Cell(x, y);
    }
  }
}