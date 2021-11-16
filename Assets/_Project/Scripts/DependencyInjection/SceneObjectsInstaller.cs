using Config;
using Input;
using Mechanics.GameField;
using Mechanics.Player;
using Mechanics.Projectiles;
using Zenject;

namespace DependencyInjection
{
  class SceneObjectsInstaller:MonoInstaller<SceneObjectsInstaller>
  {
    public UnifiedConfigStorage ConfigStorage;
    public PlayerActionsAdapter ActionAdapter;
    public PlayerShip Ship;
    public Projectile ProjectilePrefab;
    public GameState GameState;

    public override void InstallBindings()
    {
      Container.Bind<IPlayerShipConfig>()
        .FromInstance(ConfigStorage.PlayerShip);
      Container.Bind<IGameFieldConfig>()
        .FromInstance(ConfigStorage.Field);
      Container.Bind<IProjectileConfig>()
        .WithId(Party.Player)
        .FromInstance(ConfigStorage.PlayerProjectileConfig);
      Container.Bind<IProjectileConfig>()
        .WithId(Party.Enemy)
        .FromInstance(ConfigStorage.EnemyProjectileConfig);

      Container.Bind<IGameField>()
        .To<GameField>()
        .AsSingle();

      Container.BindInstance<IGameInput>(ActionAdapter);

      Container.BindInstance<IPlayerShip>(Ship);

      Container.BindFactory<ProjectileLaunchParameters, Projectile, Projectile.Factory>()
        .FromMonoPoolableMemoryPool(x => x.WithInitialSize(2)
          .FromComponentInNewPrefab(ProjectilePrefab)
          .UnderTransformGroup("Projectiles"));

      Container.BindInstance(GameState);
      Container.Bind<WaitingState>().AsSingle();
      Container.Bind<PlayingState>().AsSingle();

     //Container.BindFactory<WaitingState, WaitingState.Factory>();
     //Container.BindFactory<PlayingState, PlayingState.Factory>();
     //Container.BindFactory<WaitingState, WaitingState.Factory>();
    }
  }
}