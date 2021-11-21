using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace AssetManagement
{
  internal class InvadersSceneManager
  {
    private const string Game = "GameLevel";

    public async UniTask LoadSingleGameScene()
    {
      //this snippet allows us to unload an old GameScene in editor, if we only work with game scene
      //because we should always have at least one loaded scene.
      //the other approach would be to use an empty scene during unloading.

      List<Scene> outdatedScenes = new List<Scene>();
      for (int i = 0; i < SceneManager.sceneCount; i++)
      {
        var outdatedScene = SceneManager.GetSceneAt(i);
        if (outdatedScene.name == Game)
        {
          outdatedScenes.Add(outdatedScene);
        }
      }

      await SceneManager.LoadSceneAsync(Game, LoadSceneMode.Additive);

      await UniTask.WhenAll(outdatedScenes.Select(s => SceneManager.UnloadSceneAsync(s).ToUniTask()));
    }

    public async UniTask UnloadGameScene()
    {
      if (SceneManager.sceneCount == 1)
      {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        return;
      }
      await SceneManager.UnloadSceneAsync(Game);
    }
  }
}