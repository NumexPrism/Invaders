using AssetManagement;
using Mechanics.GameRules;
using UnityEngine;
using Zenject;

namespace Installers.Project
{
  [CreateAssetMenu(fileName = "ProjectContextInstaller", menuName = "Installers/ProjectContextInstaller")]
  class ProjectContextInstaller: ScriptableObjectInstaller<ProjectContextInstaller>
  {
    public override void InstallBindings()
    {
      Container.Bind<IGameSession>().To<NullGameSession>().AsSingle();
      Container.Bind<InvadersSceneManager>().AsSingle();
    }
  }
}
