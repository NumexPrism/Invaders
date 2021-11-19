using UnityEngine;
using Zenject;

namespace Installers.UI.UiPanel
{
  abstract class UiViewMonoInstaller<TDerived> : MonoInstaller<TDerived> where TDerived : MonoInstaller<TDerived>
  {
    public override void InstallBindings()
    {
      Container.Bind<CanvasGroup>().FromComponentSibling();
    }
  }
}