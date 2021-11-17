using DependencyInjection.UI;
using UnityEngine;

namespace UI
{
  [RequireComponent(typeof(GameEndViewMonoInstaller))]
  internal class GameEndView: BaseUIView
  {
    public bool IsEnabled => enabled;
  }
}