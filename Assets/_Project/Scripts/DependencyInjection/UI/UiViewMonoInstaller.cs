using UI;
using UnityEngine;
using Zenject;

namespace DependencyInjection
{
  abstract class UiViewMonoInstaller<TDerived> : MonoInstaller<TDerived> where TDerived : MonoInstaller<TDerived>
  {
    public override void InstallBindings()
    {
      Container.Bind<CanvasGroup>().FromComponentSibling();
    }
  }
}