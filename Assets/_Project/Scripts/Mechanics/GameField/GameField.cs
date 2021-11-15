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
      return Mathf.Clamp(position, -_config.Width / 2, _config.Width / 2);
    }
  }
}