using System;
using Mechanics.Field;
using Zenject;

namespace Mechanics.Enemy
{
  internal class EnemyWave
  {
    private readonly SimpleEnemy.Factory _enemyFactory;
    private readonly IGameFieldConfig _field;

    [Inject]public EnemyWave(SimpleEnemy.Factory enemyFactory, IGameFieldConfig field)
    {
      _enemyFactory = enemyFactory;
      _field = field;
    }

    private int _remainingEnemies;

    public int WaveNumber { get; private set; }

    public event Action<int> WaveSpawned;
    public event Action WaveCleared;

    //it would be good to make it cached
    public void Restart()
    {
      WaveNumber = 0;
    }

    public void NotifyKilled()
    {
      _remainingEnemies--;
      if (_remainingEnemies == 0)
      {
        WaveCleared?.Invoke();
      }
    }

    public void Spawn()
    {
      //ToDo: ExtractParameters
      int w = 12;
      int h = 8;

      int topCell = _field.Rows - 4;

      for (int i = 0; i < w; i++)
      {
        for (int j = 0; j < h; j+=2)
        {
          var spawnParameters = new EnemySpawnParameters
          {
            position = _field.Cell(i, topCell - j)
          };
          _enemyFactory.Create(spawnParameters);
          _remainingEnemies++;
        }
      }

      WaveNumber++;
      WaveSpawned?.Invoke(WaveNumber);
    }
  }
}