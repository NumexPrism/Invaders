using Mechanics.Enemy;
using Mechanics.GameField;
using UI;
using UnityEngine;
using Zenject;

namespace Mechanics
{
  class GameState : MonoBehaviour
  {
#if UNITY_EDITOR
    [Inject] private IUiDebug _uiDebug;
#endif

    [Inject] private SimpleEnemy.Factory _enemyFactory;
    [Inject] private IGameField _field;

    void JumpToGameUiInEditor()
    {
      //The code may be ugly, but the ability to just hit play button and work on a scene is really important for level-designers
#if UNITY_EDITOR
      if (!FindObjectOfType<Starter>())
      {
        _uiDebug.ForceSwitchToGameUi();
      }
#endif
    }

    private void Start()
    {
      JumpToGameUiInEditor();
      SpawnWave();
    }

    public void SetUp()
    {
      SpawnWave();
    }

    private void SpawnWave()
    {
      int w = 8;
      int h = 8;
      for (int i = 0; i < w; i++)
      {
        for (int j = 0; j < h; j+=2)
        {
          var spawnParameters = new EnemySpawnParameters
          {
            position = _field.GetGridPosition(i, j)
          };
          _enemyFactory.Create(spawnParameters);
        }
      }
    }

    public void Pause()
    { }

    public void GameOver()
    { }
  }
}
