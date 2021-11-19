using Installers.Project.UI.UiPanel;
using UnityEngine;

namespace UI.Views.Loading
{
  [RequireComponent(typeof(LoadingViewMonoInstaller))]
  internal class LoadingView: BaseUIView
  {
    public override void Hide()
    {
      //we never show loading screen after we close it. Why don't we free some memory then.
      Destroy(gameObject);
    }
  }
}