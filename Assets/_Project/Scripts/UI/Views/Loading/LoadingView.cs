using Cysharp.Threading.Tasks;
using Installers.Project.UI.UiPanel;
using UnityEngine;

namespace UI.Views.Loading
{
  [RequireComponent(typeof(LoadingViewMonoInstaller))]
  internal class LoadingView: BaseUIView
  {
    public override async UniTask Hide()
    {
      //we never show loading screen after we close it. Why don't we free some memory then.
      await base.Hide();
      Destroy(gameObject);
    }
  }
}