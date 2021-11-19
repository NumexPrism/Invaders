﻿using Config;
using Input;
using Mechanics;
using Mechanics.Enemy;
using Mechanics.Field;
using Mechanics.Player;
using Mechanics.Projectiles;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DependencyInjection
{
  class GameSceneInstaller:MonoInstaller<GameSceneInstaller>
  {
    [SerializeField] private PlayerActionsAdapter ActionAdapter;
    [SerializeField] private PlayerShip Ship;
    [SerializeField] private UnifiedConfigStorage ConfigStorage;
    [SerializeField] private Projectile ProjectilePrefab;
    [SerializeField] private SimpleEnemy EnemyPrefab;

    [FormerlySerializedAs("GameState")] public GameController gameController;

    public override void InstallBindings()
    {
      Container.BindInstance(this).WithId("GameSceneContainer");

      Container.BindInstance<IGameInput>(ActionAdapter);
      Container.BindInstance<IPlayerShip>(Ship);

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

      Container.BindInstance(gameController);
      Container.Bind<EnemyWave>().AsTransient();
      
      Container.BindFactory<ProjectileLaunchParameters, Projectile, Projectile.Factory>()
        .FromMonoPoolableMemoryPool(x => x.WithInitialSize(2)
          .FromComponentInNewPrefab(ProjectilePrefab)
          .UnderTransformGroup("Projectiles"));

      Container.BindFactory<EnemySpawnParameters, SimpleEnemy, SimpleEnemy.Factory>()
        .FromMonoPoolableMemoryPool(x => x.WithInitialSize(32)
          .FromComponentInNewPrefab(EnemyPrefab)
          .UnderTransformGroup("Enemies"));

      //I feel like I'm missing something out here, but i'm not sure what exactly.
      //There definitely should be a "clever way" to bind UI prefab context and game scene context
      //but I struggled with that for too much time by now. Will just use parent container, which is ProjectContext and bind it there.
      foreach (var parentContainer in Container.ParentContainers)
      {
        parentContainer.BindInstance(gameController);
      }
    }
  }
}