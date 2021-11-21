using Cysharp.Threading.Tasks;
using UI;
using UnityEngine;
using Zenject;

namespace Loading
{
  public class Starter : MonoBehaviour
  {
    [Inject] private IUiFacade _ui;
  
    void Start()
    {
      StartLoadingAsync();
    }

    private async void StartLoadingAsync()
    {
      Debug.Log("started");
      await DownloadResources();
      _ui.ShowNextView();
    }

    private async UniTask DownloadResources()
    {
      // not gonna lie, I never used never used addresables in production, previously good old bundles worked just fine.
      // I can't really test it and I'm definitely not gonna set up a CI environment to build the bundles or a CDN for the sake of a testtask.
      //
      // And i mean, how hard can that be. Addressable can check hashes and download the assets if they changed
      // and they can be set up to deliver collections with different resources for tablet/phone for example. Quite a powerful thing.
    
      await UniTask.Delay(3000);
    }
  }
}
