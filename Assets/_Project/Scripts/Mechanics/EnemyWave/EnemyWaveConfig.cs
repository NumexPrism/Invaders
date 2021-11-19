using System;
using UnityEngine;

namespace Mechanics.Field
{
  [Serializable]
  public class EnemyWaveConfig : IEnemyWaveConfig
  {
    [Min(0)] public int enemyScore = 10;
    [Min(1)] public int rows = 4;
    [Min(0)] public int rowsSpacing = 1;
    [Min(1)] public int enemiesInRow = 8;
    [Min(0)] public int offsetFromTop = 4;
    [Min(0.01f)] public float spawnTime = 1.0f;

    public int ScorePerEnemy => enemyScore; 
    public int Rows => rows;
    public int RowsSpacing => rowsSpacing;
    public int EnemiesInRow => enemiesInRow;
    public int OffsetFromTop => offsetFromTop;
    public float SpawnTime => spawnTime;
  }
}