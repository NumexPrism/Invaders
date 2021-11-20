using AssetManagement;
using LeaderBoard;
using Mechanics.GameRules;
using Mechanics.Player;
using UnityEngine;
using Zenject;

namespace Installers.Project
{
  [CreateAssetMenu(fileName = "ProjectContextInstaller", menuName = "Installers/ProjectContextInstaller")]
  class ProjectContextInstaller: ScriptableObjectInstaller<ProjectContextInstaller>
  {
    public override void InstallBindings()
    {
      Container.BindInterfacesAndSelfTo<GameSessionProxy>().AsCached();
      Container.Bind<ILeaderBoardAdapter>().To<PlayerPrefsLeaderBoard>().AsCached();
      Container.Bind<IPlayerProfileAdapter>().To<StubPlayerProfile>().AsCached();
      Container.Bind<InvadersSceneManager>().AsSingle();

      Container.Bind<Metronome>().AsTransient();
      Container.Bind<Metronome>().WithId("Global").AsCached();
      Container.Bind<Blinker>().AsTransient();
    }
  }
}
