using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mechanics.Field
{
  [Serializable]
  public class GameFieldConfig : IGameFieldConfig
  {
    [Min(0.001f)]
    public float width = 1;
    [Min(0.001f)]
    public float height = 1;
    [FormerlySerializedAs("gridWidth")] [Min(2)]
    public int rows = 16;
    [FormerlySerializedAs("gridHeight")] [Min(2)]
    public int columns = 16;

    public float Width => width;
    public float Height => height;
    public int Rows => rows;
    public int Columns => columns;
  }
}