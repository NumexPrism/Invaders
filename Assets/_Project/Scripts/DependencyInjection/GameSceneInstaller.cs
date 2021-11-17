using Config;
using Input;
using Mechanics;
using Mechanics.Enemy;
using Mechanics.Field;
using Mechanics.GameField;
using Mechanics.Player;
using Mechanics.Projectiles;
using Zenject;

namespace DependencyInjection
{
  class GameSceneInstaller:MonoInstaller<GameSceneInstaller>
  {
    public PlayerActionsAdapter ActionAdapter;
    public PlayerShip Ship;
    
    public GameState GameState;

    public override void InstallBindings()
    {
      Container.BindInstance<IGameInput>(ActionAdapter);
      Container.BindInstance<IPlayerShip>(Ship);
    }
  }
}