using System;
using UnityEngine;

namespace Mechanics.Field
{
  static class GameFieldDerivedConfig
  {
    public static float Bottom(this IGameFieldConfig cfg)
    {
      return 0.0f;
    }

    public static float Top(this IGameFieldConfig cfg)
    {
      return cfg.Height;
    }

    public static float Left(this IGameFieldConfig cfg)
    {
      return -cfg.Width / 2.0f;
    }

    public static float Right(this IGameFieldConfig cfg)
    {
      return cfg.Width / 2.0f;
    }

    public static Vector3 BottomLeft(this IGameFieldConfig cfg)
    {
      return new Vector3(cfg.Left(), 0, cfg.Bottom());
    }

    public static Vector3 TopLeft(this IGameFieldConfig cfg)
    {
      return new Vector3(cfg.Left(), 0, cfg.Top());
    }

    public static Vector3 BottomRight(this IGameFieldConfig cfg)
    {
      return new Vector3(cfg.Right(), 0, cfg.Bottom());
    }

    public static Vector3 TopRight(this IGameFieldConfig cfg)
    {
      return new Vector3(cfg.Right(), 0, cfg.Top());
    }

    public static Rect GridBounds(this IGameFieldConfig cfg)
    {
      return new Rect(
        new Vector2(cfg.Left() - cfg.CellWidth() / 2, cfg.Bottom() - cfg.CellHeight() / 2),
        new Vector2(cfg.Width + cfg.CellWidth(), cfg.Height + cfg.CellHeight()));
    }

    /// <summary>
    /// center of the cell
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector3 Cell(this IGameFieldConfig cfg, int x, int y)
    {
      if (x >= cfg.Columns)
        throw new ArgumentOutOfRangeException("x");
      if (y >= cfg.Rows)
        throw new ArgumentOutOfRangeException("y");

      return new Vector3(
        cfg.Width / (cfg.Columns-1) * x + cfg.Left(), 
        0,
        cfg.Height / (cfg.Rows-1) * y + cfg.Bottom());
    }

    public static float CellWidth(this IGameFieldConfig cfg)
    {
      return cfg.Width / (cfg.Columns-1);
    }

    public static float CellHeight(this IGameFieldConfig cfg)
    {
      return cfg.Height / (cfg.Rows-1);
    }
  }
}