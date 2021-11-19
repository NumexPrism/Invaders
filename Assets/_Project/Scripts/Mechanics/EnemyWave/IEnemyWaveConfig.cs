namespace Mechanics.Field
{
  public interface IEnemyWaveConfig
  {
    int Rows { get; }
    int RowsSpacing { get; }
    int EnemiesInRow { get; }
    int OffsetFromTop { get; }
    int ScorePerEnemy { get; }
    float SpawnTime { get; }
  }
}