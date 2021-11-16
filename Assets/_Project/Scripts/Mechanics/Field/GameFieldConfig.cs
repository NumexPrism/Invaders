using System;

namespace Mechanics.GameField
{
  [Serializable]
  public class GameFieldConfig : IGameFieldConfig
  {
    public float width = 1;
    public float height = 1;

    public float Height => height;
    public float Width => width;
  }
}