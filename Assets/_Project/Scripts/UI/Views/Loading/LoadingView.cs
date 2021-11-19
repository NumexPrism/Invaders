using Installers.UI.UiPanel;
using UnityEngine;

namespace UI.Views.Loading
{
  [RequireComponent(typeof(LoadingViewMonoInstaller))]
  internal class LoadingView: BaseUIView
  {
    private void OnEnable()
    {
      Debug.Log("ENABLED!!!!");
    }
    private void OnDisable()
    {
      Debug.Log("DESABLED !!!!");
    }
  }
}