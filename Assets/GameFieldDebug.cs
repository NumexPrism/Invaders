using Config;
using Mechanics.Field;
using UnityEngine;

public class GameFieldDebug : MonoBehaviour
{
  [SerializeField] private UnifiedConfigStorage config;

#if UNITY_EDITOR
  private void OnDrawGizmos()
  {
    if (!config)
    {
      return;
    }

    var field = config.Field;

    Debug.DrawLine(field.TopLeft(), field.TopRight());
    Debug.DrawLine(field.BottomLeft(), field.BottomRight());
    Debug.DrawLine(field.TopLeft(), field.BottomLeft());
    Debug.DrawLine(field.TopRight(), field.BottomRight());

    var bounds = field.GridBounds();

    Debug.DrawLine(new Vector3(bounds.xMin, 0 , bounds.yMin), new Vector3(bounds.xMin, 0 , bounds.yMax), Color.yellow);
    Debug.DrawLine(new Vector3(bounds.xMin, 0 , bounds.yMax), new Vector3(bounds.xMax, 0 , bounds.yMax), Color.yellow);
    Debug.DrawLine(new Vector3(bounds.xMax, 0 , bounds.yMax), new Vector3(bounds.xMax, 0 , bounds.yMin), Color.yellow);
    Debug.DrawLine(new Vector3(bounds.xMax, 0 , bounds.yMin), new Vector3(bounds.xMin, 0 , bounds.yMin), Color.yellow);

    for (int x = 1; x < field.Columns; x++)
    {
      var from = field.Cell(x, 0) + new Vector3(-field.CellWidth()/2,0,-field.CellHeight()/2);
      var to = field.Cell(x, field.Rows - 1) + new Vector3(-field.CellWidth()/2,0,field.CellHeight()/2);
      Debug.DrawLine(from, to, Color.yellow);
    }

    for (int y = 1; y < field.Rows; y++)
    {
      var from = field.Cell(0, y) + new Vector3(-field.CellWidth()/2,0,-field.CellHeight()/2);
      var to = field.Cell(field.Columns - 1, y) + new Vector3(field.CellWidth()/2,0,-field.CellHeight()/2);
      Debug.DrawLine(from, to, Color.yellow);
    }
  }
#endif
}