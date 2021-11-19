using AssetManagement;
using LeaderBoard;
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
      Container.Bind<IGameSession>().To<NullGameSession>().AsCached();
      Container.Bind<ILeaderBoardAdapter>().To<PlayerPrefsLeaderBoard>().AsCached();
      Container.Bind<IPlayerProfileAdapter>().To<StubPlayerProfile>().AsCached();
      Container.Bind<InvadersSceneManager>().AsSingle();
    }
  }
}
