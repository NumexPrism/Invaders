using Loading;
using UI;
using Zenject;

namespace Mechanics.GameRules
{
  partial class GameController 
  {
#if UNITY_EDITOR
    [Inject] private IUiDebug _uiDebug;

    partial void JumpToGameUiInEditor()
    {
      //The code may be ugly, but the ability to just hit play button and work on a scene is really important for level-designers
      if (!FindObjectOfType<Starter>())
      {
        _uiDebug.ForceSwitchToGameUi();
      }
#endif
    }

    public void DebugGameOver()
    {
      GameOver();
    }
  }
}
