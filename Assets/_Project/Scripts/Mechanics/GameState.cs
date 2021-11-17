using Mechanics.Enemy;
using Mechanics.Field;
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
    [Inject] private EnemyWave _wave;
#endif

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
      _wave.WaveCleared += SpawnWave;
      SpawnWave();
    }

    public void SetUp()
    {
      SpawnWave();
    }

    private void SpawnWave()
    {
      _wave.Spawn();
    }

    public void Pause()
    { }

    public void GameOver()
    { }
  }
}
