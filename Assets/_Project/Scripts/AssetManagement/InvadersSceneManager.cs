using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AssetManagement
{
  internal class InvadersSceneManager
  {
    private const string Game = "GameLevel";

    public async UniTask LoadGameScene()
    {
      await SceneManager.LoadSceneAsync(Game, LoadSceneMode.Additive);
    }
    public async UniTask UnloadGameScene()
    {
      if (SceneManager.sceneCount == 1)
      {
        Debug.LogWarning("The only opened scene can't be unloaded.");
        return;
      }
      await SceneManager.UnloadSceneAsync(Game);
    }
  }
}