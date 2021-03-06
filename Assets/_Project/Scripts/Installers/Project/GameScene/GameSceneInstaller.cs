using Config;
using Input;
using Mechanics.Enemy;
using Mechanics.EnemyWave;
using Mechanics.Field;
using Mechanics.GameRules;
using Mechanics.Player;
using Mechanics.Projectiles;
using UI;
using UnityEngine;
using Zenject;

namespace Installers.Project.GameScene
{
  class GameSceneInstaller:MonoInstaller<GameSceneInstaller>
  {
    [SerializeField] private PlayerShip Ship;
    [SerializeField] private UnifiedConfigStorage ConfigStorage;
    [SerializeField] private Projectile ProjectilePrefab;
    [SerializeField] private SimpleEnemy EnemyPrefab;

    [SerializeField] private GameController gameController;

    [SerializeField] [Header("Editor for editor")]private UiFacade UiPrefab;

    public override void InstallBindings()
    {
      Container.BindInstance(this).WithId("GameSceneContainer");

      Container.BindInstance<IPlayerShip>(Ship);

      Container.Bind<IPlayerShipConfig>()
        .FromInstance(ConfigStorage.PlayerShip);
      Container.Bind<IGameFieldConfig>()
        .FromInstance(ConfigStorage.Field);
      Container.Bind<IEnemyWaveConfig>()
        .FromInstance(ConfigStorage.enemiesWave);

      Container.Bind<IProjectileConfig>()
        .WithId(Party.Player)
        .FromInstance(ConfigStorage.PlayerProjectileConfig);
      Container.Bind<IProjectileConfig>()
        .WithId(Party.Enemy)
        .FromInstance(ConfigStorage.EnemyProjectileConfig);

      Container.Bind<EnemyWave>().AsTransient();
      
      Container.BindFactory<ProjectileLaunchParameters, Projectile, Projectile.Factory>()
        .FromMonoPoolableMemoryPool(x => x.WithInitialSize(2)
          .FromComponentInNewPrefab(ProjectilePrefab)
          .UnderTransformGroup("Projectiles"));

      Container.BindFactory<EnemySpawnParameters, SimpleEnemy, SimpleEnemy.Factory>()
        .FromMonoPoolableMemoryPool(x => x.WithInitialSize(32)
          .FromComponentInNewPrefab(EnemyPrefab)
          .UnderTransformGroup("Enemies"));

      Container.Bind<Renderer>()
        .FromComponentOn(Ship.gameObject)
        .WhenInjectedInto<PlayerShip>();

      //I feel like I'm missing something out here, but i'm not sure what exactly.
      //There definitely should be a "clever way" to bind UI prefab context and game scene context
      //but I struggled with that for too much time by now. Will just use parent container, which is ProjectContext and bind it there.
      foreach (var parentContainer in Container.ParentContainers)
      {
        parentContainer.BindInstance(gameController);
        parentContainer.Rebind<IGameSession>().FromInstance(gameController);

        InstallUIForDebugIfNeeded(parentContainer);
      }
    }

    private void OnDestroy()
    {
      //When Scene Unloads we reset binding to proxy
      foreach (var parentContainer in Container.ParentContainers)
      {
        var session = parentContainer.Resolve<IGameSession>();
        var proxy = parentContainer.Resolve<GameSessionProxy>();
        proxy.CopyFrom(session);
        parentContainer.Rebind<IGameSession>().FromInstance(proxy);
        parentContainer.Unbind<GameController>();
      }
    }

    private void InstallUIForDebugIfNeeded(DiContainer parentContainer)
    {
#if UNITY_EDITOR
      if (!parentContainer.HasBinding<UiFacade>())
      {
        parentContainer.Bind(typeof(IUiFacade), typeof(IUiDebug), typeof(UiFacade))
          .FromComponentInNewPrefab(UiPrefab).AsSingle();
      }
#endif
    }
  }
}