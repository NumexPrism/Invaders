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
    public PlayerShip Ship;
    public PlayerActionsAdapter ActionAdapter;

    public override void InstallBindings()
    {
      Container.BindInterfacesTo<UnifiedConfigStorage>().FromInstance(ConfigStorage);

      Container.BindInstance<IPlayerShip>(Ship);
      Container.BindInstance<IGameInput>(ActionAdapter);
      Container.Bind<IProjectileFactory>().To<ProjectileFactory>().AsSingle();

      Container.Bind<IGameField>().To<GameField>().AsSingle();
    }
  }
}