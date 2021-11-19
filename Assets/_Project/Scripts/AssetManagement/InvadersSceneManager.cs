using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AssetManagement
{
  internal class InvadersSceneManager
  {
    private const string Game = "GameLevel";

    public async UniTask LoadSingleGameScene()
    {
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
      foreach (var scene in outdatedScenes)
      {
        //no need to await unloading. 
        SceneManager.UnloadSceneAsync(scene);
      }
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