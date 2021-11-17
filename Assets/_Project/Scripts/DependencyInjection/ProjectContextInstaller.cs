using UI;
using UnityEngine;
using Zenject;

namespace DependencyInjection
{
  [CreateAssetMenu(fileName = "ProjectContextInstaller", menuName = "Installers/ProjectContextInstaller")]
  class ProjectContextInstaller: ScriptableObjectInstaller<ProjectContextInstaller>
  {
    [SerializeField] private UiFacade uiPrefab; 

    public override void InstallBindings()
    {
      Container.BindInterfacesTo<UiFacade>().FromComponentInNewPrefab(uiPrefab).AsSingle();
    }
  }
}
