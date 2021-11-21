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
      // I can't really test it and I'm definitely not gonna set up a CI environment to build the bundles or a CDN for the sake of a testTask.
      //
      // The task "make everyting downloadable as much as possible" is really weird one.
      // Packaging startegy should be designed carefully, because it usually implies
      // quite significant limitations on the artists and gamedesigners
      //
      // I did grouped the adressables, in an almost meaningfull way. Now it's uses localPackages and loads them on the fly
      // it's quite easy to set it up to load from web
      //
      // and if the memory footprint is a concern, it is possible to change direct resource dependencies to assetDependencies
      // and to unload them on the fly.
      await UniTask.Delay(1000);
    }
  }
}
