using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace AssetManagement
{
  internal class InvadersSceneManager
  {
    private const string Game = "GameLevel";

    private SceneInstance _loadedScene;

    public IResourceLocator ResourceLocator;

    private bool _isAllowedToLoadScene = true;

    public async Task LoadSingleGameScene()
    {
      if (!ResourceLocator.Locate(Game, typeof(SceneInstance), out var locations)) 
      {
        Debug.LogError("can't locate ");
        return;
      }

      if (!_isAllowedToLoadScene)
      {
        return;
      }
      _isAllowedToLoadScene = false;
      _loadedScene = await Addressables.LoadSceneAsync(locations[0], LoadSceneMode.Additive).Task;
    }

    public async Task UnloadGameScene()
    {
#if UNITY_EDITOR
      if (SceneManager.sceneCount == 1)
      {
        UnityEditor.EditorApplication.isPlaying = false;
        return;
      }
#endif
      await Addressables.UnloadSceneAsync(_loadedScene).Task;
      _isAllowedToLoadScene = true;
    }
  }
}