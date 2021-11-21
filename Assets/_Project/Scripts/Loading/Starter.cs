using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Zenject;

namespace Loading
{
  public class Starter : MonoBehaviour
  {
    [Inject] private IUiFacade _ui;
    [Inject] private InvadersSceneManager _invadersSceneManager;

    private SceneInstance _sceneInstance;

    void Start()
    {
      StartPreLoading();
    }

    private void StartPreLoading()
    {
      Addressables.InitializeAsync().Completed += OnPrelaodCompleted;
    }

    private void OnPrelaodCompleted(AsyncOperationHandle<IResourceLocator> operation)
    {
      if (operation.OperationException != null)
      {
        Debug.LogException(operation.OperationException);
      }
      else
      {
        _invadersSceneManager.ResourceLocator = operation.Result;
        _ui.ShowNextView();
      }
    }
  }
}
